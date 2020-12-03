using System;
using System.Reflection.Metadata.Ecma335;

namespace EfCore.Extensions
{
    public interface IRepositoryRegistry
    {
        IRepository<TEntity> GetRepository<TEntity>(RepositoryOptions repositoryOptions) where TEntity : class;
    }
}