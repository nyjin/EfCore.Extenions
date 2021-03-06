using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCore.Extensions.Data;
using FluentAssertions;
using Xunit;
using EfCore.Extensions.Models;

namespace EfCore.Extensions.Tests
{
    public class RepositoryEnum : TestWithSqlite
    {
        private IRepository<TodoItem> CreateRepository() => new Repository<TodoItem>(new RepositoryOptions<TodoDbContext>(DbContext));

        [Fact]
        public async Task GetAllAsync_IsNotEmptyAsync()
        {
            using var repository = CreateRepository();
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var added = await repository.AddAsync(item);
            var changes = await repository.SaveAsync();
            var result = await repository.GetAllAsync();

            changes.Should().BeGreaterThan(0);
            result.Count.Should().BeGreaterThan(0);
            result.FirstOrDefault().GetType().Should().Be(typeof(TodoItem));
        }

        [Fact]
        public async Task FirstOrDefaultAsync_OnlyOneResultAsync()
        {
            using var repository = CreateRepository();
            var item = new TodoItem
            {
                Name = "Hello"
            };
            var item2 = new TodoItem
            {
                Name = "World"
            };

            await repository.AddAsync(item, item2);
            var changes = await repository.SaveAsync();
            var result = await repository.FirstOrDefaultAsync();

            changes.Should().BeGreaterThan(0);
            result.GetType().Should().Be(typeof(TodoItem));
        }

        [Fact]
        public async Task GetAllAsync_WithWhereClauseAsync()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = await repo.GetAllAsync(x => x.Where(y => y.Name == items.First().Name));

            result.Count.Should().Be(1);
        }

        [Fact]
        public void GetRepository_HasSameContext()
        {
            var repo = CreateRepository();
            var repo2 = repo.GetRepository<User>();
            repo.Context.Should().BeSameAs(repo2.Context);
        }

        [Fact]
        public async Task Update_Entity_ShouldBe_ChangedAsync()
        {
            var (repo, item) = await AddTestItemAsync();
            item.Name = "Hello2";
            repo.Update(item);

            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            var result = await repo.FirstOrDefaultAsync(x => x.Where(y => y.Name == "Hello2"));
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Remove_Entity_ShouldBe_ChangedAsync()
        {
            var (repo, item) = await AddTestItemAsync();
            repo.Remove(item);

            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            var result = await repo.FirstOrDefaultAsync(x => x.Where(y => y.Name == "Hello2"));
            result.Should().BeNull();
        }

        [Fact]
        public async Task IsUpdated_UpdatedEntity_ReturnTrueAsync()
        {
            var (repo, item) = await AddTestItemAsync();
            item.Name = "A";
            repo.IsUpdated(item).Should().BeTrue();
        }

        private async Task<(IRepository<TodoItem>, IEnumerable<TodoItem>)> AddTestItemsAsync()
        {
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var item2 = new TodoItem
            {
                Name = "World"
            };

            var repo = CreateRepository();
            await repo.AddAsync(item, item2);
            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            return (repo, new [] { item, item2});
        }

        private async Task<(IRepository<TodoItem>, TodoItem)> AddTestItemAsync()
        {
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var repo = CreateRepository();
            await repo.AddAsync(item);
            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            return (repo, item);
        }
    }
}