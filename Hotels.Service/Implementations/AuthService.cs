using AutoMapper;
using Hotels.Models.Dto;
using Hotels.Models.Dto.Identity;
using Hotels.Models.Entities;
using Hotels.Repository.Data;
using Hotels.Service.Exceptions;
using Hotels.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HotelsDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        private const string _AdminRole = "Admin";
        private const string _GuestRole = "Guest";
        private const string _ManagerRole = "Manager";
        public AuthService(HotelsDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper) 
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUser.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            if (user is not null)
            {
                var isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (!isValid)
                {
                    return new LoginResponseDto() { Token = string.Empty };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtTokenGenerator.GenerateToken(user, roles);

                //UserDto userDto = new()
                //{
                //    Email = user.Email,
                //    Id = user.Id,
                //    FullName = user.UserName,
                //};

                LoginResponseDto result = new()
                {
                  //  User= userDto,
                    Token = token
                };

                if (result is null)
                {
                    throw new InvalidOperationException(loginRequestDto.UserName);
                }

                return result;
            }
            else
            {
                throw new NotFoundException($"User {loginRequestDto.UserName} not found");
            }
        }

        public async Task Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = _mapper.Map<ApplicationUser>(registrationRequestDto);

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = await _context.ApplicationUser.FirstAsync(x => x.Email.ToLower() == registrationRequestDto.Email.ToLower());

                if (userToReturn != null)
                {
                    if (!await _roleManager.RoleExistsAsync(_GuestRole))
                        await _roleManager.CreateAsync(new IdentityRole(_GuestRole));

                    await _userManager.AddToRoleAsync(userToReturn, _GuestRole);
                }
            }
            else
            {
                throw new InvalidOperationException(result.Errors.FirstOrDefault().Description);
            }
        }
        public async Task RegisterAdmin(RegistrationRequestDto registrationRequestDto)
        {
            var user = _mapper.Map<ApplicationUser>(registrationRequestDto);

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = await _context.ApplicationUser.FirstAsync(x => x.Email.ToLower() == registrationRequestDto.Email.ToLower());

                if (userToReturn != null)
                {
                    if (!await _roleManager.RoleExistsAsync(_AdminRole))
                        await _roleManager.CreateAsync(new IdentityRole(_AdminRole));

                    await _userManager.AddToRoleAsync(userToReturn, _AdminRole);
                }
            }
            else
            {
                throw new InvalidOperationException(result.Errors.FirstOrDefault().Description);
            }
        }

        public async Task RegisterManager(RegistrationRequestDto registrationRequestDto)
        {
            var user = _mapper.Map<ApplicationUser>(registrationRequestDto);

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = await _context.ApplicationUser.FirstAsync(x => x.Email.ToLower() == registrationRequestDto.Email.ToLower());

                if (userToReturn != null)
                {
                    if (!await _roleManager.RoleExistsAsync(_ManagerRole))
                        await _roleManager.CreateAsync(new IdentityRole(_ManagerRole));

                    await _userManager.AddToRoleAsync(userToReturn, _ManagerRole);
                }
            }
            else
            {
                throw new InvalidOperationException(result.Errors.FirstOrDefault().Description);
            }
        }
    }
}
