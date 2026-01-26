using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for creating a refund.
    /// </summary>
    public class RefundCreateRequest
    {
        /// <summary>
        /// The ID of the Payment Intent to refund.
        /// </summary>
        [JsonPropertyName("payment_id")]
        public string PaymentId { get; set; } = string.Empty;

        /// <summary>
        /// The amount to refund, in the smallest currency unit. 
        /// If not provided, it might default to the full refundable amount of the payment.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// An optional reason for the refund.
        /// Valid values include: "duplicate", "fraudulent", "requested_by_customer", "other".
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        /// <summary>
        /// Set of key-value pairs that you can attach to the object.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// An optional unique identifier for the refund request on the merchant's side.
        /// </summary>
        [JsonPropertyName("merchant_refund_id")]
        public string? MerchantRefundId { get; set; }
        
        // Other potential fields like 'refund_type' etc.
    }
}
