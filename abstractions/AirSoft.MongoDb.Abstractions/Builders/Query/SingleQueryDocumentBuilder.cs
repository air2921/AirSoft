using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.Base;
using AirSoft.MongoDb.Abstractions.Documents;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions.Builders.Query
{
    /// <summary>
    /// A class that helps build queries for filtering and sorting for single document query.
    /// <para>This class is designed to assist with querying document of type <typeparamref name="TDocument"/>.</para>
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to query, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public sealed class SingleQueryDocumentBuilder<TDocument> : BaseDocumentBuilder<RangeQueryDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// An expressions for filtering documents based on a condition.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal IList<Expression<Func<TDocument, bool>>> Filters { get; private set; } = [];

        /// <summary>
        /// Gets the projection selector expression that transforms the query results.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Expression<Func<TDocument, TDocument>>? Selector { get; private set; }

        /// <summary>
        /// A list of sorting expressions and their directions.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal IList<(Expression<Func<TDocument, object?>> Expression, bool Descending)> SortOptions { get; } = [];

        /// <summary>
        /// Creates a new instance of SingleQueryDocumentBuilder with default settings.
        /// </summary>
        public static SingleQueryDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets the filter expression for the query.
        /// </summary>
        /// <param name="filter">The filter expression to apply.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when the filter expression is null.</exception>
        public SingleQueryDocumentBuilder<TDocument> WithFilter(Expression<Func<TDocument, bool>> filter)
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
        public SingleQueryDocumentBuilder<TDocument> WithProjection(Expression<Func<TDocument, TDocument>> selector)
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
        public SingleQueryDocumentBuilder<TDocument> WithOrdering(Expression<Func<TDocument, object?>> expression, bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithOrdering)} without order expression is not allowed");

            SortOptions.Add((expression, descending));
            return this;
        }
    }
}
