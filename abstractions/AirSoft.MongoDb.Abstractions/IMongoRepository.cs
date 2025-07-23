using AirSoft.MongoDb.Abstractions.Documents;
using AirSoft.MongoDb.Abstractions.Repository;

namespace AirSoft.MongoDb.Abstractions
{
    /// <summary>
    /// Represents a MongoDB-specific repository pattern for performing CRUD operations on documents of type <typeparamref name="TDocument"/>.
    /// Provides document operations with support for transactions, timeouts and cancellation.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface IMongoRepository<TDocument> :
        ICheckRepository<TDocument>,
        IGetRepository<TDocument>,
        IInsertRepository<TDocument>,
        IRemoveRepository<TDocument>,
        IReplaceRepository<TDocument>
        where TDocument : DocumentBase;
}
