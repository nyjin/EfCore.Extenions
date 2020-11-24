using Microsoft.EntityFrameworkCore;
using EfCore.Models;

namespace EfCore.Extensions.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get;set; }
        public DbSet<User> Users { get; set; }
    }
}