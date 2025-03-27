using Hotels.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Interfaces
{
    public interface IGuestReservationRepository
    {
        Task<List<GuestReservation>> GetByReservationIdAsync(int reservationId);
        Task<List<GuestReservation>> GetByGuestIdAsync(int guestId);
        Task<List<GuestReservation>> GetAllAsync();
        Task AddAsync(GuestReservation guestReservation);
        
        Task RemoveAsync(GuestReservation guestToRemove);
        Task SaveAsync();
    }
}
