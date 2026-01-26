using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for capturing a payment.
    /// </summary>
    public class PaymentCaptureRequest
    {
        /// <summary>
        /// The amount to capture, in the smallest currency unit (e.g., cents for USD).
        /// If not provided, the entire authorized amount of the payment intent will be captured.
        /// </summary>
        [JsonPropertyName("amount_to_capture")]
        public long? AmountToCapture { get; set; }

        // Add other potential fields if the "Payments - Capture" API supports them,
        // e.g., statement_descriptor_suffix, application_fee_amount, etc.
        // For now, keeping it simple with just amount_to_capture.
    }
}
