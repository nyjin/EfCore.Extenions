using EfCore.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}