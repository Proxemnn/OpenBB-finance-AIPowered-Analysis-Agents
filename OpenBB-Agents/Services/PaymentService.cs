using System.Threading.Tasks;
using Hyperswitch.Sdk.Models;

namespace Hyperswitch.Sdk.Services
{
    /// <summary>
    /// Provides methods for interacting with the Hyperswitch Payment Intent API.
    /// </summary>
    public class PaymentService
    {
        private readonly HyperswitchClient _client;
        private const string PaymentsUrl = "/payments"; // Base path for payments API

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentService"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HyperswitchClient"/> to use for API calls.</param>
        public PaymentService(HyperswitchClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a new payment intent.
        /// </summary>
        /// <param name="request">The payment intent creation request.</param>
        /// <returns>The created payment intent.</returns>
        public async Task<PaymentIntentResponse?> CreateAsync(PaymentIntentRequest request)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            // Ensure ProfileId is set if not provided in request and a default is available
            if (string.IsNullOrWhiteSpace(request.ProfileId) && !string.IsNullOrWhiteSpace(_client.DefaultProfileId))
            {
                request.ProfileId = _client.DefaultProfileId;
                System.Console.WriteLine($"[PaymentService DEBUG] Using default ProfileId: {request.ProfileId} for Payment Intent creation.");
            }
            return await _client.PostAsync<PaymentIntentRequest, PaymentIntentResponse>(PaymentsUrl, request);
        }

        /// <summary>
        /// Retrieves an existing payment intent.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to retrieve.</param>
        /// <returns>The retrieved payment intent.</returns>
        public async Task<PaymentIntentResponse?> RetrieveAsync(string paymentId)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));

            string requestUrl = $"{PaymentsUrl}/{paymentId}";
            return await _client.GetAsync<PaymentIntentResponse>(requestUrl);
        }

        /// <summary>
        /// Synchronizes and retrieves the latest status of an existing payment intent.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to sync.</param>
        /// <param name="clientSecret">(Optional) The client secret of the payment intent. Only use if necessary for specific flows.</param>
        /// <param name="forceSync">(Optional) If true, forces a sync with the payment processor. Defaults to false.</param>
        /// <returns>The latest payment intent details.</returns>
        public async Task<PaymentIntentResponse?> SyncPaymentStatusAsync(string paymentId, string? clientSecret = null, bool forceSync = false)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));

            string requestUrl = $"{PaymentsUrl}/{paymentId}";
            var queryParameters = new System.Collections.Generic.List<string>();

            // client_secret is generally not needed for server-side sync if API key is used.
            // if (!string.IsNullOrWhiteSpace(clientSecret))
            // {
            //     queryParameters.Add($"client_secret={System.Uri.EscapeDataString(clientSecret)}");
            // }
            if (forceSync)
            {
                queryParameters.Add($"force_sync={forceSync.ToString().ToLowerInvariant()}"); 
            }

            if (queryParameters.Count > 0)
            {
                requestUrl += "?" + string.Join("&", queryParameters);
            }
                    
            return await _client.GetAsync<PaymentIntentResponse>(requestUrl);
        }

        /// <summary>
        /// Captures a previously authorized payment.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to capture.</param>
        /// <param name="captureRequest">(Optional) Details for the capture, like amount_to_capture.</param>
        /// <returns>The updated payment intent after capture.</returns>
        public async Task<PaymentIntentResponse?> CapturePaymentAsync(string paymentId, PaymentCaptureRequest? captureRequest = null)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));

            string requestUrl = $"{PaymentsUrl}/{paymentId}/capture";
            return await _client.PostAsync<PaymentCaptureRequest?, PaymentIntentResponse>(requestUrl, captureRequest);
        }

        /// <summary>
        /// Confirms a previously created payment intent.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to confirm.</param>
        /// <param name="confirmRequest">(Optional) Details for the confirmation.</param>
        /// <returns>The updated payment intent after confirmation.</returns>
        public async Task<PaymentIntentResponse?> ConfirmPaymentAsync(string paymentId, PaymentConfirmRequest? confirmRequest = null)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));

            string requestUrl = $"{PaymentsUrl}/{paymentId}/confirm";
            return await _client.PostAsync<PaymentConfirmRequest?, PaymentIntentResponse>(requestUrl, confirmRequest);
        }

        /// <summary>
        /// Cancels (voids) a payment that has been authorized but not yet captured, or is in a pre-authorization state.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to cancel.</param>
        /// <param name="cancelRequest">(Optional) Details for the cancellation, like a reason.</param>
        /// <returns>The updated payment intent after cancellation.</returns>
        public async Task<PaymentIntentResponse?> CancelPaymentAsync(string paymentId, PaymentCancelRequest? cancelRequest = null)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));

            string requestUrl = $"{PaymentsUrl}/{paymentId}/cancel";
            return await _client.PostAsync<PaymentCancelRequest?, PaymentIntentResponse>(requestUrl, cancelRequest);
        }

        /// <summary>
        /// Updates an existing payment intent.
        /// </summary>
        /// <param name="paymentId">The ID of the payment intent to update.</param>
        /// <param name="updateRequest">The details to update on the payment intent.</param>
        /// <returns>The updated payment intent.</returns>
        public async Task<PaymentIntentResponse?> UpdatePaymentAsync(string paymentId, PaymentUpdateRequest updateRequest)
        {
            if (string.IsNullOrWhiteSpace(paymentId))
                throw new System.ArgumentNullException(nameof(paymentId));
            if (updateRequest == null)
                throw new System.ArgumentNullException(nameof(updateRequest));

            string requestUrl = $"{PaymentsUrl}/{paymentId}";
            return await _client.PostAsync<PaymentUpdateRequest, PaymentIntentResponse>(requestUrl, updateRequest);
        }
    }
}
