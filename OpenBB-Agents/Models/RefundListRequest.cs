using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents a filter for a range of amounts.
    /// </summary>
    public class AmountFilter 
    {
        /// <summary>
        /// Gets or sets the starting amount for the filter range.
        /// </summary>
        [JsonPropertyName("start_amount")]
        public long? StartAmount { get; set; }

        /// <summary>
        /// Gets or sets the ending amount for the filter range.
        /// </summary>
        [JsonPropertyName("end_amount")]
        public long? EndAmount { get; set; }
    }

    /// <summary>
    /// Represents the request parameters for listing refunds.
    /// </summary>
    public class RefundListRequest
    {
        /// <summary>
        /// Gets or sets the Payment ID to filter refunds by.
        /// </summary>
        [JsonPropertyName("payment_id")]
        public string? PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the Refund ID to filter by.
        /// </summary>
        [JsonPropertyName("refund_id")]
        public string? RefundId { get; set; } 

        /// <summary>
        /// Gets or sets the Profile ID to filter refunds by. 
        /// If not provided, the client's default Profile ID will be used.
        /// </summary>
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets a limit on the number of objects to be returned.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// Gets or sets an offset for pagination.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; } 

        /// <summary>
        /// Gets or sets the start time for filtering refunds (ISO 8601 UTC string).
        /// </summary>
        [JsonPropertyName("start_time")] 
        public string? StartTime { get; set; } 

        /// <summary>
        /// Gets or sets the end time for filtering refunds (ISO 8601 UTC string).
        /// </summary>
        [JsonPropertyName("end_time")]
        public string? EndTime { get; set; } 

        /// <summary>
        /// Gets or sets the amount filter for listing refunds.
        /// </summary>
        [JsonPropertyName("amount_filter")]
        public AmountFilter? AmountFilter { get; set; }

        /// <summary>
        /// Gets or sets a list of connector names to filter refunds by.
        /// </summary>
        [JsonPropertyName("connector")]
        public List<string>? Connector { get; set; }

        /// <summary>
        /// Gets or sets a list of merchant connector IDs to filter refunds by.
        /// </summary>
        [JsonPropertyName("merchant_connector_id")]
        public List<string>? MerchantConnectorId { get; set; }

        /// <summary>
        /// Gets or sets a list of currency codes to filter refunds by.
        /// </summary>
        [JsonPropertyName("currency")]
        public List<string>? Currency { get; set; } 

        /// <summary>
        /// Gets or sets a list of refund statuses to filter by.
        /// </summary>
        [JsonPropertyName("refund_status")]
        public List<string>? RefundStatus { get; set; } 
    }
}
