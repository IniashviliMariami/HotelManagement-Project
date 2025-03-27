using AutoMapper;
using Hotels.Models.Dto.Room;
using Hotels.Models.Entities;
using Hotels.Repository.Interfaces;
using Hotels.Service.Exceptions;
using Hotels.Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly ISavable _savable;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IReservationRepository reservationRepository,
                           IHotelRepository hotelRepository, ISavable savable, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _reservationRepository = reservationRepository;
            _hotelRepository = hotelRepository;
            _savable = savable;
            _mapper = mapper;
        }
        public async Task AddRoomAsync(RoomForCreateDto roomDto)
        {
            if (roomDto.Price <= 0) 
            { 
                throw new NotFoundException("Room price must be greater than zero.");
            }
            var hotel = await _hotelRepository.GetAsync(h => h.Id == roomDto.HotelId);
            if (hotel == null)
            {
                throw new NotFoundException("Hotel not found.");
            }
                

            var room = _mapper.Map<Room>(roomDto);
            await _roomRepository.AddAsync(room);
            
        }

        public async Task DeleteRoomAsync(int roomId)
        {
            var room = await _roomRepository.GetAsync(r => r.Id == roomId);
            if (room == null)
                throw new InvalidOperationException("Room not found.");

            var activeReservations = await _reservationRepository.GetAllAsync(r => r.RoomId == roomId && r.CheckOut >= DateTime.UtcNow);
            if (activeReservations.Any())
                throw new InvalidOperationException("Cannot delete room with active reservations.");

            _roomRepository.Remove(room);
        }

        public async Task<List<RoomForGettingDto>> GetAllRoomsAsync(RoomForFilterDto filter)
        {
            var rooms = await _roomRepository.GetAllAsync(r =>
            (filter.HotelId == null || r.HotelId == filter.HotelId) &&
            (filter.IsAvailable == null || r.IsAvailable == filter.IsAvailable) &&
            (filter.MinPrice == null || r.Price >= filter.MinPrice) &&
            (filter.MaxPrice == null || r.Price <= filter.MaxPrice)
        );

            return _mapper.Map<List<RoomForGettingDto>>(rooms);
        }

        public  async Task<RoomForGettingDto> GetRoomByIdAsync(int roomId)
        {
            var room = await _roomRepository.GetAsync(r => r.Id == roomId);
            if (room == null)
                throw new InvalidOperationException("Room not found.");

            return _mapper.Map<RoomForGettingDto>(room);
        }

        public async Task UpdateRoomAsync(RoomForUpdateDto roomDto)
        {
            var room = await _roomRepository.GetAsync(r => r.Id == roomDto.Id);
            if (room == null)
                throw new InvalidOperationException("Room not found.");

            if (roomDto.Price <= 0)
                throw new InvalidOperationException("Room price must be greater than zero.");

            room.Name = roomDto.Name;
            room.IsAvailable = roomDto.IsAvailable;
            room.Price = roomDto.Price;

            await _roomRepository.Update(room);
        }
        public async Task SaveRoom() => await _roomRepository.Save();
    }
}
