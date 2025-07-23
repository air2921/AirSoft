using AirSoft.MongoDb.Abstractions.Builders.Base;
using AirSoft.MongoDb.Abstractions.Documents;
using System.ComponentModel;

namespace AirSoft.MongoDb.Abstractions.Builders.State.Replace
{
    /// <summary>
    /// Fluent builder for replacing range documents in MongoDB.
    /// Handles documents initialization.
    /// </summary>
    /// <typeparam name="TDocument">Type of document to replace, must inherit from DocumentBase</typeparam>
    public sealed class ReplaceRangeDocumentBuilder<TDocument> : BaseDocumentStateBuilder<ReplaceRangeDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Gets the documents to be replaced
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IReadOnlyCollection<TDocument> Documents { get; private set; } = [];

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static ReplaceRangeDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets documents for batch replacing
        /// </summary>
        /// <param name="documents">Documents to replace</param>
        public ReplaceRangeDocumentBuilder<TDocument> WithDocuments(IEnumerable<TDocument> documents)
        {
            Documents = [.. documents];
            return this;
        }
    }

}
