using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response object for a customer.
    /// </summary>
    public class CustomerResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the customer.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer's email address.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the customer's full name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the customer's phone number.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the country code for the customer's phone number.
        /// </summary>
        [JsonPropertyName("phone_country_code")]
        public string? PhoneCountryCode { get; set; }

        /// <summary>
        /// Gets or sets an arbitrary string attached to the customer object.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the customer's address.
        /// </summary>
        [JsonPropertyName("address")]
        public AddressDetails? Address { get; set; } 

        /// <summary>
        /// Gets or sets a set of key-value pairs attached to the customer object.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the customer object was created (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; } 

        // The Hyperswitch_Customer_SDK README example did not show default_payment_method_id,
        // but it's a common field. Add if confirmed by API docs.
        // [JsonPropertyName("default_payment_method_id")]
        // public string? DefaultPaymentMethodId { get; set; } 
    }
}
