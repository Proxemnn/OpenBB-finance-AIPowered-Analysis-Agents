using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the top-level structure of an error response from the Hyperswitch API.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the details of the error.
        /// </summary>
        [JsonPropertyName("error")]
        public ErrorDetails? Error { get; set; }
    }

    /// <summary>
    /// Provides detailed information about an API error.
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// Gets or sets the error code provided by the API.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        // Depending on Hyperswitch API, there might be other fields like 'param', 'type', etc.
        // These can be added later if needed.
    }
}
