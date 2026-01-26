using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for updating a payment intent.
    /// </summary>
    public class PaymentUpdateRequest
    {
        /// <summary>
        /// The amount to be charged, in the smallest currency unit.
        /// Can be updated if the payment is in a state that allows amount modification.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// Three-letter ISO currency code.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// An arbitrary string attached to the object. Often useful for displaying to users.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Set of key-value pairs that you can attach to an object. 
        /// This can be useful for storing additional information about the object in a structured format.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Email address of the customer.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Phone number of the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        
        /// <summary>
        /// Gets or sets the country code for the customer's phone number.
        /// </summary>
        [JsonPropertyName("phone_country_code")]
        public string? PhoneCountryCode { get; set; }
        
        /// <summary>
        /// Shipping details for the payment.
        /// </summary>
        [JsonPropertyName("shipping")]
        public Address? Shipping { get; set; }

        /// <summary>
        /// Billing details for the payment.
        /// </summary>
        [JsonPropertyName("billing")]
        public Address? Billing { get; set; }

        /// <summary>
        /// The ID of an existing Customer to associate with this payment.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Payment method data. Can be used to update or add payment method details.
        /// Required if updating payment details on a payment in requires_confirmation state.
        /// </summary>
        [JsonPropertyName("payment_method_data")]
        public PaymentMethodData? PaymentMethodData { get; set; }

        /// <summary>
        /// The payment method to be used for this payment.
        /// Required if updating payment details on a payment in requires_confirmation state.
        /// </summary>
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// The type of payment method.
        /// Required if updating payment details on a payment in requires_confirmation state.
        /// </summary>
        [JsonPropertyName("payment_method_type")]
        public string? PaymentMethodType { get; set; }

        // Add other updatable fields as per Hyperswitch "Payments - Update" API documentation
        // e.g., return_url, capture_method, authentication_type etc.
    }
}
