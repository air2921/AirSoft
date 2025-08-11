using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Includer;
using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Query
{
    /// <summary>
    /// A builder class for constructing queries to retrieve a single entity with various options.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to query, must inherit from <see cref="EntityBase"/>.</typeparam>
    public sealed class SingleQueryBuilder<TEntity> :
        BaseEntityBuilder<SingleQueryBuilder<TEntity>, TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Indicates whether the query should ignore default query filters.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IgnoreDefaultQueryFilters { get; private set; } = false;

        /// <summary>
        /// Indicates whether the query should ignore auto includes.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IgnoreAutoInclude { get; private set; } = false;

        /// <summary>
        /// Indicates whether to use split query behavior to avoid cartesian explosion.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AsSplitQuery { get; private set; } = false;

        /// <summary>
        /// Indicates whether change tracking should be enabled.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AsTracking { get; private set; } = true;

        /// <summary>
        /// Whether to take the first entity (true) or last entity (false).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool TakeFirst { get; private set; } = true;

        /// <summary>
        /// Sets up includes using the fluent include builder.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IIncluder<TEntity>? Joiner { get; private set; }

        /// <summary>
        /// An expressions for filtering entities based on a condition.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<Expression<Func<TEntity, bool>>> Filters { get; private set; } = [];

        /// <summary>
        /// Gets the projection selector expression that transforms the query results.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TEntity, TEntity>>? Selector { get; private set; }

        /// <summary>
        /// A list of sorting expressions and their directions.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<(Expression<Func<TEntity, object?>> Expression, bool Descending)> SortOptions { get; } = [];

        /// <summary>
        /// Creates a new instance of SingleQueryBuilder.
        /// </summary>
        public static SingleQueryBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets whether to ignore default query filters.
        /// </summary>
        /// <param name="ignore">True to ignore default filters.</param>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithIgnoreQueryFilters(bool ignore = true)
        {
            IgnoreDefaultQueryFilters = ignore;
            return this;
        }

        /// <summary>
        /// Sets whether to ignore auto includes.
        /// </summary>
        /// <param name="ignore">True to ignore auto includes.</param>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithIgnoreAutoInclude(bool ignore = true)
        {
            IgnoreAutoInclude = ignore;
            return this;
        }

        /// <summary>
        /// Sets whether to use split query behavior.
        /// </summary>
        /// <param name="split">True to use split query (recommended when including collections).</param>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithSplitQuery(bool split = true)
        {
            AsSplitQuery = split;
            return this;
        }

        /// <summary>
        /// Sets whether to enable change tracking.
        /// </summary>
        /// <param name="tracking">True to enable tracking.</param>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithTracking(bool tracking = true)
        {
            AsTracking = tracking;
            return this;
        }

        /// <summary>
        /// Sets whether to take the first entity (true) or last entity (false).
        /// </summary>
        /// <param name="takeFirst">True to take first entity, false for last.</param>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithTakeFirst(bool takeFirst = true)
        {
            TakeFirst = takeFirst;
            return this;
        }

        /// <summary>
        /// Sets up includes using the fluent ощшт builder.
        /// </summary>
        /// <returns>The current builder instance.</returns>
        public SingleQueryBuilder<TEntity> WithJoiner(Func<IIncluder<TEntity>, IIncluder<TEntity>> config)
        {
            Joiner = config(new Includer<TEntity>());
            return this;
        }

        /// <summary>
        /// Sets the filter expression for the query.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when filter is null</exception>
        public SingleQueryBuilder<TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
        {
            _ = filter ?? throw new InvalidArgumentException($"Using a {nameof(WithFilter)} without filter expression is not allowed");
            Filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Sets a projection selector for the query results.
        /// </summary>
        /// <param name="selector">The projection expression that transforms the query results.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when selector is null</exception>
        public SingleQueryBuilder<TEntity> WithProjection(Expression<Func<TEntity, TEntity>> selector)
        {
            Selector = selector ?? throw new InvalidArgumentException($"Using a {nameof(WithProjection)} without projection expression is not allowed");
            return this;
        }

        /// <summary>
        /// Specifies an ordering expression and direction.If there is at least one ordering expression and direction, adds the next ordering expression and direction. (Does not overwrite !)
        /// </summary>
        /// <param name="expression">The ordering expression.</param>
        /// <param name="descending">True for descending order.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null</exception>
        public SingleQueryBuilder<TEntity> WithOrdering(
            Expression<Func<TEntity, object?>> expression,
            bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithOrdering)} without order expression is not allowed");

            SortOptions.Add((expression, descending));
            return this;
        }
    }
}