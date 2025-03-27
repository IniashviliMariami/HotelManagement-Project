using AutoMapper;
using Hotels.Models.Dto.Guest;
using Hotels.Models.Dto.GuestReservation;
using Hotels.Models.Dto.Reservation;
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
    public class GuestReservationService : IGuestReservationService
    {
        private readonly IGuestReservationRepository _guestReservationRepository;
        private readonly IMapper _mapper;

        public  GuestReservationService(IGuestReservationRepository guestReservationRepository, IMapper mapper)
        {
            _guestReservationRepository = guestReservationRepository;
            _mapper = mapper;
        }

        public async Task AddGuestToReservationAsync(GuestReservationForGettingDto guestReservationDto)
        {
            var guestReservation = _mapper.Map<GuestReservation>(guestReservationDto);

            await _guestReservationRepository.AddAsync(guestReservation);
           
        }
        public async Task<List<GuestReservationForGettingDto>> GetAllGuestReservationsAsync()
        {
            var guestReservations = await _guestReservationRepository.GetAllAsync();
            return _mapper.Map<List<GuestReservationForGettingDto>>(guestReservations);
        }

      

        public async Task SaveGuestReservation()
        {
           await _guestReservationRepository.SaveAsync();
        }
    }
}
