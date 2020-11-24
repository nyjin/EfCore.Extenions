using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EfCore.Extensions
{
    public static class RepositoryCollectionExtensions
    {
        public static IServiceCollection UseRepository(this IServiceCollection serviceCollection)
        {
            if(serviceCollection is null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.TryAddScoped(typeof(IRepository<>), typeof(Repository<>));
            return serviceCollection;
        }
    }
}