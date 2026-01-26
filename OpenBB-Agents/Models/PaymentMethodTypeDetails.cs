using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Describes a payment experience type and its eligible connectors.
    /// </summary>
    public class PaymentExperience
    {
        /// <summary>
        /// Gets or sets the type of payment experience (e.g., "redirect_to_url").
        /// </summary>
        [JsonPropertyName("payment_experience_type")]
        public string? PaymentExperienceType { get; set; } 

        /// <summary>
        /// Gets or sets the list of eligible connectors for this payment experience.
        /// </summary>
        [JsonPropertyName("eligible_connectors")]
        public List<string>? EligibleConnectors { get; set; }
    }

    /// <summary>
    /// Provides information about a card network, including surcharge details and eligible connectors.
    /// </summary>
    public class CardNetworkInfo
    {
        /// <summary>
        /// Gets or sets the name of the card network (e.g., "Visa").
        /// </summary>
        [JsonPropertyName("card_network")]
        public string? CardNetwork { get; set; } 

        /// <summary>
        /// Gets or sets surcharge details applicable to this card network.
        /// </summary>
        [JsonPropertyName("surcharge_details")]
        public SurchargeDetailsContainer? SurchargeDetails { get; set; }

        /// <summary>
        /// Gets or sets the list of eligible connectors for this card network.
        /// </summary>
        [JsonPropertyName("eligible_connectors")]
        public List<string>? EligibleConnectors { get; set; }
    }

    /// <summary>
    /// Provides information about bank names and their eligible connectors.
    /// </summary>
    public class BankNameInfo
    {
        /// <summary>
        /// Gets or sets the list of bank names.
        /// </summary>
        [JsonPropertyName("bank_name")]
        public List<string>? BankName { get; set; } 

        /// <summary>
        /// Gets or sets the list of eligible connectors for these banks.
        /// </summary>
        [JsonPropertyName("eligible_connectors")]
        public List<string>? EligibleConnectors { get; set; }
    }

    /// <summary>
    /// Provides information about eligible connectors for bank debits.
    /// </summary>
    public class BankDebitInfo
    {
        /// <summary>
        /// Gets or sets the list of eligible connectors for bank debits.
        /// </summary>
        [JsonPropertyName("eligible_connectors")]
        public List<string>? EligibleConnectors { get; set; }
    }

    /// <summary>
    /// Provides information about eligible connectors for bank transfers.
    /// </summary>
    public class BankTransferInfo
    {
        /// <summary>
        /// Gets or sets the list of eligible connectors for bank transfers.
        /// </summary>
        [JsonPropertyName("eligible_connectors")]
        public List<string>? EligibleConnectors { get; set; }
    }
}
