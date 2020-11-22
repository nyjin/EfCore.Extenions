using Microsoft.EntityFrameworkCore;
using nyjin.EfCore.Models;

namespace nyjin.EfCore.Extensions.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get;set; }
    }
}