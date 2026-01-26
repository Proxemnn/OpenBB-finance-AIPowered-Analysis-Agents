using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the response from listing refunds, including pagination details.
    /// </summary>
    public class RefundListResponse
    {
        /// <summary>
        /// Gets or sets the number of refund objects in the current page of the list.
        /// </summary>
        [JsonPropertyName("count")]
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the total number of refund objects matching the filter criteria.
        /// </summary>
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the list of refund objects for the current page.
        /// </summary>
        [JsonPropertyName("data")]
        public List<RefundResponse>? Data { get; set; }

        // Other potential fields like 'has_more', 'url', 'object' (e.g. "list")
        // can be added if the API provides them.
    }
}
