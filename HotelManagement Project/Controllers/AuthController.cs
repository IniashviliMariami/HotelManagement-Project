using Hotels.Models.Dto.Identity;
using Hotels.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement_Project.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);
            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegistrationRequestDto model)
        {
            await _authService.Register(model);
            return Created();
        }

        [HttpPost("registeradmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromForm] RegistrationRequestDto model)
        {
            await _authService.RegisterAdmin(model);
            return Created();
        }

        [HttpPost("RegisterManager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RegisterManager([FromForm] RegistrationRequestDto model)
        {
            await _authService.RegisterManager(model);
            return Created();
        }

    }
}
