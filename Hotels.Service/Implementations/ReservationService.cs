using AutoMapper;
using Hotels.Models.Dto.Guest;
using Hotels.Models.Dto.Hotel;
using Hotels.Models.Dto.Reservation;
using Hotels.Models.Dto.Room;
using Hotels.Models.Entities;
using Hotels.Repository.Implementations;
using Hotels.Repository.Interfaces;
using Hotels.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class ReservationService :IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IGuestRepository _guestRepository;
        private readonly IMapper _mapper;
        private readonly IGuestReservationRepository _guestReservationRepository;
        private readonly IHotelRepository _hotelRepository;


        public ReservationService(IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IGuestRepository guestRepository,
            IMapper mapper,
            IGuestReservationRepository guestReservationRepository,
            IHotelRepository hotelRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _guestRepository = guestRepository;
            _mapper = mapper;
            _guestReservationRepository = guestReservationRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task CancelReservationAsync(int reservationId)
        {

            var reservation = await _reservationRepository.GetAsync(r => r.Id == reservationId);
            if (reservation == null)
                throw new InvalidOperationException("Reservation not found.");

            var room = await _roomRepository.GetAsync(r => r.Id == reservation.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                reservation.IsActive = true;
                await _roomRepository.Update(room);
            }

            _reservationRepository.Remove(reservation);

        }

        public async Task CreateReservationAsync(ReservationForCreateDto reservationDto)
        {
            var room = await _roomRepository.GetAsync(r => r.Id == reservationDto.RoomId && r.IsAvailable);
            if (room == null)
                throw new InvalidOperationException("Room is not available.");

            if (reservationDto.CheckIn < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Check-in date must be today or in the future.");

            if (reservationDto.CheckOut <= reservationDto.CheckIn)
                throw new InvalidOperationException("Check-out date must be later than check-in date.");

            var conflicts = await _reservationRepository.GetAllAsync(r =>
                r.RoomId == reservationDto.RoomId &&
                ((reservationDto.CheckIn >= r.CheckIn && reservationDto.CheckIn < r.CheckOut) ||
                 (reservationDto.CheckOut > r.CheckIn && reservationDto.CheckOut <= r.CheckOut)));

            if (conflicts.Any())
                throw new InvalidOperationException("Reservation dates conflict with existing reservations.");

            
            var reservation = _mapper.Map<Reservation>(reservationDto);
            reservation.IsActive = true;

           
            await _reservationRepository.AddAsync(reservation);
           await _reservationRepository.Save();  

            
            var guestReservation = new GuestReservation
            {
                GuestId = reservationDto.GuestId,
                ReservationId = reservation.Id
            };
            await _guestReservationRepository.AddAsync(guestReservation);

           
            room.IsAvailable = false;
            await _roomRepository.Update(room);

            await _guestReservationRepository.SaveAsync();
        }

        public async Task<List<ReservationForGettingDto>> GetReservationsAsync(ReservationFilterDto filter)
        {

            var reservations = await _reservationRepository.GetAllAsync(
                r => (filter.RoomId == null || r.RoomId == filter.RoomId) &&
                    (filter.StartDate == null || r.CheckIn >= filter.StartDate) &&
                    (filter.EndDate == null || r.CheckOut <= filter.EndDate) &&
                    (filter.IsActive == null || (filter.IsActive.Value
                        ? r.CheckOut >= DateTime.UtcNow
                        : r.CheckOut < DateTime.UtcNow)),
                includeProperties: "Room,GuestReservations.Guest"
            );

            return _mapper.Map<List<ReservationForGettingDto>>(reservations);
        }

      
        public async Task SaveReservation() => await _reservationRepository.Save();

        public async Task UpdateReservationAsync(ReservationForUpdateDto reservationDto)
        {

            var reservation = await _reservationRepository.GetAsync(r => r.Id == reservationDto.Id);
            if (reservation == null)
                throw new InvalidOperationException("Reservation not found.");

            if (reservationDto.CheckIn < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Check-in date must be today or in the future.");

            if (reservationDto.CheckOut <= reservationDto.CheckIn)
                throw new InvalidOperationException("Check-out date must be later than check-in date.");


            var conflicts = await _reservationRepository.GetAllAsync(r =>
                r.RoomId == reservation.RoomId && r.Id != reservation.Id &&
                ((reservationDto.CheckIn >= r.CheckIn && reservationDto.CheckIn < r.CheckOut) ||
                 (reservationDto.CheckOut > r.CheckIn && reservationDto.CheckOut <= r.CheckOut)));

            if (conflicts.Any())
                throw new InvalidOperationException("New dates conflict with existing reservations.");

            reservation.CheckIn = reservationDto.CheckIn;
            reservation.CheckOut = reservationDto.CheckOut;


            reservation.IsActive = reservation.CheckOut > DateTime.UtcNow;

            await _reservationRepository.Update(reservation);

        }

        public async Task<List<ReservationForGettingDto>> GetFilteredReservationsAsync(int? hotelId, string? personalNumber, int? roomId)
        {
            var reservations = await _reservationRepository.GetAllAsync(
                r =>
                    (!hotelId.HasValue || r.Room.HotelId == hotelId) &&
                    (string.IsNullOrEmpty(personalNumber) || r.GuestReservations.Any(gr => gr.Guest.PersonalNumber == personalNumber)) &&
                    (!roomId.HasValue || r.RoomId == roomId),
                includeProperties: "Room.Hotel,GuestReservations.Guest"
            );

            return _mapper.Map<List<ReservationForGettingDto>>(reservations);
        }
    }
}
