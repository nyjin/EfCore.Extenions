using EfCore.Extensions.Data;
using EfCore.Extensions.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace EfCore.Extensions.Tests
{
    public class RepositoryCollectionExtensionsTests
    {
        private readonly ServiceProvider _provider;

        public RepositoryCollectionExtensionsTests() => _provider = GetProvider();

        private static ServiceProvider GetProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(DbContextOptions<TodoDbContext>),
                typeof(DbContextOptions<TodoDbContext>), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(
                new ServiceDescriptor(typeof(TodoDbContext), typeof(TodoDbContext), ServiceLifetime.Scoped));
            serviceCollection.AddRepository<TodoDbContext>();
            var provider = serviceCollection.BuildServiceProvider();
            return provider;
        }

        [Fact]
        public void GetService_RepositoryIsNotNull()
        {
            var repo = _provider.GetService<IRepository<TodoItem>>();
            repo.Should().NotBeNull();  
        }

        [Fact]
        public void GetRepository_NestedGetRepository_SameDbContext()
        {
            var repo1 = _provider.GetService<IRepository<TodoItem>>();
            var repo2 = repo1.GetRepository<IRepository<User>>();

            repo1.Context.Should().BeEquivalentTo(repo2.Context);
        }
    }
}
