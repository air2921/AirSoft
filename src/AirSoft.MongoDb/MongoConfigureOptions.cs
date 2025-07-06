namespace AirSoft.MongoDb
{
    /// <summary>
    /// Class for configuring MongoDB settings.
    /// </summary>
    public class MongoConfigureOptions
    {
        /// <summary>
        /// Gets or sets the connection string for the MongoDB database.
        /// </summary>
        /// <value>The connection string for the MongoDB database.</value>
        public string Connection { internal get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the MongoDB database.
        /// </summary>
        /// <value>The name of the MongoDB database.</value>
        public string Database { internal get; set; } = null!;
    }
}
