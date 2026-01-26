using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response for listing a customer's payment methods.
    /// </summary>
    public class CustomerPaymentMethodListResponse
    {
        /// <summary>
        /// Gets or sets the ID of the customer.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the list of customer payment methods.
        /// </summary>
        [JsonPropertyName("data")]
        public List<CustomerPaymentMethod>? Data { get; set; }

        /// <summary>
        /// Gets or sets the count of payment methods returned.
        /// </summary>
        [JsonPropertyName("payment_method_count")]
        public int PaymentMethodCount { get; set; }

        // Add other potential fields if the API supports more comprehensive pagination
        // e.g., "object": "list", "has_more": true, "url": "..."
    }
}
