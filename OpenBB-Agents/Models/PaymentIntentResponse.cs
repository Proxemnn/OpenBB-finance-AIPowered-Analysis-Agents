using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response object for a payment intent.
    /// </summary>
    public class PaymentIntentResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the payment intent.
        /// </summary>
        [JsonPropertyName("payment_id")]
        public string? PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the merchant ID associated with this payment intent.
        /// </summary>
        [JsonPropertyName("merchant_id")]
        public string? MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the status of the payment intent (e.g., "requires_payment_method", "succeeded").
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; } 

        /// <summary>
        /// Gets or sets the amount of the payment intent, in the smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the amount received for this payment intent, in the smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount_received")]
        public long? AmountReceived { get; set; }

        /// <summary>
        /// Gets or sets the amount that can be captured from this payment intent, in the smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount_capturable")]
        public long? AmountCapturable { get; set; }
        
        /// <summary>
        /// Gets or sets the client secret for this payment intent, used for client-side actions.
        /// </summary>
        [JsonPropertyName("client_secret")]
        public string? ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the payment intent was created (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("created")]
        public string? Created { get; set; } 

        /// <summary>
        /// Gets or sets the three-letter ISO currency code.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer associated with this payment intent.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the description for the payment intent.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        
        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone {get; set; }

        /// <summary>
        /// Gets or sets metadata associated with the payment intent.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets the next action required to complete the payment (e.g., redirection for 3DS).
        /// </summary>
        [JsonPropertyName("next_action")]
        public NextAction? NextAction { get; set; }

        /// <summary>
        /// Gets or sets the ID of the payment method used for this payment intent.
        /// </summary>
        [JsonPropertyName("payment_method_id")]
        public string? PaymentMethodId { get; set; }
        
        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        [JsonPropertyName("shipping")]
        public Address? Shipping { get; set; } 

        /// <summary>
        /// Gets or sets the billing address for the order.
        /// </summary>
        [JsonPropertyName("billing")]
        public Address? Billing { get; set; } 

        /// <summary>
        /// Gets or sets the return URL specified for this payment intent.
        /// </summary>
        [JsonPropertyName("return_url")]
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the capture method for this payment intent (e.g., "automatic", "manual").
        /// </summary>
        [JsonPropertyName("capture_method")]
        public string? CaptureMethod { get; set; }

        /// <summary>
        /// Gets or sets the reason for cancellation, if the payment intent was cancelled.
        /// </summary>
        [JsonPropertyName("cancellation_reason")]
        public string? CancellationReason { get; set; }

        /// <summary>
        /// Gets or sets details of the last payment error, if any.
        /// </summary>
        [JsonPropertyName("last_payment_error")]
        public PaymentError? LastPaymentError { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the payment intent expires (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("expires_on")]
        public string? ExpiresOn { get; set; } 

        /// <summary>
        /// Gets or sets the timestamp of the last update to the payment intent (ISO 8601 format).
        /// </summary>
        [JsonPropertyName("updated")]
        public string? Updated { get; set; } 
    }

    /// <summary>
    /// Describes the next action required to process a payment intent.
    /// </summary>
    public class NextAction
    {
        /// <summary>
        /// Gets or sets the type of the next action (e.g., "redirect_to_url").
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; } 

        /// <summary>
        /// Gets or sets the URL to redirect the customer to, if the next action type is "redirect_to_url".
        /// </summary>
        [JsonPropertyName("redirect_to_url")]
        public string? RedirectToUrl { get; set; } 
    }

    /// <summary>
    /// Represents an error that occurred during payment processing.
    /// </summary>
    public class PaymentError
    {
        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or sets the human-readable error message.
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the decline code from the payment processor, if applicable.
        /// </summary>
        [JsonPropertyName("decline_code")]
        public string? DeclineCode { get; set; }

        /// <summary>
        /// Gets or sets the parameter related to the error, if applicable.
        /// </summary>
        [JsonPropertyName("param")]
        public string? Param { get; set; }

        /// <summary>
        /// Gets or sets the ID of the payment method associated with the error, if applicable.
        /// </summary>
        [JsonPropertyName("payment_method_id")]
        public string? PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the type of error (e.g., "card_error", "invalid_request_error").
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; } 
    }
}
