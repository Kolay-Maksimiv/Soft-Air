using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SoftAir.Services.Abstract
{
    public interface IGeneralService<T>
    {
        T Find(object id);
        Task<T> FindAsync(object id);

        void Insert(T entity);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);

        ICollection<T> GetAll();

        ICollection<T> GetAll(Expression<Func<T, bool>> expression);

        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> expression);

        bool Any(Expression<Func<T, bool>> expression);

        T FirstOrDefault(Expression<Func<T, bool>> expression);

        T LastOrDefault(Expression<Func<T, bool>> expression);

        void TrackedSaveChanges();
    }
}
