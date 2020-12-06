using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EfCore.Extensions
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection AddRepository<TContext>(this IServiceCollection serviceCollection)
            where TContext : DbContext
        {
            if(serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(RepositoryOptions), p => p.GetService<RepositoryOptions<TContext>>(), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(RepositoryOptions<TContext>), typeof(RepositoryOptions<TContext>), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(IRepository<>), typeof(Repository<>), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(IRepositoryRegistry), typeof(ServiceRepositoryRegistry), ServiceLifetime.Scoped));
            return serviceCollection;
        }
    }
}