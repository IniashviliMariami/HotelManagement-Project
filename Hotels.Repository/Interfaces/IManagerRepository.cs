using Hotels.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Interfaces
{
    public interface IManagerRepository:IRepositoryBase<Manager>, IUpdate<Manager>, ISavable
    {

    }
}
