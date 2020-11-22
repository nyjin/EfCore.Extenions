using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nyjin.EfCore.Extensions
{
    public class Repository : IRepository
    {
        public Repository(DbContext context)
            => this.Context = context ?? throw new ArgumentNullException(nameof(context));

        public DbContext Context { get;private set; } = default!;

        public void Dispose()
        {
            this.Context?.Dispose();
            this.Context = null;
        }

        public Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
            => Context.Set<TEntity>().AsQueryable().ToListAsync();

        public EntityEntry<TEntity> Add<TEntity>(TEntity item) where TEntity : class => this.Context.Add(item);

        public ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity item) where TEntity : class => this.Context.AddAsync(item);

        public Task<int> SaveAsync() => Context.SaveChangesAsync();
    }
}