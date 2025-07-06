using AirSoft.Exceptions;
using AirSoft.MongoDb.Builders.Base;
using AirSoft.MongoDb.Documents;
using MongoDB.Driver;
using System.ComponentModel;

namespace AirSoft.MongoDb.Builders.State.Insert
{
    /// <summary>
    /// Builder for inserting multiple documents in MongoDB with batch insert operations.
    /// </summary>
    /// <typeparam name="TDocument">Document type inheriting from DocumentBase</typeparam>
    public sealed class InsertRangeDocumentBuilder<TDocument> : BaseDocumentStateBuilder<InsertRangeDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Documents to be inserted
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal IReadOnlyCollection<TDocument> Documents { get; private set; } = [];

        /// <summary>
        /// Options to insert documents
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal InsertManyOptions Options { get; private set; } = new();

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static InsertRangeDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets documents for batch insertion
        /// </summary>
        /// <param name="documents">Documents to insert</param>
        public InsertRangeDocumentBuilder<TDocument> WithDocuments(IEnumerable<TDocument> documents)
        {
            Documents = [.. documents];
            return this;
        }

        /// <summary>
        /// Configures batch insert options
        /// </summary>
        /// <param name="options">MongoDB insert many options</param>
        /// <exception cref="InvalidArgumentException">If options are null</exception>
        public InsertRangeDocumentBuilder<TDocument> WithOptions(InsertManyOptions options)
        {
            Options = options ?? throw new InvalidArgumentException("Options for creation cannot be null");
            return this;
        }
    }
}
