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
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        private readonly HotelsDbContext _context;
        public HotelRepository(HotelsDbContext context) : base(context)
        {
            _context = context; 
        }

        public async Task Save() => await _context.SaveChangesAsync();

        public async Task Update(Hotel entity)
        {
            var entityFromDb = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == entity.Id);

            if (entityFromDb != null)
            {
                entityFromDb.Name = entity.Name;
                entityFromDb.Address = entity.Address;
                entityFromDb.Rating = entity.Rating;


                _context.Hotels.Update(entityFromDb);
            }
            else
            {
                throw new KeyNotFoundException("Hotel not found");
            }
        }
    }
}
