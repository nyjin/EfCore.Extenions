using System;
using System.Collections.Generic;
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
    }
}