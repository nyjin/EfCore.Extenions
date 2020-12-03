using System;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions
{
    public class RepositoryOptions
    {
        public DbContext DbContext { get; internal set; }

        public IRepositoryRegistry RepositoryRegistry { get; internal set; }
    }

    public class RepositoryOptions<TContext> : RepositoryOptions where TContext : DbContext
    {
        public RepositoryOptions(TContext dbContext, IRepositoryRegistry repositoryRegistry = null)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            RepositoryRegistry = repositoryRegistry ?? new DefaultRepositoryRegistry();
        }
    }
}