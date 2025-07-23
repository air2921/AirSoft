using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Documents;
using MongoDB.Driver;
using System.ComponentModel;

namespace AirSoft.MongoDb.Abstractions.Builders.Base
{
    /// <summary>
    /// Base class for document state operations (create/update/delete).
    /// </summary>
    /// <typeparam name="TBuilder">Concrete builder type for fluent chaining</typeparam>
    /// <typeparam name="TEntity">Document type (must inherit from <see cref="DocumentBase"/>)</typeparam>
    public abstract class BaseDocumentStateBuilder<TBuilder, TEntity> :
        BaseDocumentBuilder<TBuilder, TEntity>
        where TBuilder : BaseDocumentStateBuilder<TBuilder, TEntity>
        where TEntity : DocumentBase
    {
        /// <summary>
        /// Gets the MongoDB session handle for transaction support
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IClientSessionHandle? Session { get; private set; }

        /// <summary>
        /// Attaches a MongoDB session handle for transactional operations
        /// </summary>
        /// <param name="session">Active MongoDB session</param>
        /// <returns>Current builder instance</returns>
        /// <exception cref="InvalidArgumentException">If sessionHandle is null</exception>
        public TBuilder WithSession(IClientSessionHandle session)
        {
            Session = session ?? throw new InvalidArgumentException("Session cannot be null");
            return (TBuilder)this;
        }
    }
}
