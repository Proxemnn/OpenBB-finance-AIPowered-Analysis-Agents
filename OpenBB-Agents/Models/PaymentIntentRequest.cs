using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Hyperswitch.Sdk.Models
{
    /// <summary>
    /// Represents the request parameters for creating a payment intent.
    /// </summary>
    public class PaymentIntentRequest
    {
        /// <summary>
        /// Gets or sets the amount to be charged, in the smallest currency unit (e.g., cents for USD).
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; }

        /// <summary>
        /// Gets or sets the three-letter ISO currency code.
        /// </summary>
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        /// <summary>
        /// Gets or sets the Profile ID to use for this payment. 
        /// If not provided, the client's default Profile ID will be used.
        /// </summary>
        [JsonPropertyName("profile_id")]
        public string? ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the customer to whom this payment belongs.
        /// </summary>
        [JsonPropertyName("customer_id")]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets a description for the payment.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the payer's full name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the country code for the customer's phone number.
        /// </summary>
        [JsonPropertyName("phone_country_code")]
        public string? PhoneCountryCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the payment should be immediately confirmed.
        /// Default is true. If false, the payment intent will be created in a 'requires_confirmation' state.
        /// </summary>
        [JsonPropertyName("confirm")]
        public bool? Confirm { get; set; } = true;

        /// <summary>
        /// Gets or sets how the payment should be captured.
        /// Possible values: "automatic" (default), "manual".
        /// </summary>
        [JsonPropertyName("capture_method")]
        public string? CaptureMethod { get; set; }

        /// <summary>
        /// Gets or sets the specific date and time to capture the payment (ISO 8601 format, e.g., "2029-09-10T10:11:12Z").
        /// Used when CaptureMethod is "manual" and a future capture is desired.
        /// </summary>
        [JsonPropertyName("capture_on")]
        public string? CaptureOn { get; set; }

        /// <summary>
        /// Gets or sets the amount to capture. If not provided, the full amount of the payment intent is captured.
        /// </summary>
        [JsonPropertyName("amount_to_capture")]
        public long? AmountToCapture { get; set; }

        /// <summary>
        /// Gets or sets the URL to redirect your customer back to after they authenticate on the payment processor's page.
        /// </summary>
        [JsonPropertyName("return_url")]
        public string? ReturnUrl { get; set; }

        /// <summary>
        /// Gets or sets the type of authentication to use for the payment (e.g., "no_three_ds").
        /// </summary>
        [JsonPropertyName("authentication_type")]
        public string? AuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the primary payment method type (e.g., "card").
        /// </summary>
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the specific payment method subtype (e.g., "credit", "debit").
        /// </summary>
        [JsonPropertyName("payment_method_type")]
        public string? PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets the payment method data.
        /// </summary>
        [JsonPropertyName("payment_method_data")]
        public PaymentMethodData? PaymentMethodData { get; set; }

        /// <summary>
        /// Gets or sets the shipping address for the order.
        /// </summary>
        [JsonPropertyName("shipping")]
        public Address? Shipping { get; set; }

        /// <summary>
        /// Gets or sets the billing address for the order.
        /// Note: For card payments, billing address specific to the card should be within PaymentMethodData.Card.Billing.
        /// </summary>
        [JsonPropertyName("billing")]
        public Address? Billing { get; set; }

        /// <summary>
        /// Gets or sets browser information for the customer.
        /// </summary>
        [JsonPropertyName("browser_info")]
        public BrowserInfo? BrowserInfo { get; set; }

        /// <summary>
        /// Gets or sets a set of key-value pairs that you can attach to an object.
        /// This can be useful for storing additional information about the object in a structured format.
        /// </summary>
        [JsonPropertyName("metadata")]
        public Dictionary<string, string>? Metadata { get; set; }

        /// <summary>
        /// Gets or sets a list of items in the order.
        /// </summary>
        [JsonPropertyName("order_details")]
        public List<OrderDetailItem>? OrderDetails { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a payment link.
        /// </summary>
        [JsonPropertyName("payment_link")]
        public bool? PaymentLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if this payment method should be saved for future use by the customer.
        /// Common values: "on_session", "off_session". Requires customer_id to be set.
        /// </summary>
        [JsonPropertyName("setup_future_usage")]
        public string? SetupFutureUsage { get; set; }

        /// <summary>
        /// Gets or sets mandate data for setting up new mandates.
        /// </summary>
        [JsonPropertyName("mandate_data")]
        public MandateData? MandateData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the payment is an off-session payment.
        /// </summary>
        [JsonPropertyName("off_session")]
        public bool? OffSession { get; set; }

        /// <summary>
        /// Gets or sets recurring payment details, used for subsequent mandate payments.
        /// </summary>
        [JsonPropertyName("recurring_details")]
        public RecurringDetailsInfo? RecurringDetails { get; set; }
        
        /// <summary>
        /// Gets or sets customer acceptance details directly at the top level of the request.
        /// This may be used for specific connector requirements (e.g., Stripe) as an alternative
        /// to providing it within MandateData.CustomerAcceptance.
        /// </summary>
        [JsonPropertyName("customer_acceptance")]
        public CustomerAcceptance? CustomerAcceptance { get; set; }
    }
    

    /// <summary>
    /// Represents the data for a specific payment method.
    /// </summary>
    public class PaymentMethodData
    {
        /// <summary>
        /// Gets or sets card details if the payment method is a card.
        /// </summary>
        [JsonPropertyName("card")]
        public CardDetails? Card { get; set; }

        /// <summary>
        /// Gets or sets the billing address specific to this payment method.
        /// </summary>
        [JsonPropertyName("billing")]
        public Address? Billing { get; set; } 
    }

    /// <summary>
    /// Represents the details of a payment card.
    /// </summary>
    public class CardDetails
    {
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        [JsonPropertyName("card_number")] 
        public string? CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card's expiry month.
        /// </summary>
        [JsonPropertyName("card_exp_month")] 
        public string? CardExpiryMonth { get; set; }

        /// <summary>
        /// Gets or sets the card's expiry year.
        /// </summary>
        [JsonPropertyName("card_exp_year")] 
        public string? CardExpiryYear { get; set; }

        /// <summary>
        /// Gets or sets the card's CVC/CVV code.
        /// </summary>
        [JsonPropertyName("card_cvc")] 
        public string? CardCvc { get; set; }
    }
    
    /// <summary>
    /// Represents an address, used for shipping, billing, or within payment method data.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets the detailed address information.
        /// </summary>
        [JsonPropertyName("address")]
        public AddressDetails? AddressDetails { get; set; } 

        /// <summary>
        /// Gets or sets phone details associated with this address.
        /// </summary>
        [JsonPropertyName("phone")] 
        public PhoneDetails? Phone { get; set; } 
    }

    /// <summary>
    /// Represents detailed components of a postal address.
    /// </summary>
    public class AddressDetails 
    {
        /// <summary>
        /// Gets or sets the first line of the address.
        /// </summary>
        [JsonPropertyName("line1")]
        public string? Line1 { get; set; }

        /// <summary>
        /// Gets or sets the second line of the address.
        /// </summary>
        [JsonPropertyName("line2")]
        public string? Line2 { get; set; }

        /// <summary>
        /// Gets or sets the third line of the address.
        /// </summary>
        [JsonPropertyName("line3")]
        public string? Line3 { get; set; }
        
        /// <summary>
        /// Gets or sets the city, district, suburb, town, or village.
        /// </summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the state, county, province, or region.
        /// </summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        /// <summary>
        /// Gets or sets the ZIP or postal code.
        /// </summary>
        [JsonPropertyName("zip")]
        public string? Zip { get; set; }

        /// <summary>
        /// Gets or sets the two-letter country code (ISO 3166-1 alpha-2).
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; } 

        /// <summary>
        /// Gets or sets the first name of the recipient or resident.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }
        
        /// <summary>
        /// Gets or sets the last name of the recipient or resident.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }
    }

    /// <summary>
    /// Represents phone number details.
    /// </summary>
    public class PhoneDetails 
    {
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [JsonPropertyName("number")]
        public string? Number { get; set; }

        /// <summary>
        /// Gets or sets the phone number's country code.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string? CountryCode { get; set; }
    }

    /// <summary>
    /// Represents browser information of the customer.
    /// </summary>
    public class BrowserInfo
    {
        /// <summary>
        /// Gets or sets the color depth of the customer's browser screen.
        /// </summary>
        [JsonPropertyName("color_depth")]
        public int? ColorDepth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Java is enabled in the customer's browser.
        /// </summary>
        [JsonPropertyName("java_enabled")]
        public bool? JavaEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether JavaScript is enabled in the customer's browser.
        /// </summary>
        [JsonPropertyName("java_script_enabled")]
        public bool? JavaScriptEnabled { get; set; }

        /// <summary>
        /// Gets or sets the language of the customer's browser.
        /// </summary>
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        /// <summary>
        /// Gets or sets the screen height of the customer's browser.
        /// </summary>
        [JsonPropertyName("screen_height")]
        public int? ScreenHeight { get; set; }

        /// <summary>
        /// Gets or sets the screen width of the customer's browser.
        /// </summary>
        [JsonPropertyName("screen_width")]
        public int? ScreenWidth { get; set; }

        /// <summary>
        /// Gets or sets the time zone offset of the customer's browser.
        /// </summary>
        [JsonPropertyName("time_zone")]
        public int? TimeZone { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the customer.
        /// </summary>
        [JsonPropertyName("ip_address")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the accept header from the customer's browser.
        /// </summary>
        [JsonPropertyName("accept_header")]
        public string? AcceptHeader { get; set; }

        /// <summary>
        /// Gets or sets the user agent string from the customer's browser.
        /// </summary>
        [JsonPropertyName("user_agent")]
        public string? UserAgent { get; set; }
    }

    /// <summary>
    /// Represents an item in the order details.
    /// </summary>
    public class OrderDetailItem
    {
        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        [JsonPropertyName("product_name")]
        public string? ProductName { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        /// <summary>
        /// Gets or sets the amount for this item, in the smallest currency unit.
        /// </summary>
        [JsonPropertyName("amount")]
        public long? Amount { get; set; } 

        /// <summary>
        /// Gets or sets the link to the product image.
        /// </summary>
        [JsonPropertyName("product_img_link")]
        public string? ProductImgLink { get; set; }
    }
}
