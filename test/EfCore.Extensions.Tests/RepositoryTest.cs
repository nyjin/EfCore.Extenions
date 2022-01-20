using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentAssertions;
using Xunit;
using EfCore.Extensions.Models;

namespace EfCore.Extensions.Tests
{
    public class RepositoryTest : IClassFixture<SqliteFixture>
    {
        private readonly SqliteFixture _fixture;

        public RepositoryTest(SqliteFixture fixture) => _fixture = fixture;

        private IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
            => _fixture.CreateRepository<TEntity>();

        [Fact]
        public async Task GetAllAsync_IsNotEmptyAsync()
        {
            var repository = CreateRepository<TodoItem>();
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var _ = await repository.AddAsync(item);
            var changes = await repository.SaveAsync();
            var result = await repository.GetAllAsync();

            changes.Should().BeGreaterThan(0);
            result.Count.Should().BeGreaterThan(0);
            result.FirstOrDefault().GetType().Should().Be(typeof(TodoItem));
        }

        [Fact]
        public async Task GetAllAsync_AsyncSpecBuilder_ShouldGetResult()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = await repo.GetAllAsync(async e =>
            {
                var todo = await repo.FirstOrDefaultAsync(e => e.Include(y => y.UserTodos));
                e.Where(y => todo.Id == y.Id);
            });

            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_OnlyOneResultAsync()
        {
            var repository = CreateRepository<TodoItem>();
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
        public async Task FirstOrDefaultAsync_AsyncSpecBuilder_ShouldGetResult()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = await repo.FirstOrDefaultAsync(async e =>
            {
                var todo = await repo.FirstOrDefaultAsync(e => e.Include(y => y.UserTodos));
                e.Where(y => todo.Id == y.Id);
            });

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllAsync_WithWhereClauseAsync()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = await repo.GetAllAsync(x => x.Where(y => y.Name == items.First().Name));

            result.Count.Should().Be(1);
        }

        [Fact]
        public async Task GetAll_WithWhereClause()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = repo.GetAll(x => x.Where(y => y.Name == items.First().Name));

            result.Count.Should().Be(1);
        }

        [Fact]
        public void GetRepository_HasSameContext()
        {
            var repo = CreateRepository<TodoItem>();
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
            var result = await repo.FirstOrDefaultAsync(x => x.Where(y => y.Name == item.Name));
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Remove_Entity_ShouldBe_ChangedAsync()
        {
            var (repo, item) = await AddTestItemAsync(Guid.NewGuid().ToString());
            repo.Remove(item);

            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            var result = await repo.FirstOrDefaultAsync(x => x.Where(y => y.Name == item.Name));
            result.Should().BeNull();
        }

        [Fact]
        public async Task IsUpdated_UpdatedEntity_ReturnTrueAsync()
        {
            var (repo, item) = await AddTestItemAsync();
            item.Name = "A";
            repo.IsUpdated(item).Should().BeTrue();
        }

        [Fact]
        public async Task AnyAsync_HasEntity_ReturnTrue()
        {
            var (repo, item) = await AddTestItemAsync();

            var result = await repo.AnyAsync(x => x.Where(y => y.Name == item.Name));
            result.Should().BeTrue();
        }

        [Fact]
        public async Task AnyAsync_NoEntity_ReturnFalse()
        {
            var (repo, item) = await AddTestItemAsync();
            var name = Guid.NewGuid().ToString();

            var result = await repo.AnyAsync(x => x.Where(y => y.Name == name));
            result.Should().BeFalse();
        }

        [Fact]
        public async Task AnyAsync_AsyncSpecBuilder_ShouldGetResult()
        {
            var (repo, items) = await AddTestItemsAsync();
            var result = await repo.AnyAsync(async e =>
            {
                var todo = await repo.FirstOrDefaultAsync(e => e.Include(y => y.UserTodos));
                e.Where(y => todo.Id == y.Id);
            });

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Any_HasEntity_ReturnTrue()
        {
            var (repo, item) = await AddTestItemAsync();

            var result = repo.Any(x => x.Where(y => y.Name == item.Name));
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Any_NoEntity_ReturnFalse()
        {
            var (repo, item) = await AddTestItemAsync("A");

            var result = repo.Any(x => x.Where(y => y.Name == "B"));
            result.Should().BeFalse();
        }

        [Fact]
        public async Task FirstOrDefault_HasEntity_ValidEntity()
        {
            var (repo, item) = await AddTestItemAsync();

            var result = repo.FirstOrDefault(x => x.Where(y => y.Name == item.Name));
            result.Should().NotBeNull();
            result.Should().BeOfType<TodoItem>();
        }

        [Fact]
        public async Task FirstOrDefault_NoEntity_Null()
        {
            var (repo, item) = await AddTestItemAsync();

            var result = repo.FirstOrDefault(x => x.Where(y => y.Name == "A"));
            result.Should().BeNull();
        }

        [Fact]
        public async Task Remove_RemoveAll_ShouldEmpty()
        {
            var (repo, items) = await AddTestItemsAsync();
            repo.Remove(items);
            var changes = await repo.SaveAsync();
            changes.Should().Be(items.Count());
            var result = await repo.GetAllAsync();
            var removed = items.Select(x => x.Name);
            var resultByFilter = result.Count(x => removed.Contains(x.Name));
            resultByFilter.Should().Be(0);        }

        private static IEnumerable<TodoItem> CreateTestTodoItems(User user)
        {
            var item = new TodoItem
            {
                Name = Guid.NewGuid().ToString(),
            };

            var item2 = new TodoItem
            {
                Name = Guid.NewGuid().ToString()
            };

            item.UserTodos.Add(new UserTodo
            {
                Todo = item,
                User = user
            });

            item2.UserTodos.Add(new UserTodo
            {
                Todo = item2,
                User = user
            });

            return new List<TodoItem>
            {
                item,item2
            };
        }

        private async Task<(IRepository<TodoItem>, IEnumerable<TodoItem>)> AddTestItemsAsync()
        {
            var user = new User
            {
                Name = "User1"
            };
            var items = CreateTestTodoItems(user);
            var repo = CreateRepository<TodoItem>();
            await repo.AddAsync(items);
            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            return (repo, items);
        }

        private async Task<(IRepository<TodoItem>, TodoItem)> AddTestItemAsync(string name = "Hello")
        {
            var item = new TodoItem
            {
                Name = name
            };

            var repo = CreateRepository<TodoItem>();
            await repo.AddAsync(item);
            var changes = repo.Save();
            changes.Should().BeGreaterThan(0);
            return (repo, item);
        }
    }
}