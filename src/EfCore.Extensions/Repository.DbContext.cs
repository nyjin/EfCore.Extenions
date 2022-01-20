using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions;

public class Repository<TEntity, TDbContext> : Repository<TEntity> where TEntity : class
    where TDbContext : DbContext
{
    public Repository(RepositoryOptions<TDbContext> repositoryOptions) : base(repositoryOptions) { }
}