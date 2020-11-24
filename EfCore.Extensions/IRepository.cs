using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfCore.Extensions
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        DbContext Context { get; }

        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec = null);

        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec = null);

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