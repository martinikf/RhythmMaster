using Application.Interfaces.IRepositories;
using Domain.Common;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public abstract class Repository <TEntity, TEntityId> : IRepository<TEntity, TEntityId> 
    where TEntity : class, IEntity 
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(TEntityId id)
    {
        return await DbContext.FindAsync<TEntity>(id);
    }

    public async Task Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
        await DbContext.SaveChangesAsync();
    }
    
    public async Task Remove(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }
    
    public async Task Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync();
    }

}