using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Query
{
    /// <summary>
    /// Defines a contract for building configurable range queries with support for filtering, sorting, pagination and eager loading.
    /// Provides control over EF Core query behaviors like tracking, query splitting and includes.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to query, must implement <see cref="IEntityBase"/>.</typeparam>
    /// <remarks>
    /// Enables building complex queries through method chaining while maintaining separation between query construction and execution.
    /// </remarks>
    public interface IRangeQueryBuilder<TEntity> where TEntity : IEntityBase
    {
        /// <summary>
        /// Indicates whether the query should ignore default query filters.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IgnoreDefaultQueryFilters { get; }

        /// <summary>
        /// Indicates whether the query should ignore auto includes.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IgnoreAutoInclude { get; }

        /// <summary>
        /// Indicates whether to use split query behavior to avoid cartesian explosion.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AsSplitQuery { get; }

        /// <summary>
        /// Indicates whether change tracking should be disabled.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AsNoTracking { get; }

        /// <summary>
        /// Sets up includes using the fluent include builder.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IIncluder<TEntity>? Joiner { get; }

        /// <summary>
        /// An expressions for filtering entities based on a condition.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<Expression<Func<TEntity, bool>>> Filters { get; }

        /// <summary>
        /// Gets the projection selector expression that transforms the query results.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TEntity, TEntity>>? Selector { get; }

        /// <summary>
        /// A list of sorting expressions and their directions.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<(Expression<Func<TEntity, object?>> Expression, bool Descending)> SortOptions { get; }

        /// <summary>
        /// The number of entities to skip.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Skip { get; }

        /// <summary>
        /// The number of entities to take.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Take { get; }
    }
}
