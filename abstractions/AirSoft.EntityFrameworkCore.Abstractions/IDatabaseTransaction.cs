using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Defines a contract for managing database transactions with both synchronous and asynchronous operations
    /// </summary>
    public interface IDatabaseTransaction : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Commits the current transaction synchronously
        /// </summary>
        public void Commit();

        /// <summary>
        /// Rolls back the current transaction synchronously
        /// </summary>
        public void Rollback();

        /// <summary>
        /// Commits the current transaction asynchronously
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task representing the asynchronous operation</returns>
        /// <exception cref="EntityException">Thrown when operation is cancelled</exception>
        public Task CommitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Rolls back the current transaction asynchronously
        /// </summary>
        /// <param name="cancellationToken">Optional cancellation token</param>
        /// <returns>Task representing the asynchronous operation</returns>
        /// <exception cref="EntityException">Thrown when operation is cancelled</exception>
        public Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
