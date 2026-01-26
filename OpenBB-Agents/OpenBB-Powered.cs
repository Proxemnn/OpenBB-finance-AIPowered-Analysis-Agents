using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Hyperswitch.Sdk.Exceptions;
using Hyperswitch.Sdk.Models;

namespace Hyperswitch.Sdk
{
    /// <summary>
    /// Specifies the type of API key to use for an operation.
    /// </summary>
    public enum ApiKeyType 
    {
        /// <summary>
        /// The secret API key, used for most server-to-server operations.
        /// </summary>
        Secret,
        /// <summary>
        /// The publishable API key, used for client-side or specific server-side operations that emulate client behavior.
        /// </summary>
        Publishable
    }

    /// <summary>
    /// The main client for interacting with the Hyperswitch API.
    /// </summary>
    public class HyperswitchClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _secretKey;
        private readonly string _publishableKey; // Made mandatory
        private static readonly string DefaultBaseUrl = "https://sandbox.hyperswitch.io";

        /// <summary>
        /// Gets the default Profile ID configured for this client instance.
        /// This Profile ID will be used for API calls if not overridden in the specific request.
        /// </summary>
        internal string? DefaultProfileId { get; private set; }

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperswitchClient"/> class.
        /// </summary>
        /// <param name="secretKey">Your Hyperswitch Secret API key.</param>
        /// <param name="publishableKey">Your Hyperswitch Publishable API key.</param>
        /// <param name="defaultProfileId">An optional default Profile ID to use for requests if not specified otherwise.</param>
        /// <param name="baseUrl">The base URL for the Hyperswitch API. Defaults to the sandbox environment.</param>
        /// <exception cref="ArgumentNullException">Thrown if secretKey or publishableKey is null or empty.</exception>
        public HyperswitchClient(string secretKey, string publishableKey, string? defaultProfileId = null, string? baseUrl = null)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret API key cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(publishableKey)) // Ensured mandatory
                throw new ArgumentNullException(nameof(publishableKey), "Publishable API key cannot be null or empty.");

            _secretKey = secretKey;
            _publishableKey = publishableKey;
            DefaultProfileId = defaultProfileId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl ?? DefaultBaseUrl)
            };
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private string GetApiKey(ApiKeyType keyType)
        {
            if (keyType == ApiKeyType.Publishable)
            {
                // Constructor now guarantees _publishableKey is not null or whitespace.
                return _publishableKey;
            }
            return _secretKey; // Default to secret key
        }

        internal async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest payload, ApiKeyType keyType = ApiKeyType.Secret)
        {
            var jsonPayload = JsonSerializer.Serialize(payload, JsonSerializerOptions);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Add("api-key", GetApiKey(keyType));
            requestMessage.Content = content;

            System.Console.WriteLine($"[HyperswitchClient DEBUG] Request: {requestMessage.Method} {requestMessage.RequestUri}");
            foreach (var header in requestMessage.Headers)
            {
                System.Console.WriteLine($"[HyperswitchClient DEBUG] Header: {header.Key}: {string.Join(", ", header.Value)}");
            }
            System.Console.WriteLine($"[HyperswitchClient DEBUG] Payload: {jsonPayload}");

            var response = await _httpClient.SendAsync(requestMessage);
            return await HandleResponse<TResponse>(response);
        }

        internal async Task<TResponse> GetAsync<TResponse>(string url, ApiKeyType keyType = ApiKeyType.Secret)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            requestMessage.Headers.Add("api-key", GetApiKey(keyType));
            
            System.Console.WriteLine($"[HyperswitchClient DEBUG] Request: {requestMessage.Method} {requestMessage.RequestUri}");
            foreach (var header in requestMessage.Headers)
            {
                System.Console.WriteLine($"[HyperswitchClient DEBUG] Header: {header.Key}: {string.Join(", ", header.Value)}");
            }
            
            var response = await _httpClient.SendAsync(requestMessage);
            return await HandleResponse<TResponse>(response);
        }

        internal async Task<TResponse?> DeleteAsync<TResponse>(string url, ApiKeyType keyType = ApiKeyType.Secret) where TResponse : class 
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, url);
            requestMessage.Headers.Add("api-key", GetApiKey(keyType));

            System.Console.WriteLine($"[HyperswitchClient DEBUG] Request: {requestMessage.Method} {requestMessage.RequestUri}");
            foreach (var header in requestMessage.Headers)
            {
                System.Console.WriteLine($"[HyperswitchClient DEBUG] Header: {header.Key}: {string.Join(", ", header.Value)}");
            }

            var response = await _httpClient.SendAsync(requestMessage);
            return await HandleResponse<TResponse>(response); 
        }

        private async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    return default(TResponse)!;
                }
                try
                {
                    return JsonSerializer.Deserialize<TResponse>(responseContent, JsonSerializerOptions)!;
                }
                catch (JsonException ex)
                {
                    throw new HyperswitchApiException($"Failed to deserialize response: {ex.Message}. Response content: {responseContent}", (int)response.StatusCode, responseContent, ex);
                }
            }

            ErrorResponse? errorResponse = null;
            try
            {
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, JsonSerializerOptions);
                }
            }
            catch (JsonException)
            {
                // Ignore
            }

            throw new HyperswitchApiException(
                errorResponse?.Error?.Message ?? $"API request failed with status code {response.StatusCode}.",
                (int)response.StatusCode,
                responseContent,
                errorResponse);
        }

        /// <summary>
        /// Releases the unmanaged resources and disposes of the managed resources used by the <see cref="HttpClient"/>.
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
