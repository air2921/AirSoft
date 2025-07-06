using AirSoft.Exceptions;
using AirSoft.MongoDb.Builders.Base;
using AirSoft.MongoDb.Documents;
using MongoDB.Driver;
using System.ComponentModel;

namespace AirSoft.MongoDb.Builders.State.Insert
{
    /// <summary>
    /// Fluent builder for inserting single documents in MongoDB.
    /// Handles document initialization and insert options configuration.
    /// </summary>
    /// <typeparam name="TDocument">Type of document to create, must inherit from DocumentBase</typeparam>
    public sealed class InsertSingleDocumentBuilder<TDocument> : BaseDocumentStateBuilder<InsertSingleDocumentBuilder<TDocument>, TDocument>
        where TDocument : DocumentBase
    {
        /// <summary>
        /// Gets the document to be inserted
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal TDocument Document { get; private set; } = default!;

        /// <summary>
        /// Gets the MongoDB insert options
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        internal InsertOneOptions Options { get; private set; } = new();

        /// <summary>
        /// Creates a new instance of the builder
        /// </summary>
        public static InsertSingleDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets the document to be inserted
        /// </summary>
        /// <param name="document">Document instance</param>
        /// <exception cref="InvalidArgumentException">Thrown when document is null</exception>
        public InsertSingleDocumentBuilder<TDocument> WithDocument(TDocument document)
        {
            Document = document ?? throw new InvalidArgumentException("Document cannot be null");
            return this;
        }

        /// <summary>
        /// Configures options for the insert operation
        /// </summary>
        /// <param name="options">MongoDB insert options</param>
        /// <exception cref="InvalidArgumentException">Thrown when options are null</exception>
        public InsertSingleDocumentBuilder<TDocument> WithOptions(InsertOneOptions options)
        {
            Options = options ?? throw new InvalidArgumentException("Insert options cannot be null");
            return this;
        }
    }
}
