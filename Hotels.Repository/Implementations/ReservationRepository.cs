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
    public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
    {

        private readonly HotelsDbContext _context;
        public ReservationRepository(HotelsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Save() => await _context.SaveChangesAsync();

        public async Task Update(Reservation entity)
        {
            var reservationFromDb = await _context.Reservations
                    .FirstOrDefaultAsync(r => r.Id == entity.Id);

            if (reservationFromDb == null)
            {
                throw new InvalidOperationException("Reservation not found.");
            }

            
            var conflictingReservation = await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == reservationFromDb.RoomId && 
                    r.Id != reservationFromDb.Id &&         
                    (
                        (entity.CheckIn >= r.CheckIn && entity.CheckIn < r.CheckOut) ||
                        (entity.CheckOut > r.CheckIn && entity.CheckOut <= r.CheckOut) ||
                        (entity.CheckIn <= r.CheckIn && entity.CheckOut >= r.CheckOut) 
                    )
                );

            if (conflictingReservation)
            {
                throw new InvalidOperationException("New dates conflict with an existing reservation for this room.");
            }

            
            reservationFromDb.CheckIn = entity.CheckIn;
            reservationFromDb.CheckOut = entity.CheckOut;

            await _context.SaveChangesAsync();
        }
    }
}
