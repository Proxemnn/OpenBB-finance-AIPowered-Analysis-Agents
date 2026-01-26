using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response object for a refund.
    /// </summary>
    public class RefundResponse
    {
        /// <summary>
        /// Unique identifier for the refund.
        /// </summary>
        [JsonPropertyName("refund_id")]
        public string? RefundId { get; set; }

        /// <summary>
        /// The ID of the Payment Intent that was refunded.
        /// </summary>
        [JsonPropertyName("payment_id")]
        public string? PaymentId { get; set; }

        /// <summary>
        /// The amount refunded, in the smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Three-letter ISO currency code.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// The status of the refund. 
        /// (e.g., "succeeded", "pending", "failed", "requires_action")
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Reason for the refund, if provided.
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
        
        /// <summary>
        /// Timestamp of when the refund was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; } // Assuming ISO 8601 string

        /// <summary>
        /// Timestamp of when the refund was last updated.
        /// </summary>
        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; } // Assuming ISO 8601 string

        /// <summary>
        /// Gets or sets a set of key-value pairs that can be attached to the object.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Error code if the refund failed.
        /// </summary>
        [JsonPropertyName("error_code")]
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Error message if the refund failed.
        /// </summary>
        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }
        
        /// <summary>
        /// The merchant's unique identifier for the refund.
        /// </summary>
        [JsonPropertyName("merchant_refund_id")]
        public string? MerchantRefundId { get; set; }
        
        /// <summary>
        /// Gets or sets the connector used for the refund.
        /// </summary>
        [JsonPropertyName("connector")]
        public string? Connector { get; set; }

        /// <summary>
        /// Gets or sets the Profile ID associated with this refund.
        /// </summary>
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the Merchant Connector ID used for this refund.
        /// </summary>
        [JsonPropertyName("merchant_connector_id")]
        public string? MerchantConnectorId { get; set; }
        
        // Other potential fields like 'unified_code', 'unified_message', 'split_refunds', 'issuer_error_code', 'issuer_error_message'
        // from the list refund response example.
    }
}
