using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.Base;
using AirSoft.MongoDb.Abstractions.Documents;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions.Builders.Query
{
    /// <summary>
    /// A class that helps build queries for filtering, sorting, and paginating documents in a range query.
    /// <para>This class is designed to assist with querying documents of type <typeparamref name="TDocument"/>.</para>
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to query, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public sealed class RangeQueryDocumentBuilder<TDocument> : BaseDocumentBuilder<RangeQueryDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// An expressions for filtering documents based on a condition.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<Expression<Func<TDocument, bool>>> Filters { get; private set; } = [];

        /// <summary>
        /// Gets the projection selector expression that transforms the query results.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TDocument, TDocument>>? Selector { get; private set; }

        /// <summary>
        /// A list of sorting expressions and their directions.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IList<(Expression<Func<TDocument, object?>> Expression, bool Descending)> SortOptions { get; } = [];

        /// <summary>
        /// The number of documents to skip (for pagination).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Skip { get; private set; } = 0;

        /// <summary>
        /// The number of documents to take (for pagination).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int? Take { get; private set; } = 100;

        /// <summary>
        /// Creates a new instance of RangeQueryDocumentBuilder with default settings.
        /// </summary>
        public static RangeQueryDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets the filter expression for the query.
        /// </summary>
        /// <param name="filter">The filter expression to apply.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when the filter expression is null.</exception>
        public RangeQueryDocumentBuilder<TDocument> WithFilter(Expression<Func<TDocument, bool>> filter)
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
        public RangeQueryDocumentBuilder<TDocument> WithProjection(Expression<Func<TDocument, TDocument>> selector)
        {
            Selector = selector ?? throw new InvalidArgumentException($"Using a {nameof(WithProjection)} without projection expression is not allowed");
            return this;
        }

        /// <summary>
        /// Specifies an ordering expression and direction.If there is at least one ordering expression and direction, adds the next ordering expression and direction. (Does not overwrite !)
        /// </summary>
        /// <param name="expression">The ordering expression.</param>
        /// <param name="descending">True for descending order (default is true).</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when the expression is null.</exception>
        public RangeQueryDocumentBuilder<TDocument> WithOrdering(Expression<Func<TDocument, object?>> expression, bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithOrdering)} without order expression is not allowed");

            SortOptions.Add((expression, descending));
            return this;
        }

        /// <summary>
        /// Sets pagination parameters with validation:
        /// <para>skip more or equals than 0</para>
        /// <para>take more than 0</para>
        /// <para>take less or equals than 1000 when constraints enabled</para>
        /// </summary>
        /// <param name="skip">Documents to skip (>= 0)</param>
        /// <param name="take">Documents to take (1-1000)</param>
        /// <returns>Current builder instance</returns>
        /// <exception cref="InvalidArgumentException">On invalid skip/take values</exception>
        public RangeQueryDocumentBuilder<TDocument> WithPagination(int skip, int take)
        {
            if (skip < 0)
                throw new InvalidArgumentException($"{nameof(skip)} cannot be negative");

            if (take <= 0)
                throw new InvalidArgumentException($"{nameof(take)} must be positive");

            if (take > 1000 && IsEnabledConstraints)
                throw new InvalidArgumentException($"{nameof(take)} exceeds 1000 with constraints enabled");

            Skip = skip;
            Take = take;
            return this;
        }

        /// <summary>
        /// Removes pagination limits (skip/take) from the query.
        /// </summary>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when builder constraints are enabled.</exception>
        [Obsolete("Do not disable quantity limit, unless it is done intentionally")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RangeQueryDocumentBuilder<TDocument> WithNoQuantityLimit()
        {
            if (IsEnabledConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithNoQuantityLimit)} with enabled builder contraints is not allowed");

            Skip = null;
            Take = null;
            return this;
        }
    }
}
