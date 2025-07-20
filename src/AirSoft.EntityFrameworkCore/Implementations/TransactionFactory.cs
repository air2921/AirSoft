using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Utils;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AirSoft.EntityFrameworkCore.Implementations
{
    /// <summary>
    /// A factory class to manage database transactions for a given <typeparamref name="TDbContext"/>.
    /// <para>Implements <see cref="ITransactionFactory"/> interface.</para>
    /// </summary>
    /// <typeparam name="TDbContext">The type of the database context. It must inherit from <see cref="DbContext"/>.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TransactionFactory{TDbContext}"/> class.
    /// </remarks>
    /// <param name="dbContext">The database context to be used for the transaction.</param>
    public class TransactionFactory<TDbContext>(TDbContext dbContext) : ITransactionFactory
        where TDbContext : DbContext
    {
        /// <summary>
        /// Begins a new database transaction synchronously with the default isolation level.
        /// </summary>
        /// <returns>A new <see cref="IDatabaseTransaction"/> representing the transaction.</returns>
        public IDatabaseTransaction Begin()
            => new DatabaseTransaction(dbContext.Database.BeginTransaction());

        /// <summary>
        /// Begins a new database transaction synchronously with the specified isolation level.
        /// </summary>
        /// <param name="isolation">The isolation level to use for the transaction.</param>
        /// <returns>A new <see cref="IDatabaseTransaction"/> representing the transaction.</returns>
        public IDatabaseTransaction Begin(IsolationLevel isolation)
            => new DatabaseTransaction(dbContext.Database.BeginTransaction(isolation));

        /// <summary>
        /// Begins a new database transaction asynchronously with the default isolation level.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a new <see cref="IDatabaseTransaction"/>.</returns>
        public async Task<IDatabaseTransaction> BeginAsync(CancellationToken cancellationToken = default)
            => new DatabaseTransaction(await dbContext.Database.BeginTransactionAsync(cancellationToken));

        /// <summary>
        /// Begins a new database transaction asynchronously with the specified isolation level.
        /// </summary>
        /// <param name="isolation">The isolation level to use for the transaction.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a new <see cref="IDatabaseTransaction"/>.</returns>
        public async Task<IDatabaseTransaction> BeginAsync(IsolationLevel isolation, CancellationToken cancellationToken = default)
            => new DatabaseTransaction(await dbContext.Database.BeginTransactionAsync(isolation, cancellationToken));
    }
}
