using System.Data;
using AirSoft.Exceptions;

namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Defines a contract for creating and managing database transactions.
    /// </summary>
    public interface ITransactionFactory
    {
        /// <summary>
        /// Begins a new database transaction with the default isolation level.
        /// </summary>
        /// <returns>The <see cref="IDatabaseTransaction"/> that represents the new transaction.</returns>
        /// <remarks>
        /// The default isolation level depends on the database provider. For most SQL databases, it's Read Committed.
        /// </remarks>
        public IDatabaseTransaction Begin();

        /// <summary>
        /// Begins a new database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolation">The isolation level to use for the transaction.</param>
        /// <returns>The <see cref="IDatabaseTransaction"/> that represents the new transaction.</returns>
        public IDatabaseTransaction Begin(IsolationLevel isolation);

        /// <summary>
        /// Asynchronously begins a new database transaction with the default isolation level.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IDatabaseTransaction"/>.</returns>
        /// <exception cref="EntityException">Thrown when operation is cancelled</exception>
        /// <remarks>
        /// The default isolation level depends on the database provider. For most SQL databases, it's Read Committed.
        /// </remarks>
        public Task<IDatabaseTransaction> BeginAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously begins a new database transaction with the specified isolation level.
        /// </summary>
        /// <param name="isolation">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="IDatabaseTransaction"/>.</returns>
        /// <exception cref="EntityException">Thrown when operation is cancelled</exception>
        public Task<IDatabaseTransaction> BeginAsync(IsolationLevel isolation, CancellationToken cancellationToken = default);
    }
}
