using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using nyjin.EfCore.Models;
using Xunit;

namespace nyjin.EfCore.Extensions.Tests
{
    public class RepositoryEnum : TestWithSqlite
    {
        [Fact]
        public async Task GetAllAsync_IsNotEmptyAsync()
        {
            using var repository = new Repository<TodoItem>(DbContext);
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
        public async Task GetAllAsync_FoundEntityByIdAsync()
        {
            using var repository = new Repository<TodoItem>(DbContext);
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
            var result = await repository.GetAllAsync(x => x.Id == 1);

            changes.Should().BeGreaterThan(0);
            result.Count.Should().Be(1);
            result.FirstOrDefault().GetType().Should().Be(typeof(TodoItem));
        }

        [Fact]
        public async Task FirstOrDefaultAsync_OnlyOneResultAsync()
        {
            using var repository = new Repository<TodoItem>(DbContext);
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
        public void GetRepository_HasSameContext()
        {
            var repo = new Repository<TodoItem>(DbContext);
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
            var result = await repo.FirstOrDefaultAsync(x => x.Name == "Hello2");
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Remove_Entity_ShouldBe_ChangedAsync()
        {
            var (repo, item) = await AddTestItemAsync();
            repo.Remove(item);

            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            var result = await repo.FirstOrDefaultAsync(x => x.Name == "Hello2");
            result.Should().BeNull();
        }

        private async Task<(Repository<TodoItem>, TodoItem)> AddTestItemAsync()
        {
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var repo = new Repository<TodoItem>(DbContext);
            await repo.AddAsync(item);
            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            return (repo, item);
        }
    }
}