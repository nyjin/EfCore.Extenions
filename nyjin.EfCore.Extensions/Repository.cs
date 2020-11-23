using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nyjin.EfCore.Extensions
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

        protected virtual IQueryable<TEntity> GetCompositeQuery(Expression<Func<TEntity, bool>> filter = null)
        {
            var q = GetQueryable();

            if(filter != null)
            {
                q = q.Where(filter);
            }

            return q;
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var q = GetCompositeQuery(filter);

            return q.ToListAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            var q = GetCompositeQuery(filter);

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