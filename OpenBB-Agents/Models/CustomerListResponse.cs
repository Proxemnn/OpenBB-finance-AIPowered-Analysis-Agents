using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response from listing customers, including pagination details.
    /// </summary>
    public class CustomerListResponse
    {
        /// <summary>
        /// The number of customer objects in the current page of the list.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// The total number of customer objects matching the filter criteria.
        /// </summary>
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// The list of customer objects for the current page.
        /// </summary>
        [JsonPropertyName("data")]
        public List<CustomerResponse>? Data { get; set; }

        // Other potential pagination fields like 'offset', 'limit' (echoed back), 
        // 'has_more', 'url' could be added if the API provides them.
    }
}
