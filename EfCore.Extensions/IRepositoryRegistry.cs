using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions
{
    public interface IRepositoryRegistry
    {
        IRepository<TEntity> GetRepository<TEntity>(DbContext context) where TEntity : class;
    }
}