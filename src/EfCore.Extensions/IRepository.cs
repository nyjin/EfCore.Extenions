using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Specification;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IRepository<TAnotherEntity> GetRepository<TAnotherEntity>() where TAnotherEntity : class;

        DbContext Context { get; }

        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec = null);

        Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec = null);

        Task<bool> AnyAsync(ISpecification<TEntity> spec = null);

        void Add(TEntity item);

        Task<TEntity> AddAsync(TEntity item);

        Task AddAsync(params TEntity[] items);

        void Attach(TEntity entity);

        void Attach(params TEntity[] entities);

        void Detach(TEntity entity);

        void Update(TEntity item);

        void Update(params TEntity[] items);

        void Remove(TEntity item);

        void Remove(params TEntity[] items);

        Task<int> SaveAsync();

        int Save();

        bool IsUpdated(TEntity entity);
    }
}