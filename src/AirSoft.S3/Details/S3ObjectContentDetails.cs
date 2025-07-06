namespace AirSoft.S3.Details
{
    /// <summary>
    /// Represents detailed content information for an S3 object including its data stream and implements <see cref="IDisposable"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Used primarily for download operations where both the object's metadata and content are required.
    /// The class handles proper resource cleanup through the <see cref="IDisposable"/> pattern.
    /// </para>
    /// <para>
    /// Always dispose instances of this class either by calling <see cref="Dispose()"/> directly
    /// or by using a <c>using</c> statement to ensure timely release of the underlying stream.
    /// </para>
    /// </remarks>
    public class S3ObjectContentDetails : IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Gets or sets the object key representing the full path within the S3 bucket.
        /// </summary>
        public string Key { get; set; } = null!;

        /// <summary>
        /// Gets or sets the MIME type of the object content.
        /// </summary>
        /// <value>
        /// A string representing the MIME type (e.g., "application/pdf", "image/jpeg").
        /// </value>
        public string ContentType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the size of the object content in bytes.
        /// </summary>
        /// <value>
        /// A long integer representing the exact byte count of the content.
        /// </value>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the stream containing the object's data.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This read-only stream provides direct access to the object's content.
        /// The stream is automatically disposed when the parent <see cref="S3ObjectContentDetails"/>
        /// instance is disposed.
        /// </para>
        /// <para>
        /// For optimal performance with large files:
        /// <list type="bullet">
        /// <item>Use buffered reading (e.g., through <see cref="BufferedStream"/>)</item>
        /// <item>Process the stream sequentially rather than loading entirely into memory</item>
        /// </list>
        /// </para>
        /// </remarks>
        public Stream Content { get; set; } = null!;

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                Content?.Dispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// This method calls the protected <see cref="Dispose(bool)"/> method with <c>true</c> parameter
        /// and suppresses finalization of the instance.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
