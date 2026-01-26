using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response from a customer deletion operation.
    /// </summary>
    public class CustomerDeleteResponse
    {
        /// <summary>
        /// The ID of the customer that was deleted.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// A boolean indicating whether the customer was successfully deleted.
        /// </summary>
        [JsonPropertyName("customer_deleted")]
        public bool CustomerDeleted { get; set; }

        /// <summary>
        /// A boolean indicating whether associated payment methods were also deleted.
        /// This field might not always be present; depends on API design.
        /// </summary>
        [JsonPropertyName("payment_methods_deleted")] 
        public bool? PaymentMethodsDeleted { get; set; } // Optional, based on API behavior
    }
}
