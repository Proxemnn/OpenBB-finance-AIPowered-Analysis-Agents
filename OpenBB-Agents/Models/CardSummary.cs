using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Provides a summary of card details.
    /// </summary>
    public class CardSummary
    {
        /// <summary>
        /// Gets or sets the last four digits of the card number.
        /// </summary>
        [JsonPropertyName("last4_digits")]
        public string? Last4Digits { get; set; }

        /// <summary>
        /// Gets or sets the expiry month of the card.
        /// </summary>
        [JsonPropertyName("expiry_month")]
        public string? ExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the expiry year of the card.
        /// </summary>
        [JsonPropertyName("expiry_year")]
        public string? ExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets the issuer country code of the card.
        /// </summary>
        [JsonPropertyName("issuer_country_code")]
        public string? IssuerCountryCode { get; set; } 

        /// <summary>
        /// Gets or sets the tokenization status of the card.
        /// </summary>
        [JsonPropertyName("tokenization_status")]
        public string? TokenizationStatus { get; set; } 
    }
}
