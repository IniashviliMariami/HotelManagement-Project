using AutoMapper;
using Hotels.Models.Dto.Manager;
using Hotels.Models.Entities;
using Hotels.Repository.Interfaces;
using Hotels.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class ManagerService:IManagerService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ISavable _savable;
        private readonly IMapper _mapper;

        public ManagerService(IManagerRepository managerRepository, IHotelRepository hotelRepository, ISavable savable, IMapper mapper)
        {
            _managerRepository = managerRepository;
            _hotelRepository = hotelRepository;
            _savable = savable;
            _mapper = mapper;
        }

        public  async Task AddManagerAsync(ManagerForCreateDto managerDto)
        {


            var managers = await _managerRepository.GetAllAsync(m => m.PersonalNumber == managerDto.PersonalNumber || m.Email == managerDto.Email);
            if (managers.Any())
            {
                throw new InvalidOperationException("Manager with the same personal number or email already exists.");
            }
            var manager = _mapper.Map<Manager>(managerDto);
          
            await _managerRepository.AddAsync(manager);
        }

       

        public async Task DeleteManagerAsync(int managerId)
        {
            var manager = await _managerRepository.GetAsync(m => m.Id == managerId, includeProperties: "Hotel");

            if (manager == null)
            {
                throw new InvalidOperationException("Manager not found.");
            }

            if (manager.Hotel != null)
            {
               
                var hotelManagers = await _managerRepository.GetAllAsync(m => m.HotelId == manager.HotelId && m.Id != managerId);

                
                if (!hotelManagers.Any())
                {
                    throw new InvalidOperationException("Hotel must have at least one manager.");
                }
            }

            
            _managerRepository.Remove(manager);
        }

        public async Task<List<ManagerForGettingDto>> GetAllManagersAsync()
        {
            var managers = await _managerRepository.GetAllAsync();
            return _mapper.Map<List<ManagerForGettingDto>>(managers);
        }

        public async Task SaveManager() => await _managerRepository.Save();

        public async Task UpdateManagerAsync(ManagerForUpdateDto managerDto)
        {
            var manager = await _managerRepository.GetAsync(m => m.Id == managerDto.Id);
            if (manager == null)
                throw new InvalidOperationException("Manager not found.");

          
            var existingManager = await _managerRepository.GetAsync(m => m.HotelId == managerDto.HotelId && m.Id != managerDto.Id);
            if (existingManager != null)
                throw new InvalidOperationException("This hotel already has a manager assigned.");

            manager.FirstName = managerDto.FirstName;
            manager.LastName = managerDto.LastName;
            manager.Email = managerDto.Email;
            manager.PhoneNumber = managerDto.PhoneNumber;
            manager.HotelId = managerDto.HotelId;
        }

        
    }
}
