using Domain.Common;

namespace Application.Interfaces.IRepositories;

public interface IRepository<TEntity, in TEntityId> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(TEntityId id);

    public Task Add(TEntity entity);

    public Task Remove(TEntity entity);

    public Task Update(TEntity entity);
}