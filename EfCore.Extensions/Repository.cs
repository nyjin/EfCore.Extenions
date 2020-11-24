using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfCore.Extensions
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private bool _disposed;
        private readonly RepositoryRegistry _registry = new();

        public Repository(DbContext context)
            => this.Context = context ?? throw new ArgumentNullException(nameof(context));

        public DbContext Context { get; private set; } = default!;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposable)
        {
            if(_disposed)
            {
                return;
            }

            if(disposable)
            {
                this.Context?.Dispose();
                this.Context = null;
            }

            _disposed = true;
        }

        protected virtual IQueryable<TEntity> GetQueryable() => Context.Set<TEntity>().AsQueryable();

        protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec = null)
        {
            var q = GetQueryable();

            if(spec != null)
            {
                var evaluator = new SpecificationEvaluator<TEntity>();
                q = evaluator.GetQuery(this.GetQueryable(), spec);
            }

            return q;
        }

        public Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec = null)
        {
            var q = ApplySpecification(spec);

            return q.ToListAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec = null)
        {
            var q = ApplySpecification(spec);

            return q.FirstOrDefaultAsync();
        }

        public EntityEntry<TEntity> Add(TEntity item) => item == null ?
            throw new ArgumentNullException(nameof(item)) : this.Context.Add(item);

        public ValueTask<EntityEntry<TEntity>> AddAsync(TEntity item) => item == null ?
            throw new ArgumentNullException(nameof(item)) : this.Context.AddAsync(item);

        public Task AddAsync(params TEntity[] items) => items is null
            ? throw new ArgumentNullException(nameof(items))
            : Context.AddRangeAsync(items);

        public EntityEntry<TEntity> Update(TEntity item) => item is null ? throw new ArgumentNullException(nameof(item)) : Context.Update(item);

        public void Update(params TEntity[] items)
        {
            if(items == null) { throw new ArgumentNullException(nameof(items)); }

            Context.UpdateRange(items);
        }

        public EntityEntry<TEntity> Remove(TEntity item) => item is null ? throw new ArgumentNullException(nameof(item)) : Context.Remove(item);

        public void Remove(params TEntity[] items)
        {
            if(items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Context.RemoveRange(items);
        }

        public Task<int> SaveAsync() => Context.SaveChangesAsync();

        public int Save() => Context.SaveChanges();

        /// <inheritdoc />
        public IRepository<TAnotherEntity> GetRepository<TAnotherEntity>() where TAnotherEntity : class => _registry.GetRepository<TAnotherEntity>(Context);
    }
}