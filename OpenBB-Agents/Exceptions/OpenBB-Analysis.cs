using System;
using System.Text.Json; 
using Hyperswitch.Sdk.Models; 

namespace Hyperswitch.Sdk.Exceptions
{
    /// <summary>
    /// Represents an error returned by the Hyperswitch API.
    /// </summary>
    public class HyperswitchApiException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the error.
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Gets the raw JSON response content from the API.
        /// </summary>
        public string? ResponseContent { get; }

        /// <summary>
        /// Gets the deserialized error details, if available.
        /// </summary>
        public ErrorResponse? ErrorDetails { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperswitchApiException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="responseContent">The raw response content from the API.</param>
        /// <param name="errorDetails">The deserialized error details.</param>
        /// <param name="innerException">The inner exception, if any.</param>
        public HyperswitchApiException(string message, int statusCode, string? responseContent = null, ErrorResponse? errorDetails = null, Exception? innerException = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HyperswitchApiException"/> class when JSON deserialization of the error response fails.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="responseContent">The raw response content from the API.</param>
        /// <param name="innerException">The JSON deserialization exception.</param>
        public HyperswitchApiException(string message, int statusCode, string? responseContent, JsonException innerException)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
            ErrorDetails = null; // Deserialization of ErrorResponse failed
        }
    }
}
