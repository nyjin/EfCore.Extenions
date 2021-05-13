using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EfCore.Extensions.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EfCore.Extensions.Data
{
    public class TodoDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public TodoDbContext(DbContextOptions<TodoDbContext> options, ILoggerFactory loggerFactory = null) : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoItemSettings> TodoItemSettings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTodo> UserTodos { get;set; }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(_loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory).EnableSensitiveDataLogging();
            }
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if(Database.IsSqlite())
            {
                foreach(var entity in modelBuilder.Model.GetEntityTypes())
                {
                    if(entity.ClrType.IsSubclassOf(typeof(TrackableEntity)))
                    {
                        modelBuilder.Entity(entity.ClrType)
                            .Property<string>(nameof(TrackableEntity.CreatedBy))
                            .HasDefaultValue("SYSTEM");

                        modelBuilder.Entity(entity.ClrType)
                            .Property<string>(nameof(TrackableEntity.UpdatedBy))
                            .HasDefaultValue("SYSTEM");

                        modelBuilder.Entity(entity.ClrType)
                            .Property<DateTime>(nameof(TrackableEntity.CreatedAt))
                            .ValueGeneratedOnAdd()
                            .HasDefaultValueSql("datetime('now', 'localtime')");

                        modelBuilder.Entity(entity.ClrType)
                            .Property<DateTime>(nameof(TrackableEntity.UpdatedAt))
                            .ValueGeneratedOnAdd()
                            .HasDefaultValueSql("datetime('now', 'localtime')");
                    }
                }
            }

            modelBuilder.Entity<UserTodo>()
                .HasOne(x => x.Todo)
                .WithMany(x => x.UserTodos)
                .HasForeignKey(x => x.TodoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTodo>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserTodos)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItemSettings>()
                .HasOne(x => x.TodoItem)
                .WithMany(x => x.Settings)
                .HasForeignKey(x => x.TodoItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetUpdateAtWhenEntityChanged();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            SetUpdateAtWhenEntityChanged();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void SetUpdateAtWhenEntityChanged()
        {
            if(Database.IsSqlite())
            {
                var now = DateTime.Now;
                foreach(var entity in this.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Modified)
                    .Select(x => x.Entity)
                    .OfType<TrackableEntity>())
                {
                    entity.UpdatedAt = now;
                }
            }
        }
    }
}