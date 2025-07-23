using AirSoft.Exceptions;
using AirSoft.MongoDb.Abstractions.Documents;
using System.ComponentModel;

namespace AirSoft.MongoDb.Abstractions.Builders.Base
{
    /// <summary>
    /// An abstract base class for building queries or commands with common functionality.
    /// Provides shared functionality like timeout configuration and constraint management.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the derived builder (for fluent chaining).</typeparam>
    /// <typeparam name="TDocument">The type of the document being built for.</typeparam>
    public abstract class BaseDocumentBuilder<TBuilder, TDocument>
        where TBuilder : BaseDocumentBuilder<TBuilder, TDocument>
        where TDocument : DocumentBase
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
        /// <exception cref="InvalidArgumentException">Thrown when the timeout is <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="InvalidArgumentException">Thrown when the timeout less than 5 seconds and constraints are enabled.</exception>
        /// <exception cref="InvalidArgumentException">Thrown when the timeout more than 3 min and constraints are enabled.</exception>
        /// <remarks>
        /// The default constraints require timeouts between 5 seconds and 3 minutes.
        /// Use <see cref="WithIgnoreBuilderConstraints"/> to bypass these constraints when necessary.
        /// </remarks>
        public TBuilder WithTimeout(TimeSpan timeout)
        {
            if (timeout == TimeSpan.Zero)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} with {nameof(TimeSpan.Zero)} is not allowed");

            if (timeout < TimeSpan.FromSeconds(5) && IsEnabledConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} using timeout less than 5 seconds is not allowed");

            if (timeout >= TimeSpan.FromMinutes(3) && IsEnabledConstraints)
                throw new InvalidArgumentException($"Using a {nameof(WithTimeout)} using timeout more than 3 minutes is not allowed");

            Timeout = timeout;
            return (TBuilder)this;
        }
    }
}
