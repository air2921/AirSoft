using System.ComponentModel;

namespace AirSoft.EntityFrameworkCore.Abstractions.Builders.Abstractions
{
    /// <summary>
    /// An interface for building queries or commands with common functionality.
    /// Provides shared functionality like timeout configuration.
    /// </summary>
    public interface IBaseEntityBuilder
    {
        /// <summary>
        /// Gets or sets the timeout duration for the operation execution.
        /// This property is internal and not intended for direct use outside the builder.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public TimeSpan Timeout { get; }
    }
}
