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
    public class GuestRepository : RepositoryBase<Guest>, IGuestRepository
    {
        private readonly HotelsDbContext _context;
        public GuestRepository(HotelsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Save()=>await _context.SaveChangesAsync();

        public async Task Update(Guest entity)
        {
            if (await _context.GuestReservations
                .AnyAsync(gr => gr.GuestId == entity.Id))
            {
                throw new InvalidOperationException("Guest cannot be updated because they have an active reservation.");
            }

            var guestFromDb = await _context.Guests
                .FirstOrDefaultAsync(g => g.Id == entity.Id);

            if (guestFromDb != null)
            {

                guestFromDb.FirstName = entity.FirstName;
                guestFromDb.LastName = entity.LastName;
                guestFromDb.PersonalNumber = entity.PersonalNumber;
                guestFromDb.PhoneNumber = entity.PhoneNumber;

            }
        }
    }
}
