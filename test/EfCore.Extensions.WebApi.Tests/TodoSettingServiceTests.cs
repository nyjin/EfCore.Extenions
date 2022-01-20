using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using AutoMapper;
using EfCore.Extensions.Models;
using EfCore.Extensions.Tests;
using EfCore.Extensions.WebApi.Profiles;
using EfCore.Extensions.WebApi.Services;
using FluentAssertions;
using Xunit;

namespace EfCore.Extensions.WebApi.Tests
{
    public class TodoSettingServiceTests : IClassFixture<SqliteFixture>
    {
        private readonly SqliteFixture _fixture;

        public TodoSettingServiceTests(SqliteFixture fixture)
        {
            _fixture = fixture;

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoProfile());
            });
            Mapper = mappingConfig.CreateMapper();
        }

        public IMapper Mapper { get; }

        [Fact]
        public async Task AddOrUpdateSettingsAsync_AddSettingValue_ShouldBeSave()
        {
            var repository = _fixture.CreateRepository<TodoItem>();

            var items = await Setup(repository, new [] { "a" }, new[] { "a" });
            var settingRepository = repository.GetRepository<TodoItemSettings>();

            var service = new TodoService(Mapper, settingRepository);
            var todoId = items.ElementAt(0).TodoId;

            var changes = await service.AddOrUpdatePropsAsync(todoId,
                new TodoItemSettingDto
                {
                    Name = "Name",
                    IsCompleted = true,
                    Description = "Description",
                    ExpireDate = DateTime.MinValue
                });

            changes.Should().BeGreaterThan(0);

        }

        [Fact]
        public async Task FirstOrDefault_FilterSpecificUser_ShouldGetTodo()
        {
            var repository = _fixture.CreateRepository<TodoItem>();

            var todoNames = new[] { "a", "b" };
            var userNames = new [] { "a", "b"};

            var userTodos = await Setup(repository, todoNames, userNames);

            var result = await repository.FirstOrDefaultAsync(x
                => x.Include(e => e.UserTodos.Where(r => r.User.Name.Equals(userNames[0]))));

            result.Name.Equals(todoNames[0]);
        }

        private async Task<IEnumerable<UserTodo>> Setup(IRepository<TodoItem> repository, string[] todoNames, string[] userNames)
        {
            var userTodos = todoNames.Select((x, i) => new UserTodo
            {
                Todo = new TodoItem { Name = x },
                User = new User { Name = userNames[i] }
            });

            var userTodoRepository = repository.GetRepository<UserTodo>();
            userTodoRepository.Add(userTodos);

            await userTodoRepository.SaveAsync();
            return await userTodoRepository.GetAllAsync(x => x.Include(e => e.User).Include(e => e.Todo).AsNoTrackingWithIdentityResolution());
        }
    }
}
