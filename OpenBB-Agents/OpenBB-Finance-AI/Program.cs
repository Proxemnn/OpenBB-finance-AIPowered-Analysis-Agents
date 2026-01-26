using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hyperswitch.Sdk;
using Hyperswitch.Sdk.Models;
using Hyperswitch.Sdk.Services;
using Hyperswitch.Sdk.Exceptions;

namespace Hyperswitch.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hyperswitch SDK Sample - Full Test Suite");

            string secretKey = "API_KEY_HERE"; 
            string publishableKey = "PUBLISHABLE_KEY_HERE";
            string defaultProfileId = "PROFILE_ID_HERE";

            var client = new HyperswitchClient(secretKey: secretKey, publishableKey: publishableKey, defaultProfileId: defaultProfileId);
            var paymentService = new PaymentService(client);
            var refundService = new RefundService(client);
            var customerService = new CustomerService(client);
            var merchantService = new MerchantService(client);

            var createdPaymentIds = new List<string?>();
            RefundResponse? lastCreatedRefund = null;
            string? createdTestCustomerId = null; // This customer is used for general tests

            Console.WriteLine("\n--- SCENARIO 1: MANUAL CAPTURE (Two-Step Create-Confirm) ---");
            var p1 = await TestPaymentFlowTwoStep(paymentService, "manual");
            if (p1 != null && !string.IsNullOrEmpty(p1.PaymentId)) createdPaymentIds.Add(p1.PaymentId);

            Console.WriteLine("\n--- SCENARIO 2: AUTOMATIC CAPTURE (Two-Step Create-Confirm) ---");
            var p2 = await TestPaymentFlowTwoStep(paymentService, "automatic");
            if (p2 != null && !string.IsNullOrEmpty(p2.PaymentId)) createdPaymentIds.Add(p2.PaymentId);

            Console.WriteLine("\n--- SCENARIO 3: SINGLE-CALL MANUAL CAPTURE (Create with Confirm:true) ---");
            var p3 = await TestSingleCallManualCaptureAsync(paymentService);
            if (p3 != null && !string.IsNullOrEmpty(p3.PaymentId)) createdPaymentIds.Add(p3.PaymentId);

            Console.WriteLine("\n--- SCENARIO 4: VOID/CANCEL PAYMENT ---");
            var p4 = await TestVoidPaymentAsync(paymentService);
            if (p4 != null && !string.IsNullOrEmpty(p4.PaymentId)) createdPaymentIds.Add(p4.PaymentId);

            Console.WriteLine("\n--- SCENARIO 5: UPDATE PAYMENT ---");
            var p5 = await TestUpdatePaymentAsync(paymentService);
            if (p5 != null && !string.IsNullOrEmpty(p5.PaymentId)) createdPaymentIds.Add(p5.PaymentId);

            Console.WriteLine("\n--- SCENARIO 6: REFUND PAYMENT ---");
            var paymentForRefund = await CreateAndConfirmPaymentForRefund(paymentService);
            if (paymentForRefund != null && paymentForRefund.Status == "succeeded" && !string.IsNullOrEmpty(paymentForRefund.PaymentId))
            { lastCreatedRefund = await TestRefundPaymentAsync(paymentService, refundService, paymentForRefund.PaymentId); }
            else { Console.WriteLine($"Skipping Refund test as prerequisite payment was not successful (Status: {paymentForRefund?.Status})."); }

            Console.WriteLine("\n--- SCENARIO 7: UPDATE REFUND ---");
            if (lastCreatedRefund != null && !string.IsNullOrEmpty(lastCreatedRefund.RefundId) && lastCreatedRefund.Status == "pending")
            { await TestUpdateRefundAsync(refundService, lastCreatedRefund.RefundId); }
            else { Console.WriteLine($"Skipping Update Refund test (No suitable pending refund from Scenario 6. Status: {lastCreatedRefund?.Status})."); }

            Console.WriteLine("\n--- SCENARIO 8: LIST REFUNDS ---");
            string? paymentIdForListTest = lastCreatedRefund?.PaymentId ?? createdPaymentIds.FirstOrDefault(id => !string.IsNullOrEmpty(id));
            if (!string.IsNullOrEmpty(paymentIdForListTest))
            {
                Console.WriteLine($"Listing refunds for payment ID: {paymentIdForListTest}");
                await TestListRefundsAsync(refundService, paymentIdToList: paymentIdForListTest);
            }
            Console.WriteLine("Listing general refunds (limit 3)...");
            await TestListRefundsAsync(refundService, limit: 3);

            Console.WriteLine("\n--- SCENARIO 10: CREATE CUSTOMER (Main Test Customer) ---");
            createdTestCustomerId = await TestCreateCustomerAsync(customerService); // This customer is used for several scenarios

            Console.WriteLine($"\n--- SCENARIO 18: MANDATE PAYMENT FLOW ---");
            // Part A of Mandate Flow uses createdTestCustomerId
            await TestMandatePaymentFlowAsync(paymentService, customerService, createdTestCustomerId);

            if (!string.IsNullOrEmpty(createdTestCustomerId))
            {
                Console.WriteLine($"\n--- SCENARIO 11: RETRIEVE CUSTOMER (Main Test Customer: {createdTestCustomerId}) ---");
                await TestRetrieveCustomerAsync(customerService, createdTestCustomerId);

                Console.WriteLine($"\n--- SCENARIO 12: UPDATE CUSTOMER (Main Test Customer: {createdTestCustomerId}) ---");
                await TestUpdateCustomerAsync(customerService, createdTestCustomerId);

                Console.WriteLine($"\n--- SCENARIO 13: LIST CUSTOMERS ---");
                await TestListCustomersAsync(customerService);

                Console.WriteLine($"\n--- SCENARIO 9 (REVISED): LIST CUSTOMER PAYMENT METHODS (Main Test Customer: {createdTestCustomerId}) ---");
                await TestListCustomerPaymentMethodsAsync(customerService, createdTestCustomerId);

                Console.WriteLine($"\n--- SCENARIO 16: FULL PAYMENT FLOW WITH PM SELECTION (Main Test Customer: {createdTestCustomerId}) ---");
                await TestFullPaymentFlowWithPMSelection(paymentService, customerService, merchantService, createdTestCustomerId);

                Console.WriteLine($"\n--- SCENARIO 17: LIST SAVED PMS FOR CUSTOMER ASSOCIATED WITH A PAYMENT (Main Test Customer: {createdTestCustomerId}) ---");
                await TestListCustomerPMsFromPaymentContext(paymentService, customerService, createdTestCustomerId);

                // Note: Deletion of createdTestCustomerId is not explicitly done here as it might have active mandates.
                // The self-contained TestDeleteCustomerAsync (Scenario 14) tests deletion independently.
            }
            else { Console.WriteLine("\nSkipping Customer-dependent tests (Scenarios 11-13, 9, 16, 17, Part A of 18) as main customer creation failed."); }

            Console.WriteLine($"\n--- SCENARIO 14 & 15: CREATE, DELETE, AND VERIFY DELETION OF A TEMPORARY CUSTOMER ---");
            await TestDeleteCustomerAsync(customerService); // This is now self-contained

            client.Dispose();
        }

        static async Task TestSpecificPaymentSyncAndRefundAsync(PaymentService paymentService, RefundService refundService, string paymentIdToTest)
        {
            Console.WriteLine($"  Testing Sync for Payment ID: {paymentIdToTest}");
            PaymentIntentResponse? paymentDetails = null;
            try
            {
                paymentDetails = await paymentService.SyncPaymentStatusAsync(paymentIdToTest, forceSync: true);
                if (paymentDetails != null)
                {
                    PrintPaymentDetails($"  Synced Payment Details for {paymentIdToTest}", paymentDetails);
                }
                else
                {
                    PrintAndReturnError($"  Sync returned null for Payment ID: {paymentIdToTest}. It might not exist or an issue occurred.");
                    return;
                }
            }
            catch (HyperswitchApiException apiEx)
            {
                PrintApiError(apiEx, $"in sync for payment {paymentIdToTest}");
                return;
            }
            catch (Exception ex)
            {
                PrintGenericError(ex, $"in sync for payment {paymentIdToTest}");
                return;
            }

            if (paymentDetails.Status == "succeeded")
            {
                Console.WriteLine($"\n  Payment {paymentIdToTest} status is 'succeeded'. Attempting refund...");
                long amountToRefund = paymentDetails.Amount / 2;
                if (amountToRefund == 0 && paymentDetails.Amount > 0) amountToRefund = paymentDetails.Amount;
                if (amountToRefund == 0)
                {
                    PrintAndReturnError($"  Cannot refund 0 amount for payment {paymentIdToTest}. Original amount: {paymentDetails.Amount}");
                    return;
                }

                var refundRequest = new RefundCreateRequest
                {
                    PaymentId = paymentIdToTest,
                    Amount = amountToRefund,
                    Reason = "OTHER",
                    Metadata = new Dictionary<string, string> { { "test_type", "specific_payment_refund" } }
                };

                try
                {
                    RefundResponse? createdRefund = await refundService.CreateRefundAsync(refundRequest);
                    if (createdRefund != null && !string.IsNullOrEmpty(createdRefund.RefundId))
                    {
                        PrintRefundDetails($"  Refund Created for {paymentIdToTest}", createdRefund);

                        await Task.Delay(1000);
                        RefundResponse? retrievedRefund = await refundService.RetrieveRefundAsync(createdRefund.RefundId);
                        PrintRefundDetails($"  Retrieved Refund Status for {createdRefund.RefundId}", retrievedRefund);
                    }
                    else
                    {
                        PrintAndReturnError($"  Refund creation failed or did not return a RefundId for payment {paymentIdToTest}.");
                    }
                }
                catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in creating refund for payment {paymentIdToTest}"); }
                catch (Exception ex) { PrintGenericError(ex, $"in creating refund for payment {paymentIdToTest}"); }
            }
            else
            {
                Console.WriteLine($"  Payment {paymentIdToTest} is not in 'succeeded' state (Current status: {paymentDetails.Status}). Cannot process refund.");
            }
        }

        static async Task TestListCustomerPMsFromPaymentContext(PaymentService paymentService, CustomerService customerService, string customerId)
        {
            Console.WriteLine($"  Testing listing customer PMs using customer ID from a payment context for Customer ID: {customerId}");
            PaymentIntentResponse? paymentIntent = null;
            try
            {
                Console.WriteLine("    1. Creating a new Payment Intent associated with the customer...");
                var createPiRequest = new PaymentIntentRequest
                {
                    Amount = 500,
                    Currency = "USD",
                    CustomerId = customerId,
                    Confirm = false,
                    Description = "Payment for Customer PM List Test"
                };
                paymentIntent = await paymentService.CreateAsync(createPiRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId))
                {
                    PrintAndReturnError("    Payment Intent creation failed for PM list test.");
                    return;
                }
                PrintPaymentDetails("    1. Payment Intent Created", paymentIntent);

                if (string.IsNullOrEmpty(paymentIntent.CustomerId))
                {
                    PrintAndReturnError("    CustomerId not found in PaymentIntentResponse, cannot list PMs.");
                    return;
                }

                Console.WriteLine($"\n    2. Listing payment methods for Customer ID: {paymentIntent.CustomerId} (obtained from payment intent)...");
                await TestListCustomerPaymentMethodsAsync(customerService, paymentIntent.CustomerId);
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in TestListCustomerPMsFromPaymentContext for customer {customerId}"); }
            catch (Exception ex) { PrintGenericError(ex, $"in TestListCustomerPMsFromPaymentContext for customer {customerId}"); }
        }


        static async Task TestFullPaymentFlowWithPMSelection(
            PaymentService paymentService,
            CustomerService customerService,
            MerchantService merchantService,
            string customerId)
        {
            Console.WriteLine($"Starting full payment flow for customer: {customerId}");
            PaymentIntentResponse? paymentIntent = null;

            try
            {
                Console.WriteLine("  1. Creating Payment Intent (confirm: false)...");
                var createPiRequest = new PaymentIntentRequest
                {
                    Amount = 1500,
                    Currency = "USD",
                    CustomerId = customerId,
                    Confirm = false,
                    SetupFutureUsage = "on_session",
                    Description = "Test Payment Flow with PM Selection"
                };
                paymentIntent = await paymentService.CreateAsync(createPiRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId))
                { PrintAndReturnError("  Payment Intent creation failed."); return; }
                PrintPaymentDetails("  1. Payment Intent Created", paymentIntent);
                string paymentId = paymentIntent.PaymentId;

                Console.WriteLine("\n  2. Listing customer's saved payment methods...");
                CustomerPaymentMethodListResponse? savedPms = await customerService.ListPaymentMethodsAsync(customerId);
                PaymentConfirmRequest confirmRequest = new PaymentConfirmRequest { ReturnUrl = "https://example.com/sdk/return" };

                if (savedPms != null && savedPms.Data != null && savedPms.Data.Any())
                {
                    var firstSavedPm = savedPms.Data.First();
                    Console.WriteLine($"    Found {savedPms.PaymentMethodCount} saved PM(s). Simulating selection of first one: {firstSavedPm.PaymentToken} ({firstSavedPm.PaymentMethod} {firstSavedPm.Card?.Last4Digits})");
                    confirmRequest.PaymentMethodToken = firstSavedPm.PaymentToken;
                }
                else
                {
                    Console.WriteLine("    No saved payment methods found for customer. Simulating selection of a new payment method.");
                    Console.WriteLine("\n  4. Listing available merchant payment methods (PML) using ClientSecret and pk_snd_ key...");

                    if (string.IsNullOrEmpty(paymentIntent.ClientSecret))
                    {
                        PrintAndReturnError("  ClientSecret is missing from created Payment Intent. Cannot list PML for this test.");
                        return;
                    }

                    var pmlRequest = new MerchantPMLRequest { ClientSecret = paymentIntent.ClientSecret };
                    MerchantPMLResponse? pmlResponse = await merchantService.ListAvailablePaymentMethodsAsync(pmlRequest);

                    if (pmlResponse != null && pmlResponse.PaymentMethods != null && pmlResponse.PaymentMethods.Any())
                    {
                        var cardPaymentMethodGroup = pmlResponse.PaymentMethods.FirstOrDefault(pmg => pmg.PaymentMethodCategory?.ToLower() == "card");
                        if (cardPaymentMethodGroup != null && cardPaymentMethodGroup.PaymentMethodTypes != null && cardPaymentMethodGroup.PaymentMethodTypes.Any())
                        {
                            var firstCardTypeDetails = cardPaymentMethodGroup.PaymentMethodTypes.First();
                            Console.WriteLine($"    Simulating selection of new 'card' payment method (type: {firstCardTypeDetails.PaymentMethodType}) from PML.");
                            confirmRequest.PaymentMethod = "card";
                            confirmRequest.PaymentMethodData = new PaymentMethodData
                            {
                                Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "123" }
                            };
                            confirmRequest.BrowserInfo = new BrowserInfo
                            {
                                AcceptHeader = "application/json",
                                UserAgent = "Hyperswitch .NET SDK Sample v1.0",
                                IpAddress = "192.168.1.100",
                                Language = "en-US",
                                ScreenHeight = 1080,
                                ScreenWidth = 1920,
                                ColorDepth = 24,
                                JavaEnabled = true,
                                JavaScriptEnabled = true,
                                TimeZone = -330
                            };
                        }
                        else { PrintAndReturnError("    'card' payment method category or its types not found in merchant PML."); return; }
                    }
                    else { PrintAndReturnError("    Failed to retrieve merchant PML, or PML is empty or has no payment_methods list."); return; }
                }

                Console.WriteLine("\n  5. Confirming payment...");
                paymentIntent = await paymentService.ConfirmPaymentAsync(paymentId, confirmRequest);
                if (paymentIntent == null) { PrintAndReturnError("  Payment confirmation failed."); return; }
                PrintPaymentDetails("  5. After Payment Confirmation", paymentIntent);

                if (paymentIntent.Status == "requires_customer_action")
                { Console.WriteLine($"    Payment requires customer action. Redirect to: {paymentIntent.NextAction?.RedirectToUrl}"); }
                else if (paymentIntent.Status == "succeeded")
                { Console.WriteLine("    Payment succeeded!"); }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in full payment flow for customer {customerId}"); }
            catch (Exception ex) { PrintGenericError(ex, $"in full payment flow for customer {customerId}"); }
        }

        static async Task<PaymentIntentResponse?> CreateAndConfirmPaymentForRefund(PaymentService paymentService)
        {
            Console.WriteLine("   Creating a payment with auto-capture for refund testing...");
            var createRequest = new PaymentIntentRequest { Amount = 1200, Currency = "USD", Confirm = true, CaptureMethod = "automatic", PaymentMethod = "card", PaymentMethodType = "credit", Email = "customer-for-refund@example.com", Description = "Payment for Refund Test", ReturnUrl = "https://example.com/sdk_auto_capture_return", PaymentMethodData = new PaymentMethodData { Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }, Billing = new Address { AddressDetails = new AddressDetails { Line1 = "404 Refund Ln", City = "SucceedCity", Country = "US", Zip = "33333" } } }, AuthenticationType = "no_three_ds", BrowserInfo = new BrowserInfo { ColorDepth = 24, JavaEnabled = true, JavaScriptEnabled = true, Language = "en-US", ScreenHeight = 720, ScreenWidth = 1280, TimeZone = -330, IpAddress = "208.127.127.198", AcceptHeader = "application/json", UserAgent = "Mozilla/5.0 SDK Refund Prereq" } };
            PaymentIntentResponse? paymentIntent = await paymentService.CreateAsync(createRequest);
            if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId)) { PrintAndReturnError("   Payment creation for refund test failed."); return null; }
            PrintPaymentDetails("   Prereq Payment Created", paymentIntent);

            int attempts = 0;
            while (paymentIntent.Status == "requires_customer_action" || paymentIntent.Status == "processing" || paymentIntent.Status == "requires_capture")
            {
                if (attempts >= 3) { Console.ForegroundColor = ConsoleColor.Yellow; Console.WriteLine($"   Payment {paymentIntent.PaymentId} did not reach 'succeeded' after {attempts} syncs for refund test. Current status: {paymentIntent.Status}. Manual browser auth might be needed for this payment to be used in refund tests."); Console.ResetColor(); return paymentIntent; }
                Console.WriteLine($"\n   Syncing prereq payment {paymentIntent.PaymentId} (attempt {++attempts}), current status: {paymentIntent.Status}...");
                await Task.Delay(2000);
                paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                if (paymentIntent == null) { PrintAndReturnError("   Sync returned null for refund prereq."); return null; }
                PrintPaymentDetails($"   After Prereq Sync Attempt {attempts}", paymentIntent);
            }
            return paymentIntent;
        }

        static async Task<PaymentIntentResponse?> TestPaymentFlowTwoStep(PaymentService paymentService, string captureMethod)
        {
            Console.WriteLine($"Testing Two-Step Flow with CaptureMethod: {captureMethod}");
            PaymentIntentResponse? paymentIntent = null;
            try
            {
                Console.WriteLine("\n1. Creating Payment Intent (confirm: false)...");
                var createRequest = new PaymentIntentRequest { Amount = 650, Currency = "USD", Confirm = false, CaptureMethod = captureMethod, PaymentMethod = "card", PaymentMethodType = "credit", Email = "customer-sdk-test@example.com", Description = $"Test SDK Payment ({captureMethod} capture, two-step)", ReturnUrl = "https://example.com/sdk_return", PaymentMethodData = new PaymentMethodData { Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }, Billing = new Address { AddressDetails = new AddressDetails { Line1 = "123 Test St", City = "Testville", Country = "US", Zip = "12345" } } }, AuthenticationType = "no_three_ds", BrowserInfo = new BrowserInfo { ColorDepth = 24, JavaEnabled = true, JavaScriptEnabled = true, Language = "en-GB", ScreenHeight = 720, ScreenWidth = 1280, TimeZone = -330, IpAddress = "208.127.127.193", AcceptHeader = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8", UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36 Edg/126.0.0.0" } };
                paymentIntent = await paymentService.CreateAsync(createRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId) || string.IsNullOrEmpty(paymentIntent.ClientSecret)) { PrintAndReturnError("PaymentId or ClientSecret is missing after create."); return null; }
                PrintPaymentDetails("1. After Create (confirm:false)", paymentIntent);

                if (paymentIntent.Status == "requires_confirmation" || paymentIntent.Status == "requires_payment_method")
                {
                    Console.WriteLine($"\n2. Confirming Payment Intent {paymentIntent.PaymentId}...");
                    var confirmRequest = new PaymentConfirmRequest { ReturnUrl = createRequest.ReturnUrl, BrowserInfo = createRequest.BrowserInfo, CaptureMethod = createRequest.CaptureMethod };
                    paymentIntent = await paymentService.ConfirmPaymentAsync(paymentIntent.PaymentId!, confirmRequest);
                    if (paymentIntent == null) { PrintAndReturnError("Payment Intent confirmation returned null or failed."); return null; }
                    PrintPaymentDetails("2. After Confirm", paymentIntent);
                }
                else { Console.WriteLine($"\n2. Payment status is '{paymentIntent.Status}', skipping explicit Confirm call."); }

                Console.WriteLine($"\n3. Syncing status for {paymentIntent.PaymentId}...");
                paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                if (paymentIntent == null) { PrintAndReturnError("Payment Intent sync returned null or failed."); return null; }
                PrintPaymentDetails("3. After Sync", paymentIntent);

                if (captureMethod == "manual" && paymentIntent.Status == "requires_capture" && !string.IsNullOrEmpty(paymentIntent.PaymentId))
                {
                    Console.WriteLine($"\n4. Capturing Payment {paymentIntent.PaymentId}...");
                    var captureRequest = new PaymentCaptureRequest { AmountToCapture = paymentIntent.AmountCapturable };
                    paymentIntent = await paymentService.CapturePaymentAsync(paymentIntent.PaymentId!, captureRequest);
                    if (paymentIntent == null) { PrintAndReturnError("Payment Capture returned null or failed."); return null; }
                    PrintPaymentDetails("4. After Capture", paymentIntent);
                    Console.WriteLine($"\n5. Final Sync for {paymentIntent.PaymentId} after capture attempt...");
                    paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                    PrintPaymentDetails("5. After Final Sync (post-capture)", paymentIntent);
                }
                else if (captureMethod == "manual")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine($"\n4. Payment status is '{paymentIntent.Status}' (expected 'requires_capture' for manual test). Capture not attempted."); Console.ResetColor();
                }
                else { Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine($"\n4. Automatic capture method. Final status should be checked after confirm/sync."); Console.ResetColor(); }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in {captureMethod} two-step flow"); }
            catch (Exception ex) { PrintGenericError(ex, $"in {captureMethod} two-step flow"); }
            return paymentIntent;
        }

        static async Task<PaymentIntentResponse?> TestSingleCallManualCaptureAsync(PaymentService paymentService)
        {
            Console.WriteLine($"Testing Single-Call Flow with Confirm:true, CaptureMethod:manual");
            PaymentIntentResponse? paymentIntent = null;
            try
            {
                Console.WriteLine("\n1. Creating Payment Intent (confirm: true, capture_method: manual)...");
                var createRequest = new PaymentIntentRequest { Amount = 750, Currency = "USD", Confirm = true, CaptureMethod = "manual", PaymentMethod = "card", PaymentMethodType = "credit", Email = "customer-single-call-manual@example.com", Description = "Test SDK Payment (Single-Call Manual Capture)", ReturnUrl = "https://example.com/sdk_single_call_return", PaymentMethodData = new PaymentMethodData { Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }, Billing = new Address { AddressDetails = new AddressDetails { Line1 = "789 Test Ave", City = "Testburg", Country = "US", Zip = "67890" } } }, AuthenticationType = "no_three_ds", BrowserInfo = new BrowserInfo { ColorDepth = 24, JavaEnabled = true, JavaScriptEnabled = true, Language = "en-US", ScreenHeight = 1080, ScreenWidth = 1920, TimeZone = -300, IpAddress = "208.127.127.194", AcceptHeader = "application/json", UserAgent = "Mozilla/5.0 SDK Test" } };
                paymentIntent = await paymentService.CreateAsync(createRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId)) { PrintAndReturnError("PaymentId is missing after create."); return null; }
                PrintPaymentDetails("1. After Create (confirm:true, manual capture)", paymentIntent);

                Console.WriteLine($"\n2. Syncing status for {paymentIntent.PaymentId}...");
                paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                if (paymentIntent == null) { PrintAndReturnError("Payment Intent sync returned null or failed."); return null; }
                PrintPaymentDetails("2. After Sync", paymentIntent);

                if (paymentIntent.Status == "requires_capture" && !string.IsNullOrEmpty(paymentIntent.PaymentId))
                {
                    Console.WriteLine($"\n3. Capturing Payment {paymentIntent.PaymentId}...");
                    var captureRequest = new PaymentCaptureRequest { AmountToCapture = paymentIntent.AmountCapturable };
                    paymentIntent = await paymentService.CapturePaymentAsync(paymentIntent.PaymentId!, captureRequest);
                    if (paymentIntent == null) { PrintAndReturnError("Payment Capture returned null or failed."); return null; }
                    PrintPaymentDetails("3. After Capture", paymentIntent);
                    Console.WriteLine($"\n4. Final Sync for {paymentIntent.PaymentId} after capture attempt...");
                    paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                    PrintPaymentDetails("4. After Final Sync (post-capture)", paymentIntent);
                }
                else { Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine($"\n3. Payment status is '{paymentIntent.Status}' (expected 'requires_capture'). Capture not attempted."); Console.ResetColor(); }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in single-call manual capture flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in single-call manual capture flow"); }
            return paymentIntent;
        }

        static async Task<PaymentIntentResponse?> TestVoidPaymentAsync(PaymentService paymentService)
        {
            Console.WriteLine($"Testing Void/Cancel Payment Flow");
            PaymentIntentResponse? paymentIntent = null;
            try
            {
                Console.WriteLine("\n1. Creating Payment Intent for Void Test (confirm: true, capture_method: manual)...");
                var createRequest = new PaymentIntentRequest { Amount = 800, Currency = "USD", Confirm = true, CaptureMethod = "manual", PaymentMethod = "card", PaymentMethodType = "credit", Email = "customer-void-test@example.com", Description = "Test SDK Payment (for Voiding)", ReturnUrl = "https://example.com/sdk_void_return", PaymentMethodData = new PaymentMethodData { Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }, Billing = new Address { AddressDetails = new AddressDetails { Line1 = "101 Void St", City = "Cancelburg", Country = "US", Zip = "00000" } } }, AuthenticationType = "no_three_ds", BrowserInfo = new BrowserInfo { ColorDepth = 24, JavaEnabled = true, JavaScriptEnabled = true, Language = "en-US", ScreenHeight = 720, ScreenWidth = 1280, TimeZone = -330, IpAddress = "208.127.127.195", AcceptHeader = "application/json", UserAgent = "Mozilla/5.0 SDK Void Test" } };
                paymentIntent = await paymentService.CreateAsync(createRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId)) { PrintAndReturnError("PaymentId is missing after create for void test."); return null; }
                PrintPaymentDetails("1. After Create (for Void Test)", paymentIntent);

                Console.WriteLine($"\n2. Syncing status for {paymentIntent.PaymentId} before attempting void...");
                paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                if (paymentIntent == null) { PrintAndReturnError("Payment Intent sync (pre-void) returned null or failed."); return null; }
                PrintPaymentDetails("2. After Sync (pre-void)", paymentIntent);

                if (paymentIntent.Status != "succeeded" && paymentIntent.Status != "failed" && paymentIntent.Status != "cancelled")
                {
                    Console.WriteLine($"\n3. Attempting to Cancel/Void Payment {paymentIntent.PaymentId} (Status: {paymentIntent.Status})...");
                    var cancelRequest = new PaymentCancelRequest { CancellationReason = "Test cancellation from SDK" };
                    paymentIntent = await paymentService.CancelPaymentAsync(paymentIntent.PaymentId!, cancelRequest);
                    if (paymentIntent == null) { PrintAndReturnError("Payment Cancel/Void call returned null or failed."); return null; }
                    PrintPaymentDetails("3. After Cancel/Void Attempt", paymentIntent);
                }
                else { Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine($"\n3. Payment status is '{paymentIntent.Status}'. Void not attempted."); Console.ResetColor(); }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in void payment flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in void payment flow"); }
            return paymentIntent;
        }

        static async Task<PaymentIntentResponse?> TestUpdatePaymentAsync(PaymentService paymentService)
        {
            Console.WriteLine($"Testing Update Payment Flow");
            PaymentIntentResponse? paymentIntent = null;
            try
            {
                Console.WriteLine("\n1. Creating Payment Intent for Update Test (confirm: false)...");
                var createRequest = new PaymentIntentRequest { Amount = 850, Currency = "USD", Confirm = false, CaptureMethod = "manual", PaymentMethod = "card", PaymentMethodType = "credit", Email = "customer-update-initial@example.com", Description = "Initial Description for Update Test", ReturnUrl = "https://example.com/sdk_update_return", PaymentMethodData = new PaymentMethodData { Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }, Billing = new Address { AddressDetails = new AddressDetails { Line1 = "202 Update Ave", City = "Modifyville", Country = "US", Zip = "11111" } } }, AuthenticationType = "no_three_ds", BrowserInfo = new BrowserInfo { ColorDepth = 24, JavaEnabled = true, JavaScriptEnabled = true, Language = "en-US", ScreenHeight = 720, ScreenWidth = 1280, TimeZone = -330, IpAddress = "208.127.127.196", AcceptHeader = "application/json", UserAgent = "Mozilla/5.0 SDK Update Test" } };
                paymentIntent = await paymentService.CreateAsync(createRequest);
                if (paymentIntent == null || string.IsNullOrEmpty(paymentIntent.PaymentId)) { PrintAndReturnError("PaymentId is missing after create for update test."); return null; }
                PrintPaymentDetails("1. After Create (for Update Test)", paymentIntent);

                Console.WriteLine($"\n2. Updating Payment Intent {paymentIntent.PaymentId}...");
                var updateRequest = new PaymentUpdateRequest { Amount = 900, Description = "Updated Description for Payment", Email = "customer-update-final@example.com", Metadata = new Dictionary<string, string> { { "updated_by", "sdk_sample" }, { "reason", "test_update_flow" } }, PaymentMethod = createRequest.PaymentMethod, PaymentMethodType = createRequest.PaymentMethodType, PaymentMethodData = createRequest.PaymentMethodData };
                paymentIntent = await paymentService.UpdatePaymentAsync(paymentIntent.PaymentId!, updateRequest);
                if (paymentIntent == null) { PrintAndReturnError("Payment Update call returned null or failed."); return null; }
                PrintPaymentDetails("2. After Update Attempt", paymentIntent);

                Console.WriteLine($"\n3. Syncing status for {paymentIntent.PaymentId} after update...");
                paymentIntent = await paymentService.SyncPaymentStatusAsync(paymentIntent.PaymentId!, clientSecret: paymentIntent.ClientSecret, forceSync: true);
                PrintPaymentDetails("3. After Sync (post-update)", paymentIntent);
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in update payment flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in update payment flow"); }
            return paymentIntent;
        }

        static async Task<RefundResponse?> TestRefundPaymentAsync(PaymentService paymentService, RefundService refundService, string? paymentIdToRefund)
        {
            Console.WriteLine($"Testing Refund Payment Flow for Payment ID: {paymentIdToRefund ?? "N/A"}");
            if (string.IsNullOrEmpty(paymentIdToRefund))
            {
                PrintAndReturnError("No valid Payment ID provided for refund test.");
                return null;
            }

            PaymentIntentResponse? paymentDetails = null;
            RefundResponse? createdRefund = null;
            try
            {
                Console.WriteLine($"\n1. Syncing status for payment {paymentIdToRefund} to ensure it's succeeded...");
                paymentDetails = await paymentService.SyncPaymentStatusAsync(paymentIdToRefund, clientSecret: null, forceSync: true);
                if (paymentDetails == null) { PrintAndReturnError($"Failed to sync payment {paymentIdToRefund}."); return null; }
                PrintPaymentDetails("1. After Sync (pre-refund)", paymentDetails);

                if (paymentDetails.Status != "succeeded") { PrintAndReturnError($"Payment {paymentIdToRefund} is not 'succeeded' (Status: {paymentDetails.Status}). Cannot test refund."); return null; }

                Console.WriteLine($"\n2. Creating Refund for Payment {paymentDetails.PaymentId}...");
                var refundRequest = new RefundCreateRequest { PaymentId = paymentDetails.PaymentId!, Amount = paymentDetails.Amount / 2, Reason = "OTHER", Metadata = new Dictionary<string, string> { { "test_type", "sdk_refund_test" } } };
                createdRefund = await refundService.CreateRefundAsync(refundRequest);
                if (createdRefund == null || string.IsNullOrEmpty(createdRefund.RefundId)) { PrintAndReturnError("Refund creation returned null or failed to provide RefundId."); return null; }
                PrintRefundDetails("2. After Refund Create", createdRefund);

                Console.WriteLine($"\n3. Retrieving (rsync) Refund {createdRefund.RefundId} (initial check)...");
                RefundResponse? retrievedRefund = await refundService.RetrieveRefundAsync(createdRefund.RefundId!);
                PrintRefundDetails("3. After Initial Refund Retrieve", retrievedRefund);

                int refundSyncAttempts = 0;
                if (retrievedRefund != null && !string.IsNullOrEmpty(retrievedRefund.RefundId))
                {
                    while (retrievedRefund.Status == "pending" || retrievedRefund.Status == "requires_action")
                    {
                        if (refundSyncAttempts >= 3) { Console.WriteLine($"   Refund {retrievedRefund.RefundId} still '{retrievedRefund.Status}' after {refundSyncAttempts} sync attempts. Stopping poll."); break; }
                        refundSyncAttempts++;
                        Console.WriteLine($"\n   Syncing refund {retrievedRefund.RefundId} (attempt {refundSyncAttempts}), current status: {retrievedRefund.Status}...");
                        await Task.Delay(2000);
                        retrievedRefund = await refundService.RetrieveRefundAsync(retrievedRefund.RefundId!);
                        if (retrievedRefund == null) { PrintAndReturnError("RetrieveRefundAsync returned null during polling."); break; }
                        PrintRefundDetails($"   After Refund Sync Attempt {refundSyncAttempts}", retrievedRefund);
                    }
                    if (retrievedRefund != null) { Console.WriteLine($"   Final polled status for refund {retrievedRefund.RefundId}: {retrievedRefund.Status}"); }
                }
                Console.WriteLine($"\n4. Syncing original payment {paymentDetails.PaymentId} again after refund process...");
                PaymentIntentResponse? paymentAfterRefund = await paymentService.SyncPaymentStatusAsync(paymentDetails.PaymentId!, clientSecret: null, forceSync: true);
                PrintPaymentDetails("4. Original Payment Status After Refund Process", paymentAfterRefund);
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in refund payment flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in refund payment flow"); }
            return createdRefund;
        }

        static async Task TestUpdateRefundAsync(RefundService refundService, string refundIdToUpdate)
        {
            Console.WriteLine($"Testing Update Refund Flow for Refund ID: {refundIdToUpdate}");
            try
            {
                Console.WriteLine($"\n1. Retrieving refund {refundIdToUpdate} before update...");
                RefundResponse? refundDetails = await refundService.RetrieveRefundAsync(refundIdToUpdate);
                if (refundDetails == null) { PrintAndReturnError($"Failed to retrieve refund {refundIdToUpdate} before update."); return; }
                PrintRefundDetails("1. Before Update Refund", refundDetails);

                if (refundDetails.Status == "pending" || refundDetails.Status == "requires_action")
                {
                    Console.WriteLine($"\n2. Updating refund {refundIdToUpdate}...");
                    var updateRequest = new RefundUpdateRequest { Reason = "Updated reason via SDK", Metadata = new Dictionary<string, string> { { "update_source", "sdk_sample_test" }, { "initial_reason", refundDetails.Reason ?? "N/A" } } };
                    RefundResponse? updatedRefund = await refundService.UpdateRefundAsync(refundIdToUpdate, updateRequest);
                    if (updatedRefund == null) { PrintAndReturnError("Refund Update call returned null or failed."); return; }
                    PrintRefundDetails("2. After Update Refund Attempt", updatedRefund);
                }
                else { Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine($"\n2. Refund status is '{refundDetails.Status}'. Update not typically allowed/meaningful. Skipping update attempt."); Console.ResetColor(); }
                return;
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in update refund flow"); return; }
            catch (Exception ex) { PrintGenericError(ex, "in update refund flow"); return; }
        }

        static async Task TestListRefundsAsync(RefundService refundService, string? paymentIdToList = null, int? limit = null)
        {
            if (!string.IsNullOrEmpty(paymentIdToList)) { Console.WriteLine($"Testing List Refunds Flow for Payment ID: {paymentIdToList}"); }
            else { Console.WriteLine($"Testing List Refunds Flow (general list with limit {limit?.ToString() ?? "default"})"); }

            try
            {
                var listRequest = new RefundListRequest();
                if (!string.IsNullOrEmpty(paymentIdToList)) { listRequest.PaymentId = paymentIdToList; }
                if (limit.HasValue) { listRequest.Limit = limit; }

                Console.WriteLine("\n1. Listing refunds...");
                RefundListResponse? refunds = await refundService.ListRefundsAsync(listRequest);

                if (refunds == null) { PrintAndReturnError("List Refunds call returned null."); return; }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Found {refunds.Data?.Count ?? 0} refund(s). (API reported: Count={refunds.Count}, TotalCount={refunds.TotalCount})");
                Console.ResetColor();
                int count = 0;
                if (refunds.Data != null)
                {
                    foreach (var refund in refunds.Data)
                    {
                        PrintRefundDetails($"Listed Refund {++count}", refund);
                    }
                }
                if (refunds.Data == null || refunds.Data.Count == 0) { Console.WriteLine("No refunds found matching criteria in the current list page."); }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in list refunds flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in list refunds flow"); }
        }

        static async Task TestListCustomerPaymentMethodsAsync(CustomerService customerService, string customerId)
        {
            Console.WriteLine($"Testing List Customer Payment Methods for Customer ID: {customerId}");
            if (string.IsNullOrEmpty(customerId))
            {
                PrintAndReturnError("Customer ID is required to list payment methods.");
                return;
            }
            try
            {
                Console.WriteLine("\n1. Listing customer payment methods...");
                CustomerPaymentMethodListResponse? response = await customerService.ListPaymentMethodsAsync(customerId);

                if (response == null) { PrintAndReturnError("List Customer Payment Methods call returned null."); return; }

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Customer ID: {response.CustomerId}");
                Console.WriteLine($"Payment Method Count: {response.PaymentMethodCount}");
                Console.ResetColor();

                if (response.Data != null && response.Data.Any())
                {
                    int count = 0;
                    foreach (var pm in response.Data)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine($"  Payment Method {++count}:");
                        Console.WriteLine($"    Token: {pm.PaymentToken}");
                        Console.WriteLine($"    Method: {pm.PaymentMethod}, Type: {pm.PaymentMethodType}, Issuer: {pm.PaymentMethodIssuer}");
                        Console.WriteLine($"    Recurring: {pm.RecurringEnabled}, Installment: {pm.InstallmentPaymentEnabled}");
                        Console.WriteLine($"    Created: {pm.Created}");
                        if (pm.Card != null)
                        {
                            Console.WriteLine($"    Card: Last4={pm.Card.Last4Digits}, Expires={pm.Card.ExpiryMonth}/{pm.Card.ExpiryYear}");
                        }
                        if (pm.Metadata != null && pm.Metadata.Any())
                        {
                            Console.WriteLine("    Metadata:");
                            foreach (var meta in pm.Metadata) { Console.WriteLine($"      {meta.Key}: {meta.Value}"); }
                        }
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("No saved payment methods found for this customer.");
                }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in list customer payment methods flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in list customer payment methods flow"); }
        }

        static async Task<string?> TestCreateCustomerAsync(CustomerService customerService)
        {
            Console.WriteLine("Attempting to create a new customer...");
            var customerRequest = new CustomerRequest
            {
                Email = $"test.customer.{Guid.NewGuid().ToString().Substring(0, 8)}@example.com",
                Name = "Test SDK Customer",
                Phone = "9876543210",
                PhoneCountryCode = "+91",
                Description = "Customer created via SDK sample for testing.",
                Address = new AddressDetails
                {
                    Line1 = "123 SDK Test St",
                    City = "Testville",
                    Country = "US",
                    Zip = "12345",
                    FirstName = "Test",
                    LastName = "Customer"
                },
                Metadata = new Dictionary<string, string> { { "sdk_test_run", DateTime.UtcNow.ToString("o") } }
            };

            try
            {
                CustomerResponse? response = await customerService.CreateCustomerAsync(customerRequest);
                if (response != null && !string.IsNullOrEmpty(response.CustomerId))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Customer created successfully! ID: {response.CustomerId}, Name: {response.Name}, Email: {response.Email}");
                    Console.ResetColor();
                    PrintCustomerDetails("Created Customer", response);
                    return response.CustomerId;
                }
                else
                {
                    PrintAndReturnError("Customer creation failed or did not return a CustomerId.");
                    return null;
                }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in create customer flow"); return null; }
            catch (Exception ex) { PrintGenericError(ex, "in create customer flow"); return null; }
        }

        static async Task TestRetrieveCustomerAsync(CustomerService customerService, string customerId)
        {
            Console.WriteLine($"Attempting to retrieve customer with ID: {customerId}");
            try
            {
                CustomerResponse? response = await customerService.RetrieveCustomerAsync(customerId);
                if (response != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Customer retrieved successfully! ID: {response.CustomerId}, Name: {response.Name}, Email: {response.Email}");
                    Console.ResetColor();
                    PrintCustomerDetails("Retrieved Customer", response);
                }
                else
                {
                    PrintAndReturnError($"Retrieve customer returned null for ID: {customerId}. This might indicate not found if API returns null on 404 for GET.");
                }
            }
            catch (HyperswitchApiException apiEx)
            {
                if (apiEx.StatusCode == 404) PrintAndReturnError($"Customer with ID {customerId} not found (404).");
                else PrintApiError(apiEx, $"in retrieve customer flow for ID: {customerId}");
            }
            catch (Exception ex) { PrintGenericError(ex, $"in retrieve customer flow for ID: {customerId}"); }
        }

        static async Task TestUpdateCustomerAsync(CustomerService customerService, string customerId)
        {
            Console.WriteLine($"Attempting to update customer with ID: {customerId}");
            var updateRequest = new CustomerUpdateRequest
            {
                Name = "Test SDK Customer (Updated)",
                Description = "Customer details updated via SDK sample.",
                Metadata = new Dictionary<string, string> { { "update_timestamp", DateTime.UtcNow.ToString("o") } }
            };
            try
            {
                CustomerResponse? response = await customerService.UpdateCustomerAsync(customerId, updateRequest);
                if (response != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Customer updated successfully! ID: {response.CustomerId}, New Name: {response.Name}, New Desc: {response.Description}");
                    Console.ResetColor();
                    PrintCustomerDetails("Updated Customer", response);
                }
                else
                {
                    PrintAndReturnError($"Update customer returned null for ID: {customerId}.");
                }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in update customer flow for ID: {customerId}"); }
            catch (Exception ex) { PrintGenericError(ex, $"in update customer flow for ID: {customerId}"); }
        }

        static async Task TestListCustomersAsync(CustomerService customerService)
        {
            Console.WriteLine("Attempting to list customers...");
            try
            {
                var listRequest = new CustomerListRequest { Limit = 10 };
                List<CustomerResponse>? customers = await customerService.ListCustomersAsync(listRequest);

                if (customers != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Retrieved {customers.Count} customer(s).");
                    Console.ResetColor();
                    int i = 0;
                    foreach (var cust in customers)
                    {
                        PrintCustomerDetails($"Listed Customer {++i}", cust);
                        if (i >= 5 && customers.Count > 5)
                        {
                            Console.WriteLine($"... and {customers.Count - 5} more customers.");
                            break;
                        }
                    }
                    if (customers.Count == 0) Console.WriteLine("No customers found.");
                }
                else
                {
                    PrintAndReturnError("List customers returned null.");
                }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, "in list customers flow"); }
            catch (Exception ex) { PrintGenericError(ex, "in list customers flow"); }
        }

        // Modified to be self-contained for testing delete functionality
        static async Task TestDeleteCustomerAsync(CustomerService customerService)
        {
            Console.WriteLine("Attempting to create a temporary customer for deletion test...");
            string? customerIdToDelete = await TestCreateCustomerAsync(customerService); // Create a fresh customer

            if (string.IsNullOrEmpty(customerIdToDelete))
            {
                PrintAndReturnError("Failed to create temporary customer for deletion test. Skipping delete test.");
                return;
            }

            Console.WriteLine($"Attempting to delete temporary customer with ID: {customerIdToDelete}");
            try
            {
                CustomerDeleteResponse? response = await customerService.DeleteCustomerAsync(customerIdToDelete);
                if (response != null && response.CustomerDeleted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Temporary customer with ID {response.CustomerId} deleted successfully. Payment methods deleted: {response.PaymentMethodsDeleted}");
                    Console.ResetColor();
                }
                else
                {
                    PrintAndReturnError($"Failed to delete temporary customer with ID {customerIdToDelete}. Response: Deleted={response?.CustomerDeleted}, ID={response?.CustomerId}");
                }
            }
            catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in delete customer flow for temporary ID: {customerIdToDelete}"); }
            catch (Exception ex) { PrintGenericError(ex, $"in delete customer flow for temporary ID: {customerIdToDelete}"); }

            Console.WriteLine($"\nVerifying deletion by attempting to retrieve temporary customer ID: {customerIdToDelete}");
            await TestRetrieveCustomerAsync(customerService, customerIdToDelete); // Verify deletion
        }

        static async Task TestMandatePaymentFlowAsync(
            PaymentService paymentService,
            CustomerService customerService,
            string? customerIdForInitialNewSetup) // Nullable if customer creation failed earlier in Main
        {
            Console.WriteLine($"\n--- Testing Mandate Payment Flow ---");
            PaymentIntentResponse? newCitPayment = null;
            // string? newCitPaymentMethodToken = null; // Not strictly needed if not used later

            // Part A: Attempt to set up a new mandate (for demonstration)
            Console.WriteLine("\nPart A: Attempting Initial Mandate Setup Payment (CIT Demo)...");
            if (!string.IsNullOrEmpty(customerIdForInitialNewSetup))
            {
                try
                {
                    Console.WriteLine($"  Using Customer ID for new CIT Demo: {customerIdForInitialNewSetup}");
                    var initialPaymentRequest = new PaymentIntentRequest
                    {
                        Amount = 1000,
                        Currency = "USD",
                        Confirm = true,
                        CaptureMethod = "automatic",
                        CustomerId = "cusStripeTest4", // pass your customer id here for CIT
                        SetupFutureUsage = "off_session",
                        CustomerAcceptance = new Models.CustomerAcceptance
                        {
                            AcceptanceType = "offline"
                        },
                        Description = "Initial payment for mandate setup demo",
                        PaymentMethod = "card",
                        PaymentMethodType = "credit",
                        PaymentMethodData = new PaymentMethodData
                        {
                            Card = new CardDetails { CardNumber = "4111111111111111", CardExpiryMonth = "03", CardExpiryYear = "2030", CardCvc = "737" }
                        },
                        AuthenticationType = "no_three_ds",
                        ReturnUrl = "https://example.com/mandate_return_demo",
                        BrowserInfo = new BrowserInfo
                        {
                            AcceptHeader = "application/json, text/plain, */*",
                            UserAgent = "Hyperswitch .NET SDK Sample CIT Demo v1.0",
                            IpAddress = "127.0.0.1",
                            Language = "en-US",
                            ScreenHeight = 1080,
                            ScreenWidth = 1920,
                            ColorDepth = 24,
                            JavaEnabled = true,
                            JavaScriptEnabled = true,
                            TimeZone = -330
                        }
                    };

                    newCitPayment = await paymentService.CreateAsync(initialPaymentRequest);
                    if (newCitPayment != null) PrintPaymentDetails("  New CIT Demo - Initial Response", newCitPayment);
                    else PrintAndReturnError("  New CIT Demo - Payment creation returned null.");

                    if (newCitPayment != null && (newCitPayment.Status == "requires_customer_action" || newCitPayment.Status == "processing" || newCitPayment.Status == "requires_payment_method" || newCitPayment.Status == "requires_confirmation"))
                    {
                        Console.WriteLine($"    New CIT Demo - Payment {newCitPayment.PaymentId} requires further action (Status: {newCitPayment.Status}). This is expected for demo.");
                    }
                    Console.WriteLine($"    New CIT Demo - PaymentMethodId from this attempt (not used for MIT test): {newCitPayment?.PaymentMethodId ?? "N/A"}");
                }
                catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in New CIT Demo for customer {customerIdForInitialNewSetup}"); }
                catch (Exception ex) { PrintGenericError(ex, $"in New CIT Demo for customer {customerIdForInitialNewSetup}"); }
            }
            else
            {
                Console.WriteLine("  Skipping New CIT Demo part as customerIdForInitialNewSetup was not provided (likely main customer creation failed earlier).");
            }

            // Part B: Test Subsequent Mandate Payment (MIT) using data from the New CIT Demo
            Console.WriteLine($"\nPart B: Attempting Subsequent Mandate Payment (MIT) using data from New CIT Demo (Part A)...");

            if (newCitPayment != null &&
                newCitPayment.Status == "succeeded" &&
                !string.IsNullOrEmpty(newCitPayment.PaymentMethodId) &&
                !string.IsNullOrEmpty(newCitPayment.CustomerId))
            {
                Console.WriteLine($"  New CIT (Payment ID: {newCitPayment.PaymentId}) was successful and contains required data for MIT.");
                Console.WriteLine($"  Using PaymentMethodId: {newCitPayment.PaymentMethodId} and CustomerId: {newCitPayment.CustomerId} for MIT.");

                string? dynamicMitPaymentMethodToken = newCitPayment.PaymentMethodId;
                string? dynamicMitCustomerId = newCitPayment.CustomerId;

                try
                {
                    Console.WriteLine("\n  1. MIT Test - Creating subsequent payment using the saved payment method token from New CIT Demo...");
                    var subsequentPaymentRequest = new PaymentIntentRequest
                    {
                        Amount = newCitPayment.Amount,
                        Currency = "USD",
                        Confirm = true,
                        CustomerId = dynamicMitCustomerId, // Use customerId from newCitPayment
                        OffSession = true,
                        RecurringDetails = new RecurringDetailsInfo
                        {
                            Type = "payment_method_id", 
                            Data = dynamicMitPaymentMethodToken // Use paymentMethodId from newCitPayment
                        },
                        Description = "Subsequent Mandate Test Payment (MIT from New CIT Demo)"
                    };

                    PaymentIntentResponse? subsequentPayment = await paymentService.CreateAsync(subsequentPaymentRequest);
                    if (subsequentPayment == null || string.IsNullOrEmpty(subsequentPayment.PaymentId))
                    {
                        PrintAndReturnError("  MIT Test (New CIT) - Subsequent mandate payment creation failed.");
                        return; 
                    }
                    PrintPaymentDetails("  1. MIT Test (New CIT) - Subsequent Mandate Payment Response", subsequentPayment);

                    int attempts = 0;
                    while (subsequentPayment.Status == "requires_customer_action" || 
                           subsequentPayment.Status == "processing" || 
                           subsequentPayment.Status == "requires_payment_method" || 
                           subsequentPayment.Status == "requires_confirmation")
                    {
                        if (attempts >= 3) 
                        { 
                            Console.ForegroundColor = ConsoleColor.Yellow; 
                            Console.WriteLine($"    MIT Test (New CIT) - Subsequent payment {subsequentPayment.PaymentId} did not reach a final state after {attempts} syncs. Current status: {subsequentPayment.Status}."); 
                            Console.ResetColor(); 
                            break; 
                        }
                        Console.WriteLine($"\n    MIT Test (New CIT) - Syncing subsequent payment {subsequentPayment.PaymentId} (attempt {++attempts}), current status: {subsequentPayment.Status}...");
                        await Task.Delay(2000); 
                        subsequentPayment = await paymentService.SyncPaymentStatusAsync(subsequentPayment.PaymentId!, clientSecret: subsequentPayment.ClientSecret, forceSync: true);
                        if (subsequentPayment == null) 
                        { 
                            PrintAndReturnError("    MIT Test (New CIT) - Sync returned null for subsequent payment."); 
                            return; 
                        }
                        PrintPaymentDetails($"    MIT Test (New CIT) - After Subsequent Payment Sync Attempt {attempts}", subsequentPayment);
                    }
                    Console.WriteLine($"  MIT Test (New CIT) - Final status for subsequent mandate payment {subsequentPayment?.PaymentId}: {subsequentPayment?.Status}");
                }
                catch (HyperswitchApiException apiEx) { PrintApiError(apiEx, $"in MIT Test part (New CIT) for customer {dynamicMitCustomerId}"); }
                catch (Exception ex) { PrintGenericError(ex, $"in MIT Test part (New CIT) for customer {dynamicMitCustomerId}"); }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("  Skipping MIT Test using New CIT Demo data due to one or more of the following reasons:");
                if (newCitPayment == null) 
                {
                    Console.WriteLine("    - New CIT Payment object (from Part A) is null (Part A likely failed, was skipped, or did not produce a payment object).");
                }
                else
                {
                    if (newCitPayment.Status != "succeeded") Console.WriteLine($"    - New CIT Payment status is '{newCitPayment.Status}' (expected 'succeeded'). Payment ID: {newCitPayment.PaymentId ?? "N/A"}");
                    if (string.IsNullOrEmpty(newCitPayment.PaymentMethodId)) Console.WriteLine($"    - New CIT PaymentMethodId is missing. Payment ID: {newCitPayment.PaymentId ?? "N/A"}");
                    if (string.IsNullOrEmpty(newCitPayment.CustomerId)) Console.WriteLine($"    - New CIT CustomerId is missing. Payment ID: {newCitPayment.PaymentId ?? "N/A"}");
                }
                Console.ResetColor();
                Console.WriteLine($"  NOTE: The dynamic MIT test using data from the Part A CIT was skipped due to the reasons above.");
            }

        }


        static void PrintCustomerDetails(string stage, CustomerResponse? customer)
        {
            if (customer == null) { PrintAndReturnError($"{stage}: Customer details are null."); return; }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"{stage}:");
            Console.WriteLine($"  ID: {customer.CustomerId}");
            Console.WriteLine($"  Name: {customer.Name}");
            Console.WriteLine($"  Email: {customer.Email}");
            Console.WriteLine($"  Phone: {customer.PhoneCountryCode}{customer.Phone}");
            Console.WriteLine($"  Description: {customer.Description}");
            Console.WriteLine($"  Created At: {customer.CreatedAt}");
            if (customer.Address != null)
            {
                Console.WriteLine($"  Address: {customer.Address.Line1}, {customer.Address.City}, {customer.Address.State} {customer.Address.Zip}, {customer.Address.Country}");
            }
            if (customer.Metadata != null && customer.Metadata.Any())
            {
                Console.WriteLine("  Metadata:");
                foreach (var meta in customer.Metadata) { Console.WriteLine($"    {meta.Key}: {meta.Value}"); }
            }
            Console.ResetColor();
        }

        static void PrintPaymentDetails(string stage, PaymentIntentResponse? payment)
        {
            if (payment == null) { PrintAndReturnError($"{stage}: Payment details are null."); return; }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{stage}:");
            Console.WriteLine($"  ID: {payment.PaymentId}, Status: {payment.Status}");
            Console.WriteLine($"  Amount: {payment.Amount} {payment.Currency}, Capturable: {payment.AmountCapturable}, Received: {payment.AmountReceived}");
            Console.WriteLine($"  Client Secret: {payment.ClientSecret}");
            if (!string.IsNullOrEmpty(payment.NextAction?.RedirectToUrl)) { Console.WriteLine($"  Next Action URL: {payment.NextAction.RedirectToUrl}"); }
            if (payment.LastPaymentError != null) { Console.WriteLine($"  Last Error: [{payment.LastPaymentError.Code}] {payment.LastPaymentError.Message}"); }
            Console.ResetColor();
        }

        static void PrintRefundDetails(string stage, RefundResponse? refund)
        {
            if (refund == null) { PrintAndReturnError($"{stage}: Refund details are null."); return; }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"{stage}:");
            Console.WriteLine($"  Refund ID: {refund.RefundId}, Payment ID: {refund.PaymentId}");
            Console.WriteLine($"  Amount: {refund.Amount} {refund.Currency}, Status: {refund.Status}");
            Console.WriteLine($"  Reason: {refund.Reason}, CreatedAt: {refund.CreatedAt}");
            if (!string.IsNullOrEmpty(refund.ErrorCode)) { Console.WriteLine($"  Error: [{refund.ErrorCode}] {refund.ErrorMessage}"); }
            Console.ResetColor();
        }

        static void PrintAndReturnError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        static void PrintApiError(HyperswitchApiException apiEx, string flowContext)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Hyperswitch API Error {flowContext}:");
            Console.WriteLine($"  Status Code: {apiEx.StatusCode}");
            Console.WriteLine($"  Message: {apiEx.Message}");
            if (apiEx.ErrorDetails?.Error != null) { Console.WriteLine($"  Error Code: {apiEx.ErrorDetails.Error.Code}, Msg: {apiEx.ErrorDetails.Error.Message}"); }
            Console.WriteLine($"  Raw Response: {apiEx.ResponseContent}");
            Console.ResetColor();
        }
        static void PrintGenericError(Exception ex, string flowContext)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An unexpected error occurred {flowContext}: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.ResetColor();
        }
    }
}
