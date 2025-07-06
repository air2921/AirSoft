using System.Text.Json;

namespace AirSoft.DistributedCache
{
    /// <summary>
    /// Class for configuring cache settings.
    /// </summary>
    public class ConfigureOptions
    {
        private JsonNamingPolicy _jsonNamingPolicy;
        private JsonSerializerOptions _jsonSerializerSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureOptions"/> class.
        /// </summary>
        public ConfigureOptions()
        {
            _jsonNamingPolicy = JsonNamingPolicy.CamelCase;
            _jsonSerializerSettings = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default,
                PropertyNamingPolicy = _jsonNamingPolicy,
                WriteIndented = false
            };
        }

        /// <summary>
        /// Gets or sets the JSON serializer settings.
        /// </summary>
        /// <value>The JSON serializer settings.</value>
        public JsonSerializerOptions JsonSerializerSettings
        {
            internal get => _jsonSerializerSettings;
            set => _jsonSerializerSettings = value;
        }

        /// <summary>
        /// Gets or sets the JSON naming policy.
        /// </summary>
        /// <value>The JSON naming policy.</value>
        public JsonNamingPolicy JsonNamingPolicy
        {
            internal get => _jsonNamingPolicy;
            set => _jsonNamingPolicy = value;
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string Connection { internal get; set; } = null!;
    }
}
