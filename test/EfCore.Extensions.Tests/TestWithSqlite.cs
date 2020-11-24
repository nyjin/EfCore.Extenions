using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions.Tests
{
    public abstract class TestWithSqlite : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        protected TestWithSqlite()
        {
            _connection.Open();
            var options = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(_connection).Options;
            DbContext = new TestDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public TestDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }
}