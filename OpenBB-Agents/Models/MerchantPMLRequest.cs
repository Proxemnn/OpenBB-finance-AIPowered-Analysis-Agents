using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for listing merchant payment methods.
    /// </summary>
    public class MerchantPMLRequest
    {
        /// <summary>
        /// Gets or sets the client secret associated with a payment intent. 
        /// If provided, the API call will use the publishable key and this client secret.
        /// Other parameters like Country, Currency, Amount, ProfileId will be ignored.
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the country code (ISO 3166-1 alpha-2) for filtering payment methods.
        /// Used when ClientSecret is not provided.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the currency code (ISO 4217) for filtering payment methods.
        /// Used when ClientSecret is not provided.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the amount for filtering payment methods.
        /// Used when ClientSecret is not provided.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// Gets or sets the Profile ID to fetch payment methods for.
        /// If not provided and ClientSecret is also not provided, the client's default Profile ID will be used.
        /// </summary>
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; } 
    }
}
