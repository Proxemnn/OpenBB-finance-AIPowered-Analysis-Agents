using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json; 

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents detailed information about a specific payment method type.
    /// </summary>
    public class DetailedPaymentMethodType
    {
        /// <summary>
        /// Gets or sets the specific payment method type (e.g., "ach", "credit").
        /// </summary>
        [JsonPropertyName("payment_method_type")]
        public string? PaymentMethodType { get; set; } 

        /// <summary>
        /// Gets or sets the list of payment experiences available for this payment method type.
        /// </summary>
        [JsonPropertyName("payment_experience")]
        public List<PaymentExperience>? PaymentExperience { get; set; }

        /// <summary>
        /// Gets or sets the list of card networks available, if applicable.
        /// </summary>
        [JsonPropertyName("card_networks")]
        public List<CardNetworkInfo>? CardNetworks { get; set; }

        /// <summary>
        /// Gets or sets the list of bank names available, if applicable.
        /// </summary>
        [JsonPropertyName("bank_names")]
        public List<BankNameInfo>? BankNames { get; set; }

        /// <summary>
        /// Gets or sets information about bank debits, if applicable.
        /// </summary>
        [JsonPropertyName("bank_debits")]
        public BankDebitInfo? BankDebits { get; set; } 

        /// <summary>
        /// Gets or sets information about bank transfers, if applicable.
        /// </summary>
        [JsonPropertyName("bank_transfers")]
        public BankTransferInfo? BankTransfers { get; set; } 

        /// <summary>
        /// Gets or sets the required fields for this payment method type. 
        /// The structure can be dynamic, represented by JsonElement.
        /// </summary>
        [JsonPropertyName("required_fields")]
        public JsonElement? RequiredFields { get; set; } 

        /// <summary>
        /// Gets or sets surcharge details for this payment method type.
        /// </summary>
        [JsonPropertyName("surcharge_details")]
        public SurchargeDetailsContainer? SurchargeDetails { get; set; }

        /// <summary>
        /// Gets or sets the payment method authorization connector.
        /// </summary>
        [JsonPropertyName("pm_auth_connector")]
        public string? PmAuthConnector { get; set; }
    }
}
