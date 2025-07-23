using AirSoft.EntityFrameworkCore.Abstractions.Entities;
using AirSoft.Exceptions;
using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Base
{
    /// <summary>
    /// An abstract base class for building queries or commands with common functionality.
    /// Provides shared functionality like timeout configuration and constraint management.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the derived builder (for fluent chaining).</typeparam>
    /// <typeparam name="TEntity">The type of the entity being built for.</typeparam>
    public abstract class BaseEntityBuilder<TBuilder, TEntity>
        where TBuilder : BaseEntityBuilder<TBuilder, TEntity>
        where TEntity : EntityBase
    {
        /// <summary>
        /// A flag indicating whether to enable builder constraints.
        /// This should be used with caution as it bypasses safety checks.
        /// </summary>
        private bool _enabledConstraints = true;

        /// <summary>
        /// Gets a value indicating whether builder constraints are currently enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if builder constraints are enabled; otherwise, <c>false</c>.
        /// </value>
        protected bool IsEnabledConstraints => _enabledConstraints;

        /// <summary>
        /// Gets or sets the timeout duration for the operation execution.
        /// This property is internal and not intended for direct use outside the builder.
        /// </summary>
        /// <value>
        /// The timeout duration for the operation. Default is <see cref="TimeSpan.Zero"/> (no timeout).
        /// </value>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan Timeout { get; private set; } = TimeSpan.Zero;

        /// <summary>
        /// Disables builder constraints.
        /// This method should be used with caution as it bypasses safety checks.
        /// </summary>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <remarks>
        /// Marked as obsolete to warn against casual usage. Only use when absolutely necessary.
        /// </remarks>
        [Obsolete("Do not use disabling builder restrictions unless it is done intentionally")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TBuilder WithDisableConstraints()
        {
            _enabledConstraints = false;
            return (TBuilder)this;
        }

        /// <summary>
        /// Sets the timeout duration for the operation execution.
        /// </summary>
        /// <param name="timeout">The timeout duration.</param>
        /// <returns>The current builder instance for method chaining.</returns>
        /// <exception cref="InvalidArgumentException">
        /// Thrown when:
        /// <list type="bullet">
        /// <item><description>Timeout is <see cref="TimeSpan.Zero"/></description></item>
        /// <item><description>Timeout is less than 5 seconds (unless constraints are ignored)</description></item>
        /// <item><description>Timeout is more than 30 seconds (unless constraints are ignored)</description></item>
        /// </list>
        /// </exception>
        /// <remarks>
        /// The default constraints require timeouts between 5 seconds and 30 seconds.
        /// Use <see cref="WithDisableConstraints"/> to bypass these constraints when necessary.
        /// WARNING: This timeout ONLY affects async operations.
        /// Synchronous methods will ignore this setting.
        /// </remarks>
        /// <exception cref="InvalidArgumentException">Thrown when timeout is zero, less than 5s or more than 30s (unless constraints ignored)</exception>
        public TBuilder WithTimeout(TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} with {nameof(TimeSpan.Zero)} is not allowed");

            if (timeout < TimeSpan.FromSeconds(5) && IsEnabledConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} using timeout less than 5 seconds is not allowed");

            if (timeout >= TimeSpan.FromSeconds(30) && IsEnabledConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} using timeout more than 30 seconds is not allowed");

            Timeout = timeout;
            return (TBuilder)this;
        }
    }
}
