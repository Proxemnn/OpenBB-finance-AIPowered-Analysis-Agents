using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents mandate data for creating or using mandates.
    /// Corresponds to #/components/schemas/MandateData
    /// </summary>
    public class MandateData
    {
        /// <summary>
        /// Gets or sets customer acceptance details for the mandate.
        /// </summary>
        [JsonPropertyName("customer_acceptance")]
        public CustomerAcceptance? CustomerAcceptance { get; set; } 

        /// <summary>
        /// Gets or sets the type of mandate.
        /// </summary>
        [JsonPropertyName("mandate_type")]
        public MandateType? MandateType { get; set; } 
    }

    /// <summary>
    /// Represents customer acceptance details for a mandate.
    /// Corresponds to #/components/schemas/CustomerAcceptance
    /// </summary>
    public class CustomerAcceptance 
    { 
        /// <summary>
        /// Gets or sets the type of acceptance (e.g., "online", "offline").
        /// </summary>
        [JsonPropertyName("acceptance_type")]
        public string? AcceptanceType { get; set; } 

        /// <summary>
        /// Gets or sets the timestamp when the customer acceptance was provided (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("accepted_at")]
        public string? AcceptedAt { get; set; } 

        /// <summary>
        /// Gets or sets details for online acceptance.
        /// </summary>
        [JsonPropertyName("online")]
        public OnlineMandate? Online { get; set; }
    }

    /// <summary>
    /// Represents online mandate acceptance details.
    /// Corresponds to #/components/schemas/OnlineMandate
    /// </summary>
    public class OnlineMandate
    {
        /// <summary>
        /// Gets or sets the IP address of the customer who accepted the mandate.
        /// </summary>
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent of the browser used by the customer to accept the mandate.
        /// </summary>
        [JsonPropertyName("user_agent")]
        public string? UserAgent { get; set; }
    }

    /// <summary>
    /// Represents the type of mandate (e.g., single_use, multi_use).
    /// Corresponds to #/components/schemas/MandateType which is a oneOf
    /// </summary>
    public class MandateType 
    { 
        /// <summary>
        /// Details for a single-use mandate.
        /// </summary>
        [JsonPropertyName("single_use")]
        public MandateAmountData? SingleUse { get; set; }

        /// <summary>
        /// Details for a multi-use mandate.
        /// </summary>
        [JsonPropertyName("multi_use")]
        public MandateAmountData? MultiUse { get; set; } // OpenAPI spec indicates this can be null for multi_use
    }

    /// <summary>
    /// Represents amount data for a mandate.
    /// Corresponds to #/components/schemas/MandateAmountData
    /// </summary>
    public class MandateAmountData
    {
        /// <summary>
        /// The maximum amount to be debited for the mandate transaction.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; } // Required as per spec

        /// <summary>
        /// The currency for the mandate amount.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; } // Required as per spec, but SDK often uses nullable string for enums

        /// <summary>
        /// Specifying start date of the mandate (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("start_date")]
        public string? StartDate { get; set; } 

        /// <summary>
        /// Specifying end date of the mandate (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("end_date")]
        public string? EndDate { get; set; } 
        
        /// <summary>
        /// Additional details required by mandate.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; } // Changed to Dictionary<string, object> to match spec more closely
    }
}
