using System;
using System.Threading.Tasks;
using EfCore.Extensions.Models;
using EfCore.Extensions.WebApi.Services;
using FluentAssertions;
using Xunit;

namespace EfCore.Extensions.WebApi.Tests
{
    public class TodoSettingServiceTests : TestRepository
    {
        [Fact]
        public async Task AddOrUpdateSettingsAsync_AddSettingValue_ShouldBeSave()
        {
            using var repository = CreateRepository<TodoItem>();
            var item = new TodoItem
            {
                Name = "Hello"
            };

            var added = await repository.AddAsync(item);
            repository.Save();
            repository.Detach(item);
            using var settingRepository = CreateRepository<TodoItemSettings>();

            var service = new TodoService(Mapper, settingRepository);

            var changes = await service.AddOrUpdatePropsAsync(item.Id,
                new TodoItemSettingDto
                {
                    Name = "Name",
                    IsCompleted = true,
                    Description = "Description",
                    ExpireDate = DateTime.MinValue
                });

            changes.Should().BeGreaterThan(0);

        }
    }
}
