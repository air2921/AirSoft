using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Builders.Base;
using AirSoft.MongoDb.Abstractions.Documents;
using AirSoft.MongoDb.Abstractions.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using System.ComponentModel;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions.Builders.State.Remove
{
    /// <summary>
    /// Builder for removing a single document from MongoDB.
    /// </summary>
    /// <typeparam name="TDocument">Document type inheriting from DocumentBase</typeparam>
    public sealed class RemoveSingleDocumentBuilder<TDocument> : BaseDocumentStateBuilder<RemoveSingleDocumentBuilder<TDocument>, TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Identifier of the document to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ObjectId? Id { get; private set; }

        /// <summary>
        /// The document to be removed.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TDocument? Document { get; private set; }

        /// <summary>
        /// Filter expression to select documents for removal.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TDocument, bool>>? Filter { get; private set; }

        /// <summary>
        /// Specifies the removal mode (by object, identifiers or filter).
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DocumentRemoveMode RemoveMode { get; private set; }

        /// <summary>
        /// Gets the MongoDB remove options
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DeleteOptions Options { get; private set; } = new();

        /// <summary>
        /// Creates a new builder instance
        /// </summary>
        public static RemoveSingleDocumentBuilder<TDocument> Create() => new();

        /// <summary>
        /// Sets the identifier of document to be removed.
        /// </summary>
        /// <param name="id">Document identifier.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when id of document is null</exception>
        public RemoveSingleDocumentBuilder<TDocument> WithIdentifier(ObjectId id)
        {
            Id = id;
            RemoveMode = DocumentRemoveMode.Identifier;
            return this;
        }

        /// <summary>
        /// Sets the document instance to be removed.
        /// </summary>
        /// <param name="document">Document instance to remove.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when document is null</exception>
        public RemoveSingleDocumentBuilder<TDocument> WithDocument(TDocument document)
        {
            Document = document ?? throw new InvalidArgumentException("Document for remove cannot be null");
            RemoveMode = DocumentRemoveMode.Document;
            return this;
        }

        /// <summary>
        /// Sets the filter expression to select single document for removal.
        /// </summary>
        /// <param name="filter">Filter expression to select document.</param>
        /// <returns>The current builder instance.</returns>
        /// <exception cref="InvalidArgumentException">Thrown when document is null</exception>
        public RemoveSingleDocumentBuilder<TDocument> WithFilter(Expression<Func<TDocument, bool>> filter)
        {
            Filter = filter ?? throw new InvalidArgumentException("Filter for filtering document cannot be null");
            RemoveMode = DocumentRemoveMode.Filter;
            return this;
        }

        /// <summary>
        /// Explicitly sets the removal mode.
        /// </summary>
        /// <param name="mode">The removal mode to use.</param>
        /// <returns>The current builder instance.</returns>
        public RemoveSingleDocumentBuilder<TDocument> WithRemoveMode(DocumentRemoveMode mode)
        {
            RemoveMode = mode;
            return this;
        }

        /// <summary>
        /// Configures options for the remove operation
        /// </summary>
        /// <param name="options">MongoDB remove options</param>
        /// <exception cref="InvalidArgumentException">Thrown when options are null</exception>
        public RemoveSingleDocumentBuilder<TDocument> WithOptions(DeleteOptions options)
        {
            Options = options ?? throw new InvalidArgumentException("Insert options cannot be null");
            return this;
        }
    }
}
