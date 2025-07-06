using AirSoft.Exceptions;
using AirSoft.MongoDb.Builders.Base;
using AirSoft.MongoDb.Documents;
using MongoDB.Driver;
using System.ComponentModel;

namespace AirSoft.MongoDb.Builders.State.Replace
{
    /// <summary>
    /// Fluent builder for replacing single document in MongoDB.
    /// Handles document initialization and replace options configuration.
    /// </summary>
    /// <typeparam name="TDocument">Type of document to replace, must inherit from DocumentBase</typeparam>
    public sealed class ReplaceSingleDocumentBuilder<TDocument> : BaseDocumentStateBuilder<ReplaceSingleDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Gets the document to be replaced
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal TDocument Document { get; private set; } = default!;

        /// <summary>
        /// Gets the MongoDB replace options
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal ReplaceOptions Options { get; private set; } = new();

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static ReplaceSingleDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets the document to be replaced
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <exception cref="InvalidArgumentException">Thrown when document is null</exception>
        public ReplaceSingleDocumentBuilder<TDocument> WithDocument(TDocument document)
        {
            Document = document ?? throw new InvalidArgumentException("Document cannot be null");
            return this;
        }

        /// <summary>
        /// Configures options for the replace operation
        /// </summary>
        /// <param name="options">MongoDB replace options</param>
        /// <exception cref="InvalidArgumentException">Thrown when options are null</exception>
        public ReplaceSingleDocumentBuilder<TDocument> WithOptions(ReplaceOptions options)
        {
            Options = options ?? throw new InvalidArgumentException("Insert options cannot be null");
            return this;
        }
    }
}
