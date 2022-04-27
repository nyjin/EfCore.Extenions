using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;

namespace EfCore.Extensions;

public static class RepositoryExtensions
{
    public static List<TEntity> GetAll<TEntity>(this IRepository<TEntity> repository,
                                                Action<ISpecificationBuilder<TEntity>> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.GetAll(spec);
    }

    public static Task<List<TEntity>> GetAllAsync<TEntity>(this IRepository<TEntity> repository,
                                                           Action<ISpecificationBuilder<TEntity>> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.GetAllAsync(spec);
    }

    public static async ValueTask<List<TEntity>> GetAllAsync<TEntity>(
        this IRepository<TEntity> repository, Func<ISpecificationBuilder<TEntity>, ValueTask> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        await specBuilder(spec.GetQuery()).ConfigureAwait(false);

        return await repository.GetAllAsync(spec).ConfigureAwait(false);
    }

    public static Task<TEntity> FirstOrDefaultAsync<TEntity>(this IRepository<TEntity> repository,
                                                             Action<ISpecificationBuilder<TEntity>> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.FirstOrDefaultAsync(spec);
    }

    public static async ValueTask<TEntity> FirstOrDefaultAsync<TEntity>(
        this IRepository<TEntity> repository, Func<ISpecificationBuilder<TEntity>, ValueTask> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        await specBuilder(spec.GetQuery()).ConfigureAwait(false);

        return await repository.FirstOrDefaultAsync(spec).ConfigureAwait(false);
    }

    public static TEntity FirstOrDefault<TEntity>(this IRepository<TEntity> repository,
                                                  Action<ISpecificationBuilder<TEntity>> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.FirstOrDefault(spec);
    }

    public static Task<bool> AnyAsync<TEntity>(this IRepository<TEntity> repository,
                                                     Action<ISpecificationBuilder<TEntity>> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.AnyAsync(spec);
    }

    public static async ValueTask<bool> AnyAsync<TEntity>(this IRepository<TEntity> repository,
                                                     Func<ISpecificationBuilder<TEntity>, ValueTask> specBuilder)
        where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        await specBuilder(spec.GetQuery()).ConfigureAwait(false);

        return await repository.AnyAsync(spec).ConfigureAwait(false);
    }

    public static bool Any<TEntity>(this IRepository<TEntity> repository,
                                    Action<ISpecificationBuilder<TEntity>> specBuilder) where TEntity : class
    {
        var spec = new RelaySpecification<TEntity>();
        specBuilder(spec.GetQuery());

        return repository.Any(spec);
    }

    public static bool UpdateIfChanged<TEntity>(this IRepository<TEntity> repository, TEntity entity)
        where TEntity : class
    {
        if(!repository.IsUpdated(entity)) { return false; }

        repository.Update(entity);
        return true;

    }

    public static int UpdateIfChanged<TEntity>(this IRepository<TEntity> repository, params TEntity[] entities)
        where TEntity : class
    {
        var affect = 0;
        foreach(var entity in entities.Where(repository.IsUpdated))
        {
            repository.Update(entity);
            affect++;
        }

        return affect;
    }
}