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
        [Fact]
        public void GetService_RepositoryIsNotNull()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.TryAdd(new ServiceDescriptor(typeof(DbContextOptions<TodoDbContext>), typeof(DbContextOptions<TodoDbContext>), ServiceLifetime.Scoped));
            serviceCollection.TryAdd(new ServiceDescriptor(typeof(TodoDbContext), typeof(TodoDbContext), ServiceLifetime.Scoped));
            serviceCollection.UseRepository<TodoDbContext>();
            var provider = serviceCollection.BuildServiceProvider();

            var repo = provider.GetService<IRepository<TodoItem>>();
            repo.Should().NotBeNull();  
        }
    }
}
