using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for updating a refund.
    /// </summary>
    public class RefundUpdateRequest
    {
        /// <summary>
        /// Set of key-value pairs that you can attach to the object. 
        /// This can be useful for storing additional information about the object in a structured format.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// An optional reason for the refund. Can be updated if the refund is in a state that allows it.
        /// Valid values include: "duplicate", "fraudulent", "requested_by_customer", "other".
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        // Other potential updatable fields based on full API documentation.
    }
}
