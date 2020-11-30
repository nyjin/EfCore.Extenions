using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EfCore.Extensions.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }

        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoItemSettings> TodoItemSettings { get; set; }
        public DbSet<User> Users { get; set; }

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