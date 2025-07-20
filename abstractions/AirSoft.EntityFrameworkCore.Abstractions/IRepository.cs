using AirSoft.EntityFrameworkCore.Abstractions.Repository;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Represents a generic repository pattern for performing CRUD operations on entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity. It must inherit from <see cref="IEntityBase"/>.</typeparam>
    public interface IRepository<TEntity> :
        ICheckRepository<TEntity>,
        IGetRepository<TEntity>,
        IAddRepository<TEntity>,
        IRemoveRepository<TEntity>,
        IRestoreRepository<TEntity>,
        IUpdateRepository<TEntity>
        where TEntity : IEntityBase;
}