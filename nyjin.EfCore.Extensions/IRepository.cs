using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nyjin.EfCore.Extensions
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        DbContext Context { get; }

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null);

        EntityEntry<TEntity> Add(TEntity item);

        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity item);

        Task AddAsync(params TEntity[] items);

        EntityEntry<TEntity> Update(TEntity item);

        void Update(params TEntity[] items);

        EntityEntry<TEntity> Remove(TEntity item);

        void Remove(params TEntity[] items);

        Task<int> SaveAsync();

        int Save();

        IRepository<TAnotherEntity> GetRepository<TAnotherEntity>() where TAnotherEntity : class;
    }
}