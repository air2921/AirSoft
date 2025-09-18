using AirSoft.EntityFrameworkCore.Abstractions;
using AirSoft.EntityFrameworkCore.Utils;
using AirSoft.Exceptions;
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
        /// <inheritdoc/>
        public IDatabaseTransaction Begin()
            => new DatabaseTransaction(dbContext.Database.BeginTransaction());

        /// <inheritdoc/>
        public IDatabaseTransaction Begin(IsolationLevel isolation)
            => new DatabaseTransaction(dbContext.Database.BeginTransaction(isolation));

        /// <inheritdoc/>
        public async Task<IDatabaseTransaction> BeginAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return new DatabaseTransaction(await dbContext.Database.BeginTransactionAsync(cancellationToken));
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to begin transaction", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<IDatabaseTransaction> BeginAsync(IsolationLevel isolation, CancellationToken cancellationToken = default)
        {
            try
            {
                return new DatabaseTransaction(await dbContext.Database.BeginTransactionAsync(isolation, cancellationToken));
            }
            catch (Exception ex)
            {
                throw new EntityException("Unable to begin transaction", ex);
            }
        }
    }
}
