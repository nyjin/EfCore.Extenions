using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace EfCore.Extensions
{
    public static class RepositoryExtensions
    {
        public static Task<List<TEntity>> GetAllAsync<TEntity>(this IRepository<TEntity> repository, Action<ISpecificationBuilder<TEntity>> specBuilder) where TEntity : class
        {
            var spec = new RelaySpecification<TEntity>();
            specBuilder(spec.GetQuery());

            return repository.GetAllAsync(spec);
        }

        public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IRepository<TEntity> repository, Action<ISpecificationBuilder<TEntity>> specBuilder) where TEntity : class
        {
            var spec = new RelaySpecification<TEntity>();
            specBuilder(spec.GetQuery());

            return repository.FirstOrDefaultAsync(spec);
        }

        public static Task<bool> AnyAsync<TEntity>(this IRepository<TEntity> repository, Action<ISpecificationBuilder<TEntity>> specBuilder) where TEntity : class
        {
            var spec = new RelaySpecification<TEntity>();
            specBuilder(spec.GetQuery());

            return repository.AnyAsync(spec);
        }

        public static void UpdateIfChanged<TEntity>(this IRepository<TEntity> repository, TEntity entity)
            where TEntity : class
        {
            if(repository.IsUpdated(entity))
            {
                repository.Update(entity);
            }
        }

        public static void UpdateIfChanged<TEntity>(this IRepository<TEntity> repository, params TEntity[] entities)
            where TEntity : class
        {
            foreach(var entity in entities.Where(repository.IsUpdated))
            {
                repository.Update(entity);
            }
        }
    }
}