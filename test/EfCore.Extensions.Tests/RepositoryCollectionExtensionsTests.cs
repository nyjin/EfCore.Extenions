using EfCore.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace EfCore.Extensions.Tests
{
    public class RepositoryCollectionExtensionsTests
    {
        [Fact]
        public void GetService_RepositoryIsNotNull()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TestDbContext), typeof(TestDbContext), ServiceLifetime.Scoped));
            serviceCollection.UseRepository();
            var provider = serviceCollection.BuildServiceProvider();
            var repo = provider.GetService<IRepository<TodoItem>>();
            repo.Should().NotBeNull();
        }
    }
}
