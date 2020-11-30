using System;
using AutoMapper;
using EfCore.Extensions.Data;
using EfCore.Extensions.WebApi.Profiles;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions.WebApi.Tests
{
    public abstract class TestWithSqlite : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        protected TestWithSqlite()
        {
            _connection.Open();
            var options = new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options;
            DbContext = new TodoDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public TodoDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }

    public class TestRepository : TestWithSqlite
    {
        public IMapper Mapper { get; }

        public TestRepository()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TodoProfile());
            });
            Mapper = mappingConfig.CreateMapper();
        }

        public IRepository<T> CreateRepository<T>() where T : class => new Repository<T>(new RepositoryOptions<TodoDbContext>(DbContext));
    }
}