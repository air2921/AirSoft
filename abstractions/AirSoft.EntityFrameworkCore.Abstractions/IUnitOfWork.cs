namespace AirSoft.EntityFrameworkCore.Abstractions
{
    /// <summary>
    /// Defines the contract for a unit of work pattern that coordinates the writing of changes to one or more databases.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Asynchronously saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A <see cref="Task"/> that represents the asynchronous save operation. 
        /// The task result contains the number of state entries written to the database.
        /// </returns>
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in this unit of work to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        public int SaveChanges();
    }
}
