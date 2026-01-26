using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents recurring payment details, typically used for subsequent mandate payments.
    /// </summary>
    public class RecurringDetailsInfo
    {
        /// <summary>
        /// Gets or sets the type of recurring data being provided.
        /// For mandate payments using a saved payment method, this would be "payment_method_id".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the actual data for the recurring payment.
        /// If type is "payment_method_id", this would be the payment method token (ID).
        /// </summary>
        [JsonPropertyName("data")]
        public string? Data { get; set; }
    }
}
