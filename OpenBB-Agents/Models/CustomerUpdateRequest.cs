using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for updating an existing customer.
    /// </summary>
    public class CustomerUpdateRequest
    {
        // Note: CustomerId is typically part of the URL for update, not the request body.

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
        /// Gets or sets an arbitrary string that you can attach to a customer object.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the customer's address. Provide to update.
        /// If the API supports partial address updates, this structure is fine.
        /// If the entire address object must be replaced, ensure all sub-fields are set or cleared as intended.
        /// </summary>
        [JsonPropertyName("address")]
        public AddressDetails? Address { get; set; } 

        /// <summary>
        /// Gets or sets a set of key-value pairs that you can attach to a customer object.
        /// It can be useful for storing additional information about the object in a structured format.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        // Other fields that can be updated as per API documentation.
        // For example, default_payment_method_id if supported.
    }
}
