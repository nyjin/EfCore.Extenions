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
using Xunit.Abstractions;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace EfCore.Extensions.Tests.Core
{
    public class SqliteFixture : IDisposable
    {
        private const string ConnectionString = "DataSource=:memory:";
        private readonly SqliteConnection _connection = new SqliteConnection(ConnectionString);

        public ITestOutputHelper Output { get; }

        public SqliteFixture(ITestOutputHelper outputHelper)
        {
            Output = outputHelper;
            _connection.Open();
            var options = new DbContextOptionsBuilder<TodoDbContext>().UseSqlite(_connection).Options;

            var loggerFactory = new LoggerFactory();
            var accessor = new TestOutputHelperAccessor();
            accessor.Output = Output;
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor));

            DbContext = new TodoDbContext(options);
            DbContext.Database.EnsureCreated();
        }

        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class =>
            new Repository<TEntity>(new RepositoryOptions<TodoDbContext>(DbContext));

        public TodoDbContext DbContext { get; }

        public void Dispose() => DbContext?.Dispose();
    }
}