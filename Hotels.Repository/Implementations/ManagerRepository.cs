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
    public class ManagerRepository : RepositoryBase<Manager>, IManagerRepository
    {
        readonly HotelsDbContext _context;
        public ManagerRepository(HotelsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task Save() => await _context.SaveChangesAsync();

        public async Task Update(Manager entity)
        {
            var managerFromDb = await _context.Managers.FirstOrDefaultAsync(m => m.Id == entity.Id);

            if (managerFromDb != null)
            {

                managerFromDb.FirstName = entity.FirstName;
                managerFromDb.LastName = entity.LastName;
                managerFromDb.Email = entity.Email;
                managerFromDb.PhoneNumber = entity.PhoneNumber;

            }
        }
    }
}
