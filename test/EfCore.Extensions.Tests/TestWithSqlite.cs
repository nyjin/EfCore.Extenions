using System;
using EfCore.Extensions.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions.Tests
{
    public abstract class TestWithSqlite : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        public TestWithSqlite()
        {
            _connection.Open();
            var options = new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options;
            DbContext = new TodoDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public TodoDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }
}