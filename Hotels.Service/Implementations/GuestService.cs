using AutoMapper;
using Hotels.Models.Dto.Guest;
using Hotels.Models.Entities;
using Hotels.Repository.Interfaces;
using Hotels.Service.Exceptions;
using Hotels.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Implementations
{
    public class GuestService : IGuestService
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IReservationRepository _reservationRepository;
        private readonly ISavable _savable;
        private readonly IMapper _mapper;

        public GuestService(IGuestRepository guestRepository, IReservationRepository reservationRepository, ISavable savable, IMapper mapper)
        {
            _guestRepository = guestRepository;
            _reservationRepository = reservationRepository;
            _savable = savable;
            _mapper = mapper;
        }
        public async Task AddGuestAsync(GuestForCreateDto guestDto)
        {
            var guests = await _guestRepository
                .GetAllAsync(g => g.PersonalNumber == guestDto.PersonalNumber || g.PhoneNumber == guestDto.PhoneNumber);

            if (guests.Any())
                throw new InvalidOperationException("Guest with the same personal number or phone number already exists.");

            var guest = _mapper.Map<Guest>(guestDto);
            await _guestRepository.AddAsync(guest);
            
        }

        public async Task DeleteGuestAsync(int guestId)
        {
            var guest = await _guestRepository.GetAsync(g => g.Id == guestId, includeProperties: "GuestReservations.Reservation");
            if (guest == null)
            {
                throw new NotFoundException("Guest not found.");
            }

            
            var activeReservations = guest.GuestReservations
                .Any(gr => gr.Reservation.CheckOut >= DateTime.UtcNow);

            if (activeReservations)
            {
                throw new InvalidOperationException("Cannot delete guest with active reservations.");
            }

           
            _guestRepository.Remove(guest);

        }

        public async Task<List<GuestForGettingDto>> GetAllGuestsAsync()
        {
            var guests = await _guestRepository.GetAllAsync();
            return _mapper.Map<List<GuestForGettingDto>>(guests);
        }

        public async Task<GuestForGettingDto> GetGuestAsync(int guestId)
        {
            var guest = await _guestRepository.GetAsync(g => g.Id == guestId);
            if (guest == null)
                throw new InvalidOperationException("Guest not found.");
            return _mapper.Map<GuestForGettingDto>(guest);
        }

        public async Task UpdateGuestAsync(GuestForUpdateDto guestDto)
        {
            var guest = await _guestRepository.GetAsync(g => g.Id == guestDto.Id);
            if (guest == null)
                throw new InvalidOperationException("Guest not found.");

            var existingGuests = await _guestRepository.GetAllAsync(g =>
                (g.PersonalNumber == guestDto.PersonalNumber || g.PhoneNumber == guestDto.PhoneNumber) && g.Id != guestDto.Id);

            if (existingGuests.Any())
                throw new InvalidOperationException("Another guest with the same personal number or phone number already exists.");

            
            _mapper.Map(guestDto, guest);
        }
       
        public async Task SaveGuest()=>await _guestRepository.Save();
        
    }
}
