using System;
using EfCore.Extensions.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace EfCore.Extensions.Tests
{
    public abstract class TestWithSqlite : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        private ITestOutputHelper _output;

        public TestWithSqlite(ITestOutputHelper outputHelper)
        {
            _output = outputHelper;
            _connection.Open();
            var options = new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options;

            var loggerFactory = new LoggerFactory();
            var accessor = new TestOutputHelperAccessor();
            accessor.Output = _output;
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));

            DbContext = new TodoDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public TodoDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }
}