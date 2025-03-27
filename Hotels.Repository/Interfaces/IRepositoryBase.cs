using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Interfaces
{
    public interface IRepositoryBase<T>where T : class
    {
        Task AddAsync(T entity);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        Task<T> GetAsync(Expression<Func<T, bool>> filter, string includeProperties = null);

        void Remove(T entity);

        //დავამატე
        IQueryable<T> GetQueryable(string includeProperties = null);
    }
}
