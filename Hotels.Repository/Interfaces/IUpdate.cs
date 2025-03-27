using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Interfaces
{
    public interface IUpdate<T>where T : class
    {
        Task Update(T entity);
    }
}
