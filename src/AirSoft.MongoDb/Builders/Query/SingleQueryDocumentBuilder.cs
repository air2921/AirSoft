using AirSoft.Exceptions;
using AirSoft.MongoDb.Builders.Base;
using AirSoft.MongoDb.Documents;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Builders.Query
{
    /// <summary>
    /// A class that helps build queries for filtering and sorting for single document query.
    /// <para>This class is designed to assist with querying document of type <typeparamref name="TDocument"/>.</para>
    /// </summary>
    /// <typeparam name="TDocument">The type of the document to query, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public sealed class SingleQueryDocumentBuilder<TDocument> : BaseDocumentBuilder<RangeQueryDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// An expression for filtering documents based on a condition.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal Expression<Func<TDocument, bool>>? Filter { get; private set; }

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
            Filter = filter ?? throw new InvalidArgumentException($"Using a {nameof(WithFilter)} without filter expression is not allowed");
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
        /// Sets the primary ordering expression and direction.
        /// </summary>
        /// <param name="expression">The ordering expression.</param>
        /// <param name="descending">True for descending order (default is true).</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when the expression is null.</exception>
        public SingleQueryDocumentBuilder<TDocument> WithOrdering(Expression<Func<TDocument, object?>> expression, bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithOrdering)} without order expression is not allowed");

            SortOptions.Clear();
            SortOptions.Add((expression, descending));
            return this;
        }

        /// <summary>
        /// Adds a secondary ordering expression and direction.
        /// </summary>
        /// <param name="expression">The ordering expression.</param>
        /// <param name="descending">True for descending order (default is true).</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when the expression is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when no primary ordering has been set.</exception>
        public SingleQueryDocumentBuilder<TDocument> WithThenOrdering(Expression<Func<TDocument, object?>> expression, bool descending = true)
        {
            if (expression is null)
                throw new InvalidArgumentException($"Using a {nameof(WithThenOrdering)} without order expression is not allowed");

            if (!SortOptions.Any())
                throw new InvalidOperationException($"Cannot use {nameof(WithThenOrdering)} without first calling {nameof(WithOrdering)}");

            SortOptions.Add((expression, descending));
            return this;
        }
    }
}
