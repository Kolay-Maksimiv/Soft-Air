using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SoftAir.Data.Repositories.Abstract
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        protected DbSet<TEntity> DbSet { get; }
        protected ApplicationDbContext Context => _context;


        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            if (_context != null)
            {
                DbSet = Context.Set<TEntity>();
            }
        }
        protected GenericRepository(ApplicationDbContext context, DbSet<TEntity> dbSet)
        {
            _context = context;
            DbSet = dbSet;
        }

        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> FindAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public TEntity Add(TEntity entity)
        {
            EntityEntry<TEntity> value = DbSet.Add(entity);
            return value.Entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            EntityEntry<TEntity> value = await DbSet.AddAsync(entity);
            return value.Entity;
        }

        public void AddRange(IList<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IList<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public TEntity Delete(TEntity entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            EntityEntry<TEntity> value = DbSet.Remove(entity);

            return value.Entity;
        }

        public async Task<TEntity> DeleteByIdAsync(object id)
        {
            TEntity entityToDelete = await DbSet.FindAsync(id);

            return Delete(entityToDelete);
        }


        #region IDisposable

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion
    }
}
