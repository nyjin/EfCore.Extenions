// -----------------------------------------------------------------------
// <copyright file="SqliteFixture.cs" company="NCSOFT">
// Copyright (c) NCSOFT. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using System;
using EfCore.Extensions.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCore.Extensions.Tests
{
    public class SqliteFixture : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        public SqliteFixture()
        {
            var loggerFactory = new LoggerFactory();

            var options = new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options;
            DbContext = new TodoDbContext(options, loggerFactory);
            _connection.Open();
            DbContext.Database.EnsureCreated();
        }

        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class =>
            new Repository<TEntity>(new RepositoryOptions<TodoDbContext>(DbContext));

        public TodoDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }
}