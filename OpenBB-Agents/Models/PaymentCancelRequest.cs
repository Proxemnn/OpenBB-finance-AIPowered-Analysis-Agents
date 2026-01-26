using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for cancelling a payment.
    /// </summary>
    public class PaymentCancelRequest
    {
        /// <summary>
        /// Reason for cancelling the payment.
        /// </summary>
        [JsonPropertyName("cancellation_reason")]
        public string? CancellationReason { get; set; }

        // Add other potential fields based on Hyperswitch "Payments - Cancel" API documentation if needed.
    }
}
