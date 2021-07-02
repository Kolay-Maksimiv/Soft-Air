using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoftAir.Data.Repositories.Abstract
{
    public interface IGenericRepository<TEntity> : IDisposable where TEntity : class
    {
        Task SaveChangesAsync();
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> FindAsync(object id);
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        void AddRange(IList<TEntity> entities);
        Task AddRangeAsync(IList<TEntity> entities);
        void Update(TEntity entity);
        TEntity Delete(TEntity entity);
        Task<TEntity> DeleteByIdAsync(object id);
    }
}
