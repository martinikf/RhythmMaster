using System.ComponentModel.DataAnnotations;
using Domain.Common;

namespace Domain.Entities;

public abstract class Entity<TEntityId> : IEntity
{
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    protected Entity()
    {
    }

    public TEntityId Id { get; init; } = default!;
}