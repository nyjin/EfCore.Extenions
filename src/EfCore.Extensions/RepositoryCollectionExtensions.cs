using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EfCore.Extensions
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection UseRepository<TContext>(this IServiceCollection serviceCollection)
            where TContext : DbContext
        {
            if(serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(RepositoryOptions), p => p.GetService<RepositoryOptions<TContext>>(), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(RepositoryOptions<TContext>), CreateRepositoryOptions<TContext>, ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(IRepository<>), typeof(Repository<>), ServiceLifetime.Scoped));
            return serviceCollection;
        }

        private static RepositoryOptions<TContext> CreateRepositoryOptions<TContext>(IServiceProvider provider)
            where TContext : DbContext
            => new RepositoryOptions<TContext>(provider.GetService<TContext>());
    }
}