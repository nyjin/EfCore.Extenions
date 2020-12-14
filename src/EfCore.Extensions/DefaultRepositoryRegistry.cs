using System;
using System.Collections.Concurrent;

namespace EfCore.Extensions
{
    public class DefaultRepositoryRegistry : IRepositoryRegistry
    {
        private readonly ConcurrentDictionary<Type, object> _cache = new();

        public IRepository<TEntity> GetRepository<TEntity>(RepositoryOptions options)
            where TEntity : class
        {
            return options is null
                ? throw new ArgumentNullException(nameof(options))
                : (IRepository<TEntity>)_cache.GetOrAdd(typeof(IRepository<TEntity>),
                    _ => new Repository<TEntity>(options));
        }
    }
}