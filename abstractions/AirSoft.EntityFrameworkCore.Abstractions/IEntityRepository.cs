using AirSoft.EntityFrameworkCore.Abstractions.Entities;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Comprehensive repository interface combining read-only and state management operations.
    /// Inherits functionality from both <see cref="IReadonlyEntityRepository{TEntity}"/> and <see cref="IStatefulEntityRepository{TEntity}"/>.
    /// Provides complete CRUD operations for entity management with both synchronous and asynchronous variants.
    /// </summary>
    /// <typeparam name="TEntity">Entity type, must inherit from <see cref="EntityBase"/></typeparam>
    public interface IEntityRepository<TEntity> :
        IReadonlyEntityRepository<TEntity>,
        IStatefulEntityRepository<TEntity>
        where TEntity : EntityBase;
}