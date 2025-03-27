using Hotels.Models.Dto.Guest;
using Hotels.Models.Dto.GuestReservation;
using Hotels.Models.Dto.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IGuestReservationService
    {
        Task AddGuestToReservationAsync(GuestReservationForGettingDto guestReservationDto);
        Task<List<GuestReservationForGettingDto>> GetAllGuestReservationsAsync();

       
      

        Task SaveGuestReservation();
    }
}
