using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EfCore.Extensions;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private bool _disposed;

    public Repository(RepositoryOptions repositoryOptions)
    {
        Options = repositoryOptions;
        Context = Options.DbContext;
    }

    public RepositoryOptions Options { get; }

    public DbContext Context { get;private set; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public List<TEntity> GetAll(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.ToList();
    }

    public Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.ToListAsync();
    }

    public TEntity FirstOrDefault(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.FirstOrDefault();
    }

    public Task<TEntity> FirstOrDefaultAsync(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.FirstOrDefaultAsync();
    }

    public Task<bool> AnyAsync(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.AnyAsync();
    }

    public bool Any(ISpecification<TEntity> spec = null)
    {
        var q = ApplySpecification(spec);

        return q.Any();
    }

    public void Add(TEntity item)
    {
        if(item == null) { throw new ArgumentNullException(nameof(item)); }

        Context.Add(item);
    }

    public void Add(params TEntity[] items)
    {
        if(items == null || items.Length == 0) { throw new ArgumentNullException(nameof(items)); }

        Context.AddRange(items);
    }

    public void Add(IEnumerable<TEntity> items)
    {
        if(items == null) { throw new ArgumentNullException(nameof(items)); }

        Context.AddRange(items);
    }

    public async Task<TEntity> AddAsync(TEntity item)
    {
        if(item == null) { throw new ArgumentNullException(nameof(item)); }

        Context.Add(item);
        var result = await Context.AddAsync(item);

        return result.Entity;
    }

    public Task AddAsync(params TEntity[] items) => items is null
        ? throw new ArgumentNullException(nameof(items))
        : Context.AddRangeAsync(items);

    public Task AddAsync(IEnumerable<TEntity> items) => items == null
        ? throw new ArgumentNullException(nameof(items))
        : Context.AddRangeAsync(items);

    public void Detach(TEntity entity)
    {
        if(entity == null) { throw new ArgumentNullException(nameof(entity)); }

        Context.Entry(entity).State = EntityState.Detached;
    }

    public void Detach(IEnumerable<TEntity> entities)
    {
        if(entities == null) { throw new ArgumentNullException(nameof(entities)); }

        foreach(var entity in entities) { Context.Entry(entity).State = EntityState.Detached; }
    }

    public void Update(TEntity item)
    {
        if(item == null) { throw new ArgumentNullException(nameof(item)); }

        Context.Update(item);
    }

    public void Update(params TEntity[] items)
    {
        if(items == null) { throw new ArgumentNullException(nameof(items)); }

        Context.UpdateRange(items);
    }

    public void Remove(TEntity item)
    {
        if(item == null) { throw new ArgumentNullException(nameof(item)); }

        Context.Remove(item);
    }

    public void Remove(params TEntity[] items)
    {
        if(items is null) { throw new ArgumentNullException(nameof(items)); }

        Context.RemoveRange(items);
    }

    public void Remove(IEnumerable<TEntity> items)
    {
        if(items == null) { throw new ArgumentNullException(nameof(items)); }

        Context.RemoveRange(items);
    }

    public Task<int> SaveAsync() => Context.SaveChangesAsync();

    public int Save() => Context.SaveChanges();

    public bool IsUpdated(TEntity entity) => Context.ChangeTracker.Entries<TEntity>()
        .Any(x => x.Entity == entity && x.State == EntityState.Modified);

    /// <inheritdoc />
    public IRepository<TAnotherEntity> GetRepository<TAnotherEntity>() where TAnotherEntity : class
        => Options.RepositoryRegistry.GetRepository<TAnotherEntity>(Options);

    public void Attach(TEntity entity)
    {
        if(entity == null) { throw new ArgumentNullException(nameof(entity)); }

        Context.Attach(entity);
    }

    public void Attach(params TEntity[] entities)
    {
        if(entities == null) { throw new ArgumentNullException(nameof(entities)); }

        Context.AttachRange(entities);
    }

    protected virtual void Dispose(bool disposable)
    {
        if(_disposed) { return; }

        if(disposable)
        {
            Context?.Dispose();
            Context = null;
        }

        _disposed = true;
    }

    protected virtual IQueryable<TEntity> GetQueryable() => Context.Set<TEntity>().AsQueryable();

    protected virtual IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec = null)
    {
        var q = GetQueryable();

        if(spec != null)
        {
            var evaluator = new SpecificationEvaluator();
            q = evaluator.GetQuery(q, spec);
        }

        return q;
    }
}