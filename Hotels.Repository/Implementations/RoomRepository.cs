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
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        private readonly HotelsDbContext _context;
        public RoomRepository(HotelsDbContext context) : base(context)
        {
            _context=context;
        }

        public async Task Save() => await _context.SaveChangesAsync();

        public async Task Update(Room entity)
        {
            var roomFromDb = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == entity.Id);
            if (roomFromDb != null)
            {
                roomFromDb.Name = entity.Name;
                roomFromDb.IsAvailable = entity.IsAvailable;
                roomFromDb.Price = entity.Price;

            }
        }
    }
}
