using System.Threading.Tasks;
using Hyperswitch.Sdk.Models;

namespace Hyperswitch.Sdk.Services
{
    /// <summary>
    /// Provides methods for interacting with the Hyperswitch Customer API.
    /// </summary>
    public class CustomerService
    {
        private readonly HyperswitchClient _client;
        private const string CustomersBaseUrl = "/customers"; // For single customer ops by ID
        private const string CustomerListUrl = "/customers/list"; // For listing customers

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService"/> class.
        /// </summary>
        /// <param name="client">The <see cref="HyperswitchClient"/> to use for API calls.</param>
        public CustomerService(HyperswitchClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="request">The customer creation request details.</param>
        /// <returns>The created customer object.</returns>
        public async Task<CustomerResponse?> CreateCustomerAsync(CustomerRequest request)
        {
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));
            // Add any other specific validations for CustomerRequest if needed.

            return await _client.PostAsync<CustomerRequest, CustomerResponse>(CustomersBaseUrl, request); // Use CustomersBaseUrl
        }

        /// <summary>
        /// Retrieves an existing customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>The retrieved customer object.</returns>
        public async Task<CustomerResponse?> RetrieveCustomerAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new System.ArgumentNullException(nameof(customerId));

            string requestUrl = $"{CustomersBaseUrl}/{customerId}";
            return await _client.GetAsync<CustomerResponse>(requestUrl);
        }

        /// <summary>
        /// Updates an existing customer's details.
        /// </summary>
        /// <param name="customerId">The ID of the customer to update.</param>
        /// <param name="request">The customer update request details.</param>
        /// <returns>The updated customer object.</returns>
        public async Task<CustomerResponse?> UpdateCustomerAsync(string customerId, CustomerUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new System.ArgumentNullException(nameof(customerId));
            if (request == null)
                throw new System.ArgumentNullException(nameof(request));

            string requestUrl = $"{CustomersBaseUrl}/{customerId}";
            return await _client.PostAsync<CustomerUpdateRequest, CustomerResponse>(requestUrl, request); // Typically POST or PUT for update
        }

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        /// <returns>A response indicating the result of the delete operation.</returns>
        public async Task<CustomerDeleteResponse?> DeleteCustomerAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new System.ArgumentNullException(nameof(customerId));

            string requestUrl = $"{CustomersBaseUrl}/{customerId}";
            return await _client.DeleteAsync<CustomerDeleteResponse>(requestUrl);
        }

        /// <summary>
        /// Lists customers, optionally filtered and paginated.
        /// </summary>
        /// <param name="request">(Optional) Parameters to filter and paginate the list of customers via query parameters.</param>
        /// <returns>A direct list of customer objects if successful.</returns>
        public async Task<System.Collections.Generic.List<CustomerResponse>?> ListCustomersAsync(CustomerListRequest? request = null)
        {
            string requestUrl = CustomerListUrl; // This is "/customers/list"
            if (request != null)
            {
                var queryParams = new System.Collections.Generic.List<string>();
                if (request.Limit.HasValue)
                    queryParams.Add($"limit={request.Limit.Value}");
                if (request.Offset.HasValue)
                    queryParams.Add($"offset={request.Offset.Value}");
                if (!string.IsNullOrWhiteSpace(request.Email))
                    queryParams.Add($"email={System.Uri.EscapeDataString(request.Email)}");
                if (!string.IsNullOrWhiteSpace(request.Phone))
                    queryParams.Add($"phone={System.Uri.EscapeDataString(request.Phone)}");
                // Add other supported query params from CustomerListRequest here if API supports them

                if (queryParams.Count > 0)
                {
                    requestUrl += "?" + string.Join("&", queryParams);
                }
            }
            // API returns a direct JSON array of customers for this endpoint.
            return await _client.GetAsync<System.Collections.Generic.List<CustomerResponse>>(requestUrl);
        }

        /// <summary>
        /// Lists the saved payment methods for a specific customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose payment methods are to be listed.</param>
        /// <returns>A response object containing a list of the customer's saved payment methods.</returns>
        public async Task<CustomerPaymentMethodListResponse?> ListPaymentMethodsAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                throw new System.ArgumentNullException(nameof(customerId));

            string requestUrl = $"{CustomersBaseUrl}/{customerId}/payment_methods";
            
            return await _client.GetAsync<CustomerPaymentMethodListResponse>(requestUrl);
        }
    }
}
