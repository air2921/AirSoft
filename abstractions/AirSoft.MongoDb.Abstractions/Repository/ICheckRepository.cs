using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Documents;
using System.Linq.Expressions;

namespace AirSoft.MongoDb.Abstractions.Repository
{
    /// <summary>
    /// Provides repository operations for checking entity existence in the data store.
    /// Supports both synchronous and asynchronous operations with cancellation support.
    /// </summary>
    /// <typeparam name="TDocument">The type of document the repository will handle, which must inherit from <see cref="DocumentBase"/>.</typeparam>
    public interface ICheckRepository<TDocument> where TDocument : DocumentBase
    {
        /// <summary>
        /// Asynchronously checks if any documents match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply mongo filter.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>True if matching document exists; otherwise false.</returns>
        /// <exception cref="DocumentException">
        /// Thrown when:
        /// - Database operation fails
        /// - Operation is cancelled
        /// </exception>
        public Task<bool> IsExistsAsync(Expression<Func<TDocument, bool>> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if any documents match the specified filter.
        /// </summary>
        /// <param name="filter">The filter expression to apply mongo filter.</param>
        /// <returns>True if matching document exists; otherwise false.</returns>
        /// <exception cref="DocumentException">Thrown when database operation fails</exception>
        public bool IsExists(Expression<Func<TDocument, bool>> filter);
    }
}
