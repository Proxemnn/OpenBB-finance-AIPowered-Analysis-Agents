using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the details for a single-use mandate.
    /// </summary>
    public class SingleUseMandate
    {
        /// <summary>
        /// Gets or sets the amount for the mandate.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency for the mandate amount.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the start date of the mandate (e.g., ISO 8601 format).
        /// </summary>
        [JsonPropertyName("start_date")]
        public string? StartDate { get; set; } 

        /// <summary>
        /// Gets or sets the end date of the mandate (e.g., ISO 8601 format).
        /// </summary>
        [JsonPropertyName("end_date")]
        public string? EndDate { get; set; }   

        /// <summary>
        /// Gets or sets metadata associated with the mandate.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }
    }

    /// <summary>
    /// Represents payment information related to a mandate.
    /// </summary>
    public class MandatePaymentInfo
    {
        /// <summary>
        /// Gets or sets the details for a single-use mandate, if applicable.
        /// </summary>
        [JsonPropertyName("single_use")]
        public SingleUseMandate? SingleUse { get; set; }
        
        // Add other mandate types like "multi_use" if they exist in the API
    }
}
