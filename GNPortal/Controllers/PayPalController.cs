using GenomeNext.Billing;
using GenomeNext.Data.EntityModel;
using GenomeNext.Utility;
using PayPal.PayPalAPIInterfaceService;
using PayPal.PayPalAPIInterfaceService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GenomeNext.Portal.Controllers
{
    public class PayPalController : BaseController
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: PayPal/MakeInvoicePayment
        public async Task<ActionResult> MakeInvoicePayment(Guid invoiceId)
        {
            InvoiceService invoiceService = new InvoiceService(this.db);
            GNInvoice invoice = await invoiceService.Find(invoiceId);
            double paymentAmount = invoice.Balance;
            string cancelUrl = Url.Action("MyBillingBillDetail", "Account", new { id = invoice.Id }, protocol: GetURLScheme());
            RedirectToRouteResult errorRedirectAction = RedirectToAction("MyBillingBillDetail", "Account", new
            {
                id = invoice.Id
            });

            return await DoMakePayment(invoice, paymentAmount, cancelUrl, errorRedirectAction);
        }

        // POST: PayPal/MakePayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MakePayment(double paymentAmount)
        {
            double minPurchaseAmount = 0.0;

            var myProducts = this.db.GNProducts
                .Where(p => (p.AccountType.Id == UserContact.GNOrganization.Account.AccountType.Id && p.ProductType.Name != "STORAGE"));

            if(myProducts != null && myProducts.Count() != 0)
            {
                minPurchaseAmount = myProducts.Min(p => p.Price);
            }

            RedirectToRouteResult errorRedirectAction = RedirectToAction("MyBillingBuyCredits", "Account", new
            {
                paymentAmount = paymentAmount
            });

            if (paymentAmount >= minPurchaseAmount)
            {
                InvoiceService invoiceService = new InvoiceService(this.db);
                GNInvoice invoice = await invoiceService.GetInvoiceForCurrentMonth(UserContact);
                string cancelUrl = Url.Action("MyBillingBuyCredits", "Account", new { paymentAmount = paymentAmount }, protocol: GetURLScheme());

                return await DoMakePayment(invoice, paymentAmount, cancelUrl, errorRedirectAction);
            }
            else
            {
                errorRedirectAction.RouteValues["error"] = "Purchase Amount must be greater than or equal to $" + minPurchaseAmount;
                return errorRedirectAction;
            }
        }

        private async Task<ActionResult> DoMakePayment(GNInvoice invoice, double paymentAmount, string cancelURL, RedirectToRouteResult errorRedirectAction)
        {
            SetExpressCheckoutResponseType ecResponse = null;

            try
            {
                CurrencyCodeType currencyUSD = (CurrencyCodeType)EnumUtils.GetValue("USD", typeof(CurrencyCodeType));

                //define payment details
                List<PaymentDetailsType> paymentDetails = new List<PaymentDetailsType>()
                {
                    new PaymentDetailsType
                    {
                        PaymentDetailsItem = new List<PaymentDetailsItemType>()
                        {
                            new PaymentDetailsItemType
                            {
                                Name = invoice.Name,
                                Amount = new BasicAmountType(currencyUSD, Math.Round(paymentAmount,2) + ""),
                                Quantity = 1,
                                ItemCategory = (ItemCategoryType)EnumUtils.GetValue("Physical", typeof(ItemCategoryType))
                            }
                        },
                        PaymentAction = (PaymentActionCodeType)EnumUtils.GetValue("Sale", typeof(PaymentActionCodeType)),
                        OrderTotal = new BasicAmountType(currencyUSD, (Math.Round(paymentAmount,2) * 1) + "")
                    }
                };

                //define checkout request details
                SetExpressCheckoutReq expressCheckoutRequest = new SetExpressCheckoutReq()
                {
                    SetExpressCheckoutRequest = new SetExpressCheckoutRequestType()
                    {
                        Version = "104.0",
                        SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType()
                        {
                            ReturnURL = Url.Action("PaymentSuccess", "PayPal", new { invoiceId = invoice.Id }, protocol: GetURLScheme()),
                            CancelURL = cancelURL,
                            PaymentDetails = paymentDetails
                        }
                    }
                };

                //define sdk configuration
                Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
                sdkConfig.Add("mode", ConfigurationManager.AppSettings["paypal.mode"]);
                sdkConfig.Add("account1.apiUsername", ConfigurationManager.AppSettings["paypal.apiUsername"]);
                sdkConfig.Add("account1.apiPassword", ConfigurationManager.AppSettings["paypal.apiPassword"]);
                sdkConfig.Add("account1.apiSignature", ConfigurationManager.AppSettings["paypal.apiSignature"]);

                //instantiate PayPal API service and send Express Checkout Request
                PayPalAPIInterfaceServiceService service = new PayPalAPIInterfaceServiceService(sdkConfig);
                ecResponse = service.SetExpressCheckout(expressCheckoutRequest);

            }
            catch (Exception e)
            {
                LogUtil.Error(logger, "Error sending PayPal payment!!", e);
            }

            if (ecResponse != null 
                && ecResponse.Ack.HasValue
                && ecResponse.Ack.Value.ToString() != "FAILURE"
                && ecResponse.Errors.Count == 0)
            {
                //redirect to PayPal
                if (ConfigurationManager.AppSettings["paypal.mode"] == "live")
                {
                    string paypalURL = "https://www.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + ecResponse.Token;
                    return RedirectPermanent(paypalURL);
                }
                else
                {
                    string paypalURL = "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_express-checkout&token=" + ecResponse.Token;
                    return RedirectPermanent(paypalURL);
                }
            }
            else
            {
                errorRedirectAction.RouteValues["error"] = string.Join(",", ecResponse.Errors.Select(e => e.LongMessage).ToArray());
                return errorRedirectAction;
            }
        }

        //GET : PayPal/PaymentSuccess
        public async Task<ActionResult> PaymentSuccess(Guid invoiceId)
        {
            GetExpressCheckoutDetailsRequestType request = new GetExpressCheckoutDetailsRequestType();
            request.Version = "104.0";
            request.Token = Request["token"];
            GetExpressCheckoutDetailsReq wrapper = new GetExpressCheckoutDetailsReq();
            wrapper.GetExpressCheckoutDetailsRequest = request;

            //define sdk configuration
            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            sdkConfig.Add("mode", ConfigurationManager.AppSettings["paypal.mode"]);
            sdkConfig.Add("account1.apiUsername", ConfigurationManager.AppSettings["paypal.apiUsername"]);
            sdkConfig.Add("account1.apiPassword", ConfigurationManager.AppSettings["paypal.apiPassword"]);
            sdkConfig.Add("account1.apiSignature", ConfigurationManager.AppSettings["paypal.apiSignature"]);
//            sdkConfig.Add("acct1.UserName", ConfigurationManager.AppSettings["paypal.apiUsername"]);
//            sdkConfig.Add("acct1.Password", ConfigurationManager.AppSettings["paypal.apiPassword"]);
//            sdkConfig.Add("acct1.Signature", ConfigurationManager.AppSettings["paypal.apiSignature"]);

            PayPalAPIInterfaceServiceService s = new PayPalAPIInterfaceServiceService(sdkConfig);
            GetExpressCheckoutDetailsResponseType ecResponse = s.GetExpressCheckoutDetails(wrapper);

            if (ecResponse.Ack.HasValue
                && ecResponse.Ack.Value.ToString() != "FAILURE"
                && ecResponse.Errors.Count == 0)
            {
                double paymentAmount = 0.0;
                double.TryParse(ecResponse.GetExpressCheckoutDetailsResponseDetails.PaymentDetails.FirstOrDefault().OrderTotal.value, out paymentAmount);

                PaymentDetailsType paymentDetail = new PaymentDetailsType();
                paymentDetail.NotifyURL = "http://replaceIpnUrl.com";
                paymentDetail.PaymentAction = (PaymentActionCodeType)EnumUtils.GetValue("Sale", typeof(PaymentActionCodeType));
                paymentDetail.OrderTotal = new BasicAmountType((CurrencyCodeType)EnumUtils.GetValue("USD", typeof(CurrencyCodeType)), paymentAmount + "");
                List<PaymentDetailsType> paymentDetails = new List<PaymentDetailsType>();
                paymentDetails.Add(paymentDetail);

                DoExpressCheckoutPaymentRequestType doExpressCheckoutPaymentRequestType = new DoExpressCheckoutPaymentRequestType();
                request.Version = "104.0";
                DoExpressCheckoutPaymentRequestDetailsType requestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
                requestDetails.PaymentDetails = paymentDetails;
                requestDetails.Token = Request["token"];
                requestDetails.PayerID = Request["PayerID"];
                doExpressCheckoutPaymentRequestType.DoExpressCheckoutPaymentRequestDetails = requestDetails;

                DoExpressCheckoutPaymentReq doExpressCheckoutPaymentReq = new DoExpressCheckoutPaymentReq();
                doExpressCheckoutPaymentReq.DoExpressCheckoutPaymentRequest = doExpressCheckoutPaymentRequestType;

                s = new PayPalAPIInterfaceServiceService(sdkConfig);
                DoExpressCheckoutPaymentResponseType doECResponse = s.DoExpressCheckoutPayment(doExpressCheckoutPaymentReq);

                if (doECResponse.Ack.HasValue
                    && doECResponse.Ack.Value.ToString() != "FAILURE"
                    && doECResponse.Errors.Count == 0)
                {

                    //create payment object for invoice
                    PaymentService paymentService = new PaymentService(this.db);
                    PaymentMethodService paymentMethodService = new PaymentMethodService(this.db);
                    var ccPymtMethod = paymentMethodService.GetCreditCardPaymentMethod(UserContact);

                    if (ccPymtMethod == null)
                    {
                        try
                        {
                            PaymentMethodTypeService paymentMethodTypeService = new PaymentMethodTypeService(this.db);
                            string ccTypeCode = GNPaymentMethodType.Types.CREDIT_CARD.GetCode();
                            var ccType = this.db.GNPaymentMethodTypes
                                    .Where(pt => pt.Name == ccTypeCode).FirstOrDefault();

                            await paymentMethodService.Insert(UserContact, new GNPaymentMethod
                            {
                                GNAccountId = UserContact.GNOrganization.Account.Id,
                                GNPaymentMethodTypeId = ccType.Id,
                                Description = "PAYPAL",
                                IsDefault = true,
                                IsActive = true,
                                UsedForRecurrentPayments = false,
                                PCITokenId = "X",
                                LastFourDigits = "X",
                                ExpirationDate = DateTime.MaxValue,
                                CreateDateTime = DateTime.Now,
                                CreatedBy = UserContact.Id
                            });

                        }
                        catch (Exception e)
                        {
                            LogUtil.Error(logger, "Error adding PayPal Credit Card payment method!!", e);
                        }

                        ccPymtMethod = paymentMethodService.GetCreditCardPaymentMethod(UserContact);

                        if (ccPymtMethod == null)
                        {
                            throw new Exception("Credit Card Payment Method Not Found!!");
                        }
                    }

                    var ecPaymentInfo = doECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.FirstOrDefault();

                    double grossAmount = 0.0;
                    double.TryParse(ecPaymentInfo.GrossAmount.value, out grossAmount);

                    if (grossAmount != 0.0)
                    {
                        try
                        {
                            var payment = new GNPayment
                            {
                                Id = Guid.NewGuid(),
                                GNInvoiceId = invoiceId,
                                GNPaymentMethodId = ccPymtMethod.Id,
                                PaymentDate = DateTime.Now,
                                TotalAmount = grossAmount,
                                Status = ecPaymentInfo.PaymentStatus.Value.ToString(),
                                ExternalTxnId = ecPaymentInfo.TransactionID,
                                CreateDateTime = DateTime.Now,
                                CreatedBy = UserContact.Id
                            };

                            this.AddInvoiceToPayment(payment);

                            await paymentService.Insert(UserContact, payment);
                        }
                        catch (Exception e)
                        {
                            LogUtil.Error(logger, "Error inserting payment!!", e);
                        }
                    }
                    else
                    {
                        throw new Exception("Payment Amount of 0.0 Not Allowed!!");
                    }
                }
            }

            return RedirectToAction("MyBillingPayments", "Account");
        }

        private void AddInvoiceToPayment(GNPayment payment)
        {
            if (payment.GNInvoiceId != Guid.Empty)
            {
                payment.Invoices = new List<GNInvoice>();
                payment.Invoices.Add(
                    this.db.GNInvoices.Find(payment.GNInvoiceId));
            }
        }
    }
}