using Hotels.Models.Dto.Reservation;
using Hotels.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Service.Interface
{
    public interface IReservationService
    {
        Task CreateReservationAsync(ReservationForCreateDto reservationDto);
        Task UpdateReservationAsync(ReservationForUpdateDto reservationDto);
        Task CancelReservationAsync(int reservationId);
        Task<List<ReservationForGettingDto>> GetReservationsAsync(ReservationFilterDto filter);
        Task<List<ReservationForGettingDto>> GetFilteredReservationsAsync(int? hotelId, string? PersonalNumber, int? roomId);
        Task SaveReservation();
    }
}
