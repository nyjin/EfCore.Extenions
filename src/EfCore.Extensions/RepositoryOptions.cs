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
        public RepositoryOptions(TContext dbContext)
        {
            DbContext = dbContext;
            RepositoryRegistry = new RepositoryRegistry();
        }
    }
}