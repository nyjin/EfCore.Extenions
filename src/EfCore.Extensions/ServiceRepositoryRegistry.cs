using System;

namespace EfCore.Extensions;

public class ServiceRepositoryRegistry : IRepositoryRegistry
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceRepositoryRegistry(IServiceProvider serviceProvider) => _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public IRepository<TEntity> GetRepository<TEntity>(RepositoryOptions repositoryOptions) where TEntity : class
        => (IRepository<TEntity>)_serviceProvider.GetService(typeof(IRepository<TEntity>));
}