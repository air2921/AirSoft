using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.EntityFrameworkCore.Abstractions.Enums;
using AirSoft.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.State.Remove
{
    /// <summary>
    /// A builder class for configuring parameters to remove a range of entities.
    /// Provides flexible ways to specify entities for removal either by entity instances or their identifiers.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities to remove, must inherit from <see cref="EntityBase"/>.</typeparam>
    public sealed class RemoveRangeBuilder<TEntity> :
        BaseEntityStateBuilder<RemoveRangeBuilder<TEntity>, TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Collection of entities to be removed directly.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TEntity> Entities { get; private set; } = [];

        /// <summary>
        /// Collection of entity identifiers for entities to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<string> Identifiers { get; private set; } = [];

        /// <summary>
        /// Filter expression to select entities for removal.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TEntity, bool>>? Filter { get; private set; }

        /// <summary>
        /// Specifies the removal strategy (by object, identifiers or filter).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EntityRemoveStrategy RemoveStrategy { get; private set; }

        /// <summary>
        /// Specifies whether entities should be deleted without the change tracker.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsExeсutable { get; private set; } = false;

        /// <summary>
        /// Creates a new instance of RemoveRangeBuilder.
        /// </summary>
        /// <returns>New instance of RemoveRangeBuilder</returns>
        public static RemoveRangeBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets the entities to be removed directly.
        /// </summary>
        /// <param name="entities">Collection of entities to remove.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveRangeBuilder<TEntity> WithEntities(IEnumerable<TEntity> entities)
        {
            Entities = [.. entities];
            RemoveStrategy = EntityRemoveStrategy.Entity;
            return this;
        }

        /// <summary>
        /// Sets the identifiers of entities to be removed.
        /// </summary>
        /// <param name="identifiers">Collection of entity identifiers.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveRangeBuilder<TEntity> WithIdentifiers(IEnumerable<string> identifiers)
        {
            Identifiers = [.. identifiers];
            RemoveStrategy = EntityRemoveStrategy.Identifier;
            return this;
        }

        /// <summary>
        /// Sets the filter expression to select entities for removal.
        /// </summary>
        /// <param name="filter">Filter expression to select entities.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when filter is null</exception>
        public RemoveRangeBuilder<TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
        {
            Filter = filter ?? throw new InvalidArgumentException("Filter for filtering entities cannot be null");
            RemoveStrategy = EntityRemoveStrategy.Filter;
            return this;
        }

        /// <summary>
        /// Explicitly sets the removal strategy.
        /// </summary>
        /// <param name="strategy">The removal strategy to use.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveRangeBuilder<TEntity> WithRemoveStrategy(EntityRemoveStrategy strategy)
        {
            RemoveStrategy = strategy;
            return this;
        }

        /// <summary>
        /// Configures whether to perform direct database deletion (bypassing change tracking and soft delete).
        /// </summary>
        /// <remarks>
        /// WARNING: When enabled, deletes records directly in database, ignoring:
        /// - Change tracking
        /// - Soft delete markers
        /// - Delete interceptors
        /// - Calling <see cref="BaseEntityStateBuilder{TRestoreRangeBuilder, TEntity}.WithSaveChanges(bool)"/> is not required (changes are applied immediately)
        /// This operation is irreversible.
        /// </remarks>
        /// <param name="executable">True for direct DB deletion, false for normal EF deletion</param>
        /// <returns>The current builder instance</returns>
        public RemoveRangeBuilder<TEntity> WithExecution(bool executable = true)
        {
            IsExeсutable = executable;
            return this;
        }
    }
}
