using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response from the API call to list available merchant payment method configurations.
    /// This structure is based on the JSON example provided by the user.
    /// </summary>
    public class MerchantPMLResponse
    {
        /// <summary>
        /// Gets or sets the redirect URL, if applicable for any payment method.
        /// </summary>
        [JsonPropertyName("redirect_url")]
        public string? RedirectUrl { get; set; }

        /// <summary>
        /// Gets or sets the currency for which the payment methods are listed.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the list of payment method groups available.
        /// </summary>
        [JsonPropertyName("payment_methods")]
        public List<PaymentMethodGroup>? PaymentMethods { get; set; }

        /// <summary>
        /// Gets or sets information related to mandate payments, if applicable.
        /// </summary>
        [JsonPropertyName("mandate_payment")]
        public MandatePaymentInfo? MandatePayment { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        [JsonPropertyName("merchant_name")]
        public string? MerchantName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show a surcharge breakup screen.
        /// </summary>
        [JsonPropertyName("show_surcharge_breakup_screen")]
        public bool? ShowSurchargeBreakupScreen { get; set; }

        /// <summary>
        /// Gets or sets the type of payment (e.g., "normal").
        /// </summary>
        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; } 

        /// <summary>
        /// Gets or sets a value indicating whether external 3DS authentication is requested.
        /// </summary>
        [JsonPropertyName("request_external_three_ds_authentication")]
        public bool? RequestExternalThreeDsAuthentication { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to collect shipping details from wallets.
        /// </summary>
        [JsonPropertyName("collect_shipping_details_from_wallets")]
        public bool? CollectShippingDetailsFromWallets { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to collect billing details from wallets.
        /// </summary>
        [JsonPropertyName("collect_billing_details_from_wallets")]
        public bool? CollectBillingDetailsFromWallets { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tax calculation is enabled.
        /// </summary>
        [JsonPropertyName("is_tax_calculation_enabled")]
        public bool? IsTaxCalculationEnabled { get; set; }
    }
}
