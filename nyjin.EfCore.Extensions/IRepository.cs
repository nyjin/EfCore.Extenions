using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace nyjin.EfCore.Extensions
{
    public interface IRepository : IDisposable
    {
        Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity: class;

        EntityEntry<TEntity> Add<TEntity>(TEntity item) where TEntity: class;

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity item) where TEntity: class;
    }

}