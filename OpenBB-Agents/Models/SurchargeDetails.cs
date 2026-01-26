using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents information about a surcharge.
    /// </summary>
    public class SurchargeInfo
    {
        /// <summary>
        /// Gets or sets the type of surcharge (e.g., "fixed", "percentage").
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; } 

        /// <summary>
        /// Gets or sets the value of the surcharge (amount or percentage value).
        /// </summary>
        [JsonPropertyName("value")]
        public long? Value { get; set; } 
    }

    /// <summary>
    /// Represents information about tax on a surcharge.
    /// </summary>
    public class TaxOnSurchargeInfo
    {
        /// <summary>
        /// Gets or sets the tax percentage on the surcharge.
        /// </summary>
        [JsonPropertyName("percentage")]
        public long? Percentage { get; set; } 
    }

    /// <summary>
    /// Container for surcharge and tax details.
    /// </summary>
    public class SurchargeDetailsContainer
    {
        /// <summary>
        /// Gets or sets the surcharge information.
        /// </summary>
        [JsonPropertyName("surcharge")]
        public SurchargeInfo? Surcharge { get; set; }

        /// <summary>
        /// Gets or sets the tax on surcharge information.
        /// </summary>
        [JsonPropertyName("tax_on_surcharge")]
        public TaxOnSurchargeInfo? TaxOnSurcharge { get; set; }

        /// <summary>
        /// Gets or sets the display amount for the surcharge.
        /// </summary>
        [JsonPropertyName("display_surcharge_amount")]
        public long? DisplaySurchargeAmount { get; set; }

        /// <summary>
        /// Gets or sets the display amount for the tax on the surcharge.
        /// </summary>
        [JsonPropertyName("display_tax_on_surcharge_amount")]
        public long? DisplayTaxOnSurchargeAmount { get; set; }

        /// <summary>
        /// Gets or sets the total display amount for the surcharge including tax.
        /// </summary>
        [JsonPropertyName("display_total_surcharge_amount")]
        public long? DisplayTotalSurchargeAmount { get; set; }
    }
}
