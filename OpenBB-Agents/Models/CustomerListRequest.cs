using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for listing customers.
    /// </summary>
    public class CustomerListRequest
    {
        /// <summary>
        /// A limit on the number of objects to be returned. Limit can range between 1 and 100, and the default is 10.
        /// </summary>
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// A positive integer that specifies the number of objects to skip before starting to collect the result set.
        /// </summary>
        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// Filter customers by email.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Filter customers by phone number.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        
        // Add other potential filter parameters based on API capabilities
        // e.g., name, created_at_gte, created_at_lte
    }
}
