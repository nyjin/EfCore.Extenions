using System;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions
{
    public class RepositoryRegistry : IRepositoryRegistry
    {
        private readonly ConcurrentDictionary<Type, object> _cache = new();

        public IRepository<TEntity> GetRepository<TEntity>(DbContext context) where TEntity : class
        {
            return context is null
                ? throw new ArgumentNullException(nameof(context))
                : (IRepository<TEntity>)_cache.GetOrAdd(typeof(IRepository<TEntity>),
                    _ => new Repository<TEntity>(context));
        }
    }
}