using System.Threading.Tasks;
using Hyperswitch.Sdk.Models;
using System.Collections.Generic; // Required for List<string> in queryParams

namespace Hyperswitch.Sdk.Services
{
    /// <summary>
    /// Provides methods for interacting with merchant-level Hyperswitch APIs,
    /// such as listing available payment methods.
    /// </summary>
    public class MerchantService
    {
        private readonly HyperswitchClient _client;
        // Corrected endpoint based on user-provided cURL.
        private const string AccountPaymentMethodsUrl = "/account/payment_methods"; 

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantService"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HyperswitchClient"/> to use for API calls.</param>
        public MerchantService(HyperswitchClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Lists the payment method types available and configured for the merchant account.
        /// </summary>
        /// <param name="request">(Optional) Parameters to filter the list, e.g., by country, currency, amount. 
        /// These are sent as query parameters if the API supports them.</param>
        /// <returns>A list of available payment method type information.</returns>
        public async Task<MerchantPMLResponse?> ListAvailablePaymentMethodsAsync(MerchantPMLRequest? request = null)
        {
            string requestUrl = AccountPaymentMethodsUrl;
            var queryParams = new List<string>();

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.ClientSecret))
                {
                    // If ClientSecret is provided, use it as the sole query parameter.
                    queryParams.Add($"client_secret={System.Uri.EscapeDataString(request.ClientSecret)}");
                }
                else // No ClientSecret, build query for secret key usage
                {
                    if (!string.IsNullOrWhiteSpace(request.Country))
                        queryParams.Add($"country={System.Uri.EscapeDataString(request.Country)}");
                    if (!string.IsNullOrWhiteSpace(request.Currency))
                        queryParams.Add($"currency={System.Uri.EscapeDataString(request.Currency)}");
                    if (request.Amount.HasValue)
                        queryParams.Add($"amount={request.Amount.Value}");
                    
                    string? profileIdToUse = request.ProfileId ?? _client.DefaultProfileId;
                    if (!string.IsNullOrWhiteSpace(profileIdToUse))
                        queryParams.Add($"profile_id={System.Uri.EscapeDataString(profileIdToUse)}");
                }
                
                if (queryParams.Count > 0)
                {
                    requestUrl += "?" + string.Join("&", queryParams);
                }
            }
            else // No request object, but might still use default profile ID for secret key call
            {
                 if (!string.IsNullOrWhiteSpace(_client.DefaultProfileId))
                 {
                    requestUrl += $"?profile_id={System.Uri.EscapeDataString(_client.DefaultProfileId)}";
                 }
            }
            
            if (request != null && !string.IsNullOrWhiteSpace(request.ClientSecret))
            {
                System.Console.WriteLine($"[MerchantService] DEBUG: Using Publishable Key for PML call (due to ClientSecret presence). URL: {requestUrl}");
                return await _client.GetAsync<MerchantPMLResponse>(requestUrl, keyType: ApiKeyType.Publishable);
            }
            else
            {
                System.Console.WriteLine($"[MerchantService] DEBUG: Using Secret Key for PML call. URL: {requestUrl}");
                return await _client.GetAsync<MerchantPMLResponse>(requestUrl, keyType: ApiKeyType.Secret);
            }
        }
    }
}
