using AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions.Includer;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Base;
using AirSoft.EntityFrameworkCore.Abstractions.Builders.Includer;
using AirSoft.Exceptions;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Query
{
    /// <summary>
    /// A class that helps build queries for filtering, sorting, and including related entities in a range query.
    /// <para>This class is designed to assist with pagination and custom queries for entities of type <typeparamref name="TEntity"/>.</para>
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to query.</typeparam>
    public sealed class RangeQueryBuilder<TEntity> :
        BaseEntityBuilder<RangeQueryBuilder<TEntity>, TEntity> where TEntity : IEntityBase
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
        /// Indicates whether change tracking should be disabled.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AsNoTracking { get; private set; } = false;

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
        /// The number of entities to skip.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Skip { get; private set; } = 0;

        /// <summary>
        /// The number of entities to take.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Take { get; private set; } = 100;

        /// <summary>
        /// Creates a new instance of RangeQueryBuilder with default settings.
        /// </summary>
        public static RangeQueryBuilder<TEntity> Create() => new();

        /// <summary>
        /// Sets whether to ignore default query filters.
        /// </summary>
        /// <param name="ignore">True to ignore default filters.</param>
        /// <returns>The current builder instance.</returns>
        public RangeQueryBuilder<TEntity> WithIgnoreQueryFilters(bool ignore = true)
        {
            IgnoreDefaultQueryFilters = ignore;
            return this;
        }

        /// <summary>
        /// Sets whether to ignore auto includes.
        /// </summary>
        /// <param name="ignore">True to ignore auto includes.</param>
        /// <returns>The current builder instance.</returns>
        public RangeQueryBuilder<TEntity> WithIgnoreAutoInclude(bool ignore = true)
        {
            IgnoreAutoInclude = ignore;
            return this;
        }

        /// <summary>
        /// Sets whether to use split query behavior.
        /// </summary>
        /// <param name="split">True to use split query (recommended when including collections).</param>
        /// <returns>The current builder instance.</returns>
        public RangeQueryBuilder<TEntity> WithSplitQuery(bool split = true)
        {
            AsSplitQuery = split;
            return this;
        }

        /// <summary>
        /// Sets up includes using the fluent join builder.
        /// </summary>
        /// <returns>The current builder instance.</returns>
        public RangeQueryBuilder<TEntity> WithJoiner(Func<IIncluder<TEntity>, IIncluder<TEntity>> config)
        {
            Joiner = config(new Includer<TEntity>());
            return this;
        }

        /// <summary>
        /// Sets whether to disable change tracking.
        /// </summary>
        /// <param name="noTracking">True to disable tracking.</param>
        /// <returns>The current builder instance.</returns>
        public RangeQueryBuilder<TEntity> WithNoTracking(bool noTracking = true)
        {
            AsNoTracking = noTracking;
            return this;
        }

        /// <summary>
        /// Sets the filter expression for the query.
        /// </summary>
        /// <param name="filter">The filter expression.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when filter is null</exception>
        public RangeQueryBuilder<TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
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
        public RangeQueryBuilder<TEntity> WithProjection(Expression<Func<TEntity, TEntity>> selector)
        {
            Selector = selector ?? throw new InvalidArgumentException($"Using a {nameof(WithProjection)} without projection expression is not allowed");
            return this;
        }

        /// <summary>
        /// Sets the pagination parameters.
        /// </summary>
        /// <param name="skip">Number of entities to skip.</param>
        /// <param name="take">Number of entities to take.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">
        /// Thrown when: skip < 0, take ≤ 0, or take > 1000 (unless constraints ignored)
        /// </exception>
        public RangeQueryBuilder<TEntity> WithPagination(int skip, int take)
        {
            if (skip < 0)
                throw new InvalidArgumentException($"Using a {nameof(WithPagination)} with {nameof(skip)} param less than zero is not allowed");

            if (take <= 0)
                throw new InvalidArgumentException($"Using a {nameof(WithPagination)} with {nameof(take)} param less or zero is not allowed");

            if (take > 1000 && !IsIgnoredBuilderConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithPagination)} with {nameof(take)} param more than 1000 with enabled builder contraints is not allowed");

            Skip = skip;
            Take = take;
            return this;
        }

        /// <summary>
        /// Specifies an ordering expression and direction.If there is at least one ordering expression and direction, adds the next ordering expression and direction. (Does not overwrite !)
        /// </summary>
        /// <param name="expression">The ordering expression.</param>
        /// <param name="descending">True for descending order.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when expression is null</exception>
        public RangeQueryBuilder<TEntity> WithOrdering(
            Expression<Func<TEntity, object?>> expression,
            bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithOrdering)} without order expression is not allowed");

            SortOptions.Add((expression, descending));
            return this;
        }

        /// <summary>
        /// Removes pagination limits (skip/take) from the query when builder constraints are ignored.
        /// </summary>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when builder constraints are enabled</exception>
        [Obsolete("Do not disable quantity limit, unless it is done intentionally")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RangeQueryBuilder<TEntity> WithNoQuantityLimit()
        {
            if (IsIgnoredBuilderConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithNoQuantityLimit)} with enabled builder contraints is not allowed");

            Skip = null;
            Take = null;
            return this;
        }
    }
}