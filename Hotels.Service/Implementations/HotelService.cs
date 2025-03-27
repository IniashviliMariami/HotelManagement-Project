using AutoMapper;
using Hotels.Models.Dto.Hotel;
using Hotels.Models.Entities;
using Hotels.Repository.Implementations;
using Hotels.Repository.Interfaces;
using Hotels.Service.Exceptions;
using Hotels.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class HotelService : IHotelService
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ISavable _savable;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelRepository, IRoomRepository roomRepository,
                            IReservationRepository reservationRepository, ISavable savable, IMapper mapper, IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
            _savable = savable;
            _mapper = mapper;
        }
        public async Task AddHotelAsync(HotelForCreateDto hotelDto)
        {
            if (hotelDto.Rating < 1 || hotelDto.Rating > 5)
           {
                throw new NotFoundException("The rating should be between 1 and 5 inclusive");
            }


           var manager = await _managerRepository.GetAsync(m => m.Id == hotelDto.ManagerId);
            if (manager == null)
            {
                throw new ArgumentException("This manager already has a hotel assigned.");
            }

            var hotel = _mapper.Map<Hotel>(hotelDto);

           hotel.ManagerId = manager.Id;
            

          await _hotelRepository.AddAsync(hotel);


            manager.HotelId = hotel.Id;
            await _managerRepository.Update(manager);
        }
       

        public async Task DeleteHotelAsync(int hotelId)
        {
            var hotel = await _hotelRepository.GetAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }
                
            var activeRooms = await _roomRepository.GetAllAsync(r => r.HotelId == hotelId);
          
            var activeReservations = await _reservationRepository.GetAllAsync(r => r.Room.HotelId == hotelId && r.CheckOut >= DateTime.UtcNow);

            if (activeRooms.Any() || activeReservations.Any())
            {
                throw new NotFoundException("Cannot delete hotel with active rooms or reservations.");
            }
            var manager = await _managerRepository.GetAsync(m => m.HotelId == hotelId);

            if (manager != null && hotel.ManagerId == manager.Id)
            {
                
                _managerRepository.Remove(manager);
            }
          

            _hotelRepository.Remove(hotel);
        }

        public async Task<List<HotelForGettingDto>> GetAllHotelsAsync(string country, string city, int? rating)
        {
            var hotels = await _hotelRepository.GetAllAsync(h =>
            (string.IsNullOrEmpty(country) || h.Country == country) &&
            (string.IsNullOrEmpty(city) || h.City == city) &&
            (!rating.HasValue || h.Rating == rating.Value)
            );
         
            return _mapper.Map<List<HotelForGettingDto>>(hotels);
        }

        public  async Task<HotelForGettingDto> GetHotelAsync(int hotelId)
        {
            var hotel = await _hotelRepository.GetAsync(h => h.Id == hotelId);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }
                

            return _mapper.Map<HotelForGettingDto>(hotel);
        }

        public async Task UpdateHotelAsync(HotelForUpdateingDto hotelDto)
        {
            var hotel = await _hotelRepository.GetAsync(h => h.Id == hotelDto.Id);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }
                

            if (hotelDto.Rating < 1 || hotelDto.Rating > 5)
            {

                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            hotel.Name = hotelDto.Name;
            hotel.Address = hotelDto.Address;
            hotel.Rating = hotelDto.Rating;

           await _hotelRepository.Update(hotel);
           
        }
        public async Task SaveHotel() => await _hotelRepository.Save();

     
    }
}
