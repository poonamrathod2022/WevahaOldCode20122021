using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEvaha.Models;

namespace WEvaha.Controllers
{
    public class PaypalController : Controller
    {
        DashBoardController dc = new DashBoardController();
        // GET: Paypal
        public ActionResult Index(int PackageId, string PackageName, string PackagePrice)
        {
            TempData["UserId"] = Convert.ToInt32(Session["UserId"]); 
            TempData["PackageId"]= PackageId;
            TempData["PackageName"] = PackageName;
            TempData["PackagePrice"] = PackagePrice;

            return RedirectToAction("PaymentWithPaypal", "Paypal");
        }
        
        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist

                    //it is returned by the create function call of the payment class

                    // Creating a payment

                    // baseURL is the url on which paypal sendsback the data.

                    // So we have provided URL of this controller only

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session

                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url

                    //on which payer is redirected for paypal acccount payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                   
                    if (executedPayment.state.ToLower() != "approved")
                    {
                       // dc.InsertUserPackage(ProfileId, TempData["PackageId"], Request.Form["txnid"], "PayU");
                        return View("FailureView");
                    }
                    else
                    {
                        string Description = executedPayment.transactions[0].description;
                        string Invoice_number = executedPayment.transactions[0].invoice_number;
                        string TranxId= executedPayment.transactions[0].related_resources[0].sale.id;
                        ViewBag.TranxId = TranxId;
                        ViewBag.Invoice_number = Invoice_number;
                        string[] ssplitString = Description.Split(',');
                        int PackageId = Convert.ToInt16(ssplitString[1].Trim());
                        int ProfileId = Convert.ToInt16(ssplitString[2].Trim());
                        // here we are unable to get trasaction id thats why i am placing invoice number
                        dc.InsertUserPackage(ProfileId, PackageId, TranxId, "PayPal");
                        return View("SuccessView");
                    }

                }
            }

            catch (Exception ex)
            {
                Logger.Log("Error" + ex.Message);
                return View("FailureView");
            }

            
        }

        private PayPal.Api.Payment payment;

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList() { items = new List<Item>() };
            // ViewBag.UserId = UserId;
            int PackageId=0; int UserId=0; string PackageName= string.Empty;string PackagePrice = string.Empty;
            //int PackageId= Convert.ToInt16(TempData["PackageId"]);
            //string PackageName = Convert.ToString(TempData["PackageName"]);
            //string PackagePrice= Convert.ToString(TempData["PackagePrice"]);


            if (TempData.ContainsKey("PackageId"))
                PackageId = Convert.ToInt16(TempData["PackageId"]);
            TempData.Keep("PackageId");

            if (TempData.ContainsKey("PackageName"))
                PackageName = TempData["PackageName"] as string;
            TempData.Keep("PackageName");

            if (TempData.ContainsKey("PackagePrice"))
                PackagePrice = TempData["PackagePrice"] as string;
            TempData.Keep("PackagePrice");

            if (TempData.ContainsKey("UserId"))
                UserId = Convert.ToInt16(TempData["UserId"]);
            TempData.Keep("UserId");
           
           itemList.items.Add(new Item()
            {
                name = PackageName,
                currency = "USD",
                price = PackagePrice,
                quantity = "1",
                sku = "sku"
            });

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = null,
                shipping = null,
                subtotal = null
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = PackagePrice, // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();

            transactionList.Add(new Transaction()
            {
                description = "Wevaha_Package_"+ PackageName+","+ PackageId + "," + Convert.ToInt16(UserId),
                invoice_number = "Wevaha_Invoice_"+ Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }
    }
}