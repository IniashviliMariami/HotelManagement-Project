using Hotels.Models.Entities;
using Hotels.Repository.Data;
using Hotels.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Implementations
{
    public class GuestReservationRepository : IGuestReservationRepository
        
    {
        private readonly HotelsDbContext _context;

        public GuestReservationRepository(HotelsDbContext context)
        {
            _context = context;
        }

        public async Task<List<GuestReservation>> GetByReservationIdAsync(int reservationId)
        {
            return await _context.GuestReservations
                                 .Where(gr => gr.ReservationId == reservationId)
                                 .ToListAsync();
        }

        public async Task<List<GuestReservation>> GetByGuestIdAsync(int guestId)
        {
            return await _context.GuestReservations
                                 .Where(gr => gr.GuestId == guestId)
                                 .ToListAsync();
        }

        public async Task<List<GuestReservation>> GetAllAsync()
        {
            return await _context.GuestReservations.ToListAsync();
        }

        public async Task AddAsync(GuestReservation entity)
        {
            await _context.GuestReservations.AddAsync(entity);
        }


        public async Task RemoveAsync(GuestReservation entity)
        {
            _context.GuestReservations.Remove(entity);
        }

        public async Task SaveAsync() 
        {
            await _context.SaveChangesAsync();
        }
    }
    
}
