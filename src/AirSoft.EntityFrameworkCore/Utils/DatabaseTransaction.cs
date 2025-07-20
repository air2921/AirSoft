using AirSoft.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirSoft.EntityFrameworkCore.Utils
{
    /// <summary>
    /// Provides a concrete implementation of <see cref="IDatabaseTransaction"/> that wraps an Entity Framework Core transaction.
    /// </summary>
    /// <remarks>
    /// This class serves as an adapter between the domain layer's transaction interface
    /// and Entity Framework Core's transaction implementation, providing:
    /// <list type="bullet">
    /// <item><description>Synchronous and asynchronous commit/rollback operations</description></item>
    /// <item><description>Consistent transaction handling abstraction</description></item>
    /// <item><description>Cancellation support for async operations</description></item>
    /// </list>
    /// </remarks>
    /// <param name="transaction">The underlying Entity Framework Core transaction to wrap</param>
    public class DatabaseTransaction(IDbContextTransaction transaction) : IDatabaseTransaction
    {
        /// <summary>
        /// Commits all changes made in the transaction synchronously.
        /// </summary>
        public void Commit()
            => transaction.Commit();

        /// <summary>
        /// Asynchronously commits all changes made in the transaction.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous commit operation.</returns>
        public Task CommitAsync(CancellationToken cancellationToken = default)
            => transaction.CommitAsync(cancellationToken);

        /// <summary>
        /// Rolls back all changes made in the transaction synchronously.
        /// </summary>
        public void Rollback()
            => transaction.Rollback();

        /// <summary>
        /// Asynchronously rolls back all changes made in the transaction.
        /// </summary>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous rollback operation.</returns>
        public Task RollbackAsync(CancellationToken cancellationToken = default)
            => transaction.RollbackAsync(cancellationToken);

        /// <summary>
        /// Releases all resources used by the current transaction.
        /// </summary>
        /// <remarks>
        /// If the transaction hasn't been committed or rolled back explicitly,
        /// it will be automatically rolled back during disposal.
        /// This method suppresses finalization for better performance.
        /// </remarks>
        public void Dispose()
        {
            transaction.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Asynchronously releases all resources used by the current transaction.
        /// </summary>
        /// <remarks>
        /// If the transaction hasn't been committed or rolled back explicitly,
        /// it will be automatically rolled back during disposal.
        /// This method suppresses finalization for better performance.
        /// </remarks>
        /// <returns>A ValueTask that represents the asynchronous dispose operation.</returns>
        public async ValueTask DisposeAsync()
        {
            await transaction.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
