using AirSoft.MongoDb.Builders.Query;
using AirSoft.MongoDb.Documents;
using MongoDB.Driver;

namespace AirSoft.MongoDb.Extensions
{
    /// <summary>
    /// Provides extension methods for applying query configurations from a <see cref="RangeQueryDocumentBuilder{TDocument}"/>
    /// to MongoDB queries.
    /// </summary>
    /// <remarks>
    /// This static class contains helper methods to build MongoDB queries
    /// with filtering, sorting and pagination options.
    /// </remarks>
    internal static class RangeQueryDocumentBuilderExtension
    {
        /// <summary>
        /// Applies all configured query modifications from the <see cref="RangeQueryDocumentBuilder{TDocument}"/>
        /// to the MongoDB collection query.
        /// </summary>
        /// <typeparam name="TDocument">The document type, must inherit from <see cref="DocumentBase"/></typeparam>
        /// <param name="collection">The MongoDB collection to build query from</param>
        /// <param name="builder">The query builder containing configuration settings</param>
        /// <returns>The constructed MongoDB find fluent interface with applied configurations</returns>
        /// <remarks>
        /// <para>This method applies configurations in the following order:</para>
        /// <list type="number">
        /// <item><description>Filter conditions</description></item>
        /// <item><description>Projection to select data</description></item>
        /// <item><description>Sorting configuration</description></item>
        /// <item><description>Pagination (Skip/Limit) when both values are provided</description></item>
        /// </list>
        /// </remarks>
        internal static IFindFluent<TDocument, TDocument> ApplyBuilder<TDocument>(this IMongoCollection<TDocument> collection, RangeQueryDocumentBuilder<TDocument> builder) where TDocument : DocumentBase
        {
            var filters = builder.Filters.Select(expr => Builders<TDocument>.Filter.Where(expr)).ToArray();
            var filter = filters.Length != 0 ? Builders<TDocument>.Filter.And(filters) : Builders<TDocument>.Filter.Empty;

            var query = collection.Find(filter);

            if (builder.Selector is not null)
                query = query.Project(builder.Selector);

            if (builder.SortOptions.Count > 0)
            {
                var orderedQuery = builder.SortOptions[0].Descending
                    ? query.SortByDescending(builder.SortOptions[0].Expression)
                    : query.SortBy(builder.SortOptions[0].Expression);

                for (int i = 1; i < builder.SortOptions.Count; i++)
                {
                    orderedQuery = builder.SortOptions[i].Descending
                        ? orderedQuery.ThenByDescending(builder.SortOptions[i].Expression)
                        : orderedQuery.ThenBy(builder.SortOptions[i].Expression);
                }

                query = orderedQuery;
            }

            if (!builder.Skip.HasValue || !builder.Take.HasValue)
                return query;

            query = query.Skip(builder.Skip.Value).Limit(builder.Take.Value);

            return query;
        }
    }
}
