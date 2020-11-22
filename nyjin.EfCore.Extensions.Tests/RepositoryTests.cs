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
            var item = new TodoItem
            {
                Id = 1,
                Name = "Hello"
            };

            using var repository = new Repository(DbContext);
            var added = await repository.AddAsync(item);
            var changes = await repository.SaveAsync();
            changes.Should().BeGreaterThan(0);
            var result = await repository.GetAllAsync<TodoItem>();
            result.Count().Should().BeGreaterThan(0);
            result.FirstOrDefault().GetType().Should().Be(typeof(TodoItem));
        }
    }
}