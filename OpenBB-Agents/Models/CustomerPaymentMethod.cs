using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents a customer's saved payment method.
    /// </summary>
    public class CustomerPaymentMethod
    {
        /// <summary>
        /// Gets or sets the unique token for the payment method.
        /// </summary>
        [JsonPropertyName("payment_token")]
        public string? PaymentToken { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer this payment method belongs to.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment method (e.g., "card").
        /// </summary>
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the specific type of the payment method (e.g., "credit", "debit").
        /// </summary>
        [JsonPropertyName("payment_method_type")]
        public string? PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the payment method (e.g., "Visa", "Mastercard").
        /// </summary>
        [JsonPropertyName("payment_method_issuer")]
        public string? PaymentMethodIssuer { get; set; }

        /// <summary>
        /// Gets or sets the code for the payment method issuer.
        /// </summary>
        [JsonPropertyName("payment_method_issuer_code")]
        public string? PaymentMethodIssuerCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether recurring payments are enabled for this payment method.
        /// </summary>
        [JsonPropertyName("recurring_enabled")]
        public bool RecurringEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether installment payments are enabled for this payment method.
        /// </summary>
        [JsonPropertyName("installment_payment_enabled")]
        public bool InstallmentPaymentEnabled { get; set; }

        /// <summary>
        /// Gets or sets the list of payment experiences supported by this payment method.
        /// </summary>
        [JsonPropertyName("payment_experience")]
        public List<string>? PaymentExperience { get; set; }

        /// <summary>
        /// Gets or sets the card summary details if the payment method is a card.
        /// </summary>
        [JsonPropertyName("card")]
        public CardSummary? Card { get; set; }

        /// <summary>
        /// Gets or sets the metadata associated with the payment method.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of the payment method (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("created")]
        public string? Created { get; set; } 

        /// <summary>
        /// Gets or sets additional payment method data. The structure of this object can vary.
        /// </summary>
        [JsonPropertyName("payment_method_data")]
        public object? PaymentMethodData { get; set; } 

        /// <summary>
        /// Gets or sets the ID of the locker where the payment method is stored.
        /// </summary>
        [JsonPropertyName("locker_id")]
        public string? LockerId { get; set; } 
    }
}
