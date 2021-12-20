using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI;
using WE.Utilities;
using WEvaha.Models;

namespace WEvaha.Controllers
{
    public class PayUController : Controller
    {
        public string action1 = string.Empty;
        public string hash1 = string.Empty;
        public string txnid1 = string.Empty;
        public string mKey = string.Empty;
        DashBoardController dc = new DashBoardController();
        //[HttpGet]
        //public ActionResult Pay()
        //{
        //    PaymentDetails Details = new PaymentDetails();
        //    return View();
        //}


        public ActionResult PayUPayment(int PackageId, string PackagePrice, string PackageName)
        {
            PaymentDetails Details = new PaymentDetails();
            
            int ProfileId= Convert.ToInt16(Session["UserId"]);
            var Profdetails= dc.UserProfile(ProfileId);
            Details.Amount =Convert.ToDouble(PackagePrice);
            Details.FirstName = Profdetails.ProfileName+"_"+ ProfileId;
            Details.ProductInfo = PackageName+"_"+ PackageId;
            Details.Email = Profdetails.Email;
            if(string.IsNullOrEmpty(Profdetails.Mobile))
            {
                Details.PhoneNo = "9704335558";
            }
            else
            {
                Details.PhoneNo = Profdetails.Mobile;
            }


            try
            {
                TryUpdateModel(Details);
                if (ModelState.IsValid)
                {
                    Details.key = ConfigurationManager.AppSettings["MERCHANT_KEY"];
                    string[] hashVarsSeq;
                    string hash_string = string.Empty;
                    if (string.IsNullOrEmpty(Details.TxId)) // generating txnid
                    {
                        Random rnd = new Random();
                        string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
                        txnid1 = strHash.ToString().Substring(0, 20);
                    }
                    else
                    {
                        txnid1 = Details.TxId;
                    }
                    if (string.IsNullOrEmpty(Details.Hash))
                    {
                        if (
                            string.IsNullOrEmpty(ConfigurationManager.AppSettings["MERCHANT_KEY"]) ||
                            string.IsNullOrEmpty(txnid1) ||
                            Details.Amount <= 0 ||
                            string.IsNullOrEmpty(Details.FirstName) ||
                            string.IsNullOrEmpty(Details.Email) ||
                           // string.IsNullOrEmpty(Details.PhoneNo) ||
                            string.IsNullOrEmpty(Details.ProductInfo)
                            )
                        {
                            return View(Details);
                        }
                        else
                        {
                            hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                            hash_string = "";
                            foreach (string hash_var in hashVarsSeq)
                            {
                                if (hash_var == "key")
                                {
                                    hash_string = hash_string + ConfigurationManager.AppSettings["MERCHANT_KEY"];
                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "txnid")
                                {
                                    hash_string = hash_string + txnid1;
                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "amount")
                                {
                                    hash_string = hash_string + Convert.ToDecimal(Details.Amount).ToString("g29");
                                    hash_string = hash_string + '|';
                                }

                                else if (hash_var == "productinfo")
                                {
                                    hash_string = hash_string + (Details.ProductInfo != null ? Details.ProductInfo : "");// isset if else
                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "firstname")
                                {
                                    hash_string = hash_string + (Details.FirstName != null ? Details.FirstName : "");// isset if else
                                    hash_string = hash_string + '|';
                                }
                                else if(hash_var == "email")
                                {
                                    hash_string = hash_string + (Details.Email != null ? Details.Email : "");// isset if else
                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf1")
                                {
                                   
                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf2")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf3")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf4")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf5")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf6")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf7")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf8")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf9")
                                {

                                    hash_string = hash_string + '|';
                                }
                                else if (hash_var == "udf10")
                                {

                                    hash_string = hash_string + '|';
                                }

                            }

                            hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                            hash1 = Generatehash512(hash_string).ToLower();         //generating hash
                            action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Details.Hash))
                {
                    hash1 = Details.Hash;
                    action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";
                }

                if (!string.IsNullOrEmpty(hash1))
                {
                    Details.Hash = hash1;
                    Details.TxId = txnid1;

                    System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                    data.Add("hash", Details.Hash);
                    data.Add("txnid", Details.TxId);
                    data.Add("key", Details.key);
                    string AmountForm = Convert.ToDecimal(Details.Amount).ToString("g29");// eliminating trailing zeros
                    //amount.Text = AmountForm;
                    data.Add("amount", AmountForm);
                    data.Add("firstname", Details.FirstName.Trim());
                    data.Add("email", Details.Email.Trim());
                    data.Add("phone", Details.PhoneNo.Trim());
                    data.Add("productinfo", Details.ProductInfo.Trim());
                    data.Add("surl", "https://www.wevaha.com/PayU/PaymentResponse/".Trim());
                    data.Add("furl", "https://www.wevaha.com/PayU/PaymentResponse/".Trim());
                    data.Add("lastname", "".Trim());
                    data.Add("curl", "".Trim());
                    data.Add("address1", "".Trim());
                    data.Add("address2", "".Trim());
                    data.Add("city", "".Trim());
                    data.Add("state", "".Trim());
                    data.Add("country", "".Trim());
                    data.Add("zipcode", "".Trim());
                    data.Add("udf1", "".Trim());
                    data.Add("udf2", "".Trim());
                    data.Add("udf3", "".Trim());
                    data.Add("udf4", "".Trim());
                    data.Add("udf5", "".Trim());
                    data.Add("pg", "".Trim());
                    List<MvcHtmlString> lstMvcHtmlString = PreparePOSTForm(action1, data);
                     return View(lstMvcHtmlString);
                }

                else
                {
                    //no hash

                }

                return RedirectToAction("PaymentResponse");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult PaymentResponse()
        {
            try
            {

                string[] merc_hash_vars_seq;
                string merc_hash_string = string.Empty;
                string merc_hash = string.Empty;
                string order_id = string.Empty;
                string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";


                if (Request.Form["status"] == "success")
                {

                    merc_hash_vars_seq = hash_seq.Split('|');
                    Array.Reverse(merc_hash_vars_seq);
                    merc_hash_string = ConfigurationManager.AppSettings["SALT"] + "|" + Request.Form["status"];
                 
                    string prodinfo =Request.Form["productinfo"];
                    string[] splitString = prodinfo.Split('_');
                    int PackageId =Convert.ToInt16(splitString[1].Trim());
                    string firstname = Request.Form["firstname"];
                    string Amount=Request.Form["amount"];
                    string[] ssplitString = firstname.Split('_');
                    int ProfileId = Convert.ToInt16(ssplitString[1].Trim());
                    Session["UserId"] = ProfileId;
                    var sResult=dc.InsertUserPackage(ProfileId, PackageId, Request.Form["txnid"], "PayU");
                    string[] emailString = sResult.Split('#');
                    string Email = emailString[0].Trim();
                    string Packname= emailString[1].Trim();
                    foreach (string merc_hash_var in merc_hash_vars_seq)
                    {
                        merc_hash_string += "|";
                        merc_hash_string = merc_hash_string + (Request.Form[merc_hash_var] != null ? Request.Form[merc_hash_var] : "");

                    }
                    merc_hash = Generatehash512(merc_hash_string).ToLower();



                    if (merc_hash != Request.Form["hash"]){
                        order_id = Request.Form["txnid"];
                        ViewBag.TransactionId = order_id;
                        return View("FailureView");                      

                    }
                    else
                    {
                      
                        order_id = Request.Form["txnid"];
                        TempData["sTransactionId"] = order_id;
                        var Mailsubject = "WEvaha.com | Trusted Indian Matrimony | Find Lakhs of Profiles | Payment Confimation";
                        var MailContent = "<div style='border:2px #090 solid; padding:25px 0px 15px 0px;'><div style='text-align:center; border-bottom:#F00 1px solid;'>";
                        MailContent += "<div style='background:#dadbdb; padding:30px 150px; '>";
                        MailContent += "<div style='background:#fff;'>";
                        MailContent += "<div style='padding:10px 20px;'><img src='https://www.wevaha.com/img/logo-1.png' /></div>";
                        MailContent += "<div style='background:#003557; padding:20px 10px; color:#fff; text-align:center; font-family:Arial, Helvetica, sans-serif; font-size:30px; font-weight:bold;'>";
                        MailContent += "WEvaha Payment Confirmation</div>";
                        MailContent += "<div style='text-align:center; width:100%; margin:0px; padding:0px; top:0px; vertical-align:top;'><img width='30' height='11' style='text-align:center; vertical-";
                        MailContent += "align:top;' src='https://ci3.googleusercontent.com/proxy/4TUzOTIVogBHS3TeHaHgEamo1LFh2k-Agw1lt1DuxFvEpN3-G4_tmMMfPnyUBfeoFuS3-NIEk0GeYdkHyT45qXglauJUR3H5A07jOjPbHaIl2fovXVC04dk61-jkZPnmi6piBX3vHeW3uDUWVezvJYcjpJSOjdEjGg=s0-d-e1-ft#http://image.email.travelers.com/lib/fe5815707c62007f7d1d/m/3/cf0289d3-91f6-42ed-8c05-e8c0b7e1d48e.png' border='0' ></div>";
                        MailContent += "<div style='font-family:Arial, Helvetica, sans-serif; font-size:15px; color:#46494d; padding:30px; line-height:22px;'>";
                        MailContent += "Thank you for your payment.<br><br>";
                        MailContent += "<span style='font-size:16px;'>PACKAGE NAME</span><br>";
                        MailContent += Packname+"<br><br>";
                        MailContent += "<span style='font-size:16px;'>PAYMENT STATUS</span><br>";
                        MailContent += "Sucess<br><br>";
                        MailContent += "<span style='font-size:16px;'>ORDER ID #</span><br>";
                        MailContent += order_id + "<br><br>";
                        MailContent += "<span style='font-size:16px;'>PAYMENT AMOUNT</span><br>";
                        MailContent += Amount+"<br>";
                        MailContent += "<br>";
                        MailContent += "<span style='font-size:16px;'>PAYMENT METHOD</span><br>";
                        MailContent += "<div style='float:left'><img border='0' height='25' src='https://ci3.googleusercontent.com/proxy/s5NeRjwv0uKNX1xK_W1fQ2h8g7XI4BnmmZEbBC6NUeaLgHWT7wCdI9eA9nzOqumyZ6i98_gmbLYG5_3KfelmjphQEdhnCWeiQLBEpnoy8zLMy9XVmB19fL2XPTYlwyywVceIKLgmGLPuacAEDJzdPAxP3bMmzt-lPg=s0-d-e1-ft#http://image.email.travelers.com/lib/fe5e15707c6c06797c14/m/1/0f174d8e-0d67-43ea-bff9-0212dbd850b8.png' style ='display:block' width = '25' class='CToWUd'></div>&nbsp; Pay U Money";
                        MailContent += "<br><br>";
                        MailContent += "Most payments are reflected on your WEvaha.com a Levanture Inc., Company billing account by the next business day. You can go to WEvaha.com to view your payment activity.<br><br>";
                        MailContent += "If you have any questions about this transaction, please call us at XXX XXX XXXX.</div>";
                        MailContent += "<div style='border-top:1px #CCC solid; text-align:center; padding:25px;'>";
                        MailContent += "<img src='https://www.wevaha.com/img/logo-1.png' width='150' height='39'/><br>";
                        MailContent += "<a href='https://www.wevaha.com/' target='_blank' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#0066FF;'>www.WEvaha.com</a><br>";
                        MailContent += "<div style='font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#666666; padding-top:8px;'>© 2021 WEvaha.com a Levanture Inc., Company.<br>";
                        MailContent += "All rights reserved.</div></div></div>";

                        Email sEmail = new Email();
                        string FromEmailAddress = "registration@wevaha.com";
                        int eResult = sEmail.SentEmail(FromEmailAddress, Email, Mailsubject, MailContent);
                        return RedirectToAction("SucessView", "PayU");
                        
                    }

                }

                else
                {
                    string prodinfo = Request.Form["productinfo"];
                    string[] splitString = prodinfo.Split('_');
                    int PackageId = Convert.ToInt16(splitString[1].Trim());
                    string firstname = Request.Form["firstname"];
                    string Amount = Request.Form["amount"];
                    string[] ssplitString = firstname.Split('_');
                    int ProfileId = Convert.ToInt16(ssplitString[1].Trim());
                    Session["UserId"] = ProfileId;
                    var sResult = dc.InsertUserPackage(ProfileId, PackageId, Request.Form["txnid"], "PayU");
                    string[] emailString = sResult.Split('#');
                    string Email = emailString[0].Trim();
                    string Packname = emailString[1].Trim();
                    order_id = Request.Form["txnid"];
                    TempData["sTransactionId"] = order_id;
                    var Mailsubject = "WEvaha.com | Trusted Indian Matrimony | Find Lakhs of Profiles | Payment Confimation";
                    var MailContent = "<div style='border:2px #090 solid; padding:25px 0px 15px 0px;'><div style='text-align:center; border-bottom:#F00 1px solid;'>";
                    MailContent += "<div style='background:#dadbdb; padding:30px 150px; '>";
                    MailContent += "<div style='background:#fff;'>";
                    MailContent += "<div style='padding:10px 20px;'><img src='https://www.wevaha.com/img/logo-1.png' /></div>";
                    MailContent += "<div style='background:#003557; padding:20px 10px; color:#fff; text-align:center; font-family:Arial, Helvetica, sans-serif; font-size:30px; font-weight:bold;'>";
                    MailContent += "WEvaha Payment Confirmation</div>";
                    MailContent += "<div style='text-align:center; width:100%; margin:0px; padding:0px; top:0px; vertical-align:top;'><img width='30' height='11' style='text-align:center; vertical-";
                    MailContent += "align:top;' src='https://ci3.googleusercontent.com/proxy/4TUzOTIVogBHS3TeHaHgEamo1LFh2k-Agw1lt1DuxFvEpN3-G4_tmMMfPnyUBfeoFuS3-NIEk0GeYdkHyT45qXglauJUR3H5A07jOjPbHaIl2fovXVC04dk61-jkZPnmi6piBX3vHeW3uDUWVezvJYcjpJSOjdEjGg=s0-d-e1-ft#http://image.email.travelers.com/lib/fe5815707c62007f7d1d/m/3/cf0289d3-91f6-42ed-8c05-e8c0b7e1d48e.png' border='0' ></div>";
                    MailContent += "<div style='font-family:Arial, Helvetica, sans-serif; font-size:15px; color:#46494d; padding:30px; line-height:22px;'>";
                    MailContent += "Thank you for your payment.<br><br>";
                    MailContent += "<span style='font-size:16px;'>PACKAGE NAME</span><br>";
                    MailContent += Packname + "<br><br>";
                    MailContent += "<span style='font-size:16px;'>PAYMENT STATUS</span><br>";
                    MailContent += "Payment Failed<br><br>";
                    MailContent += "<span style='font-size:16px;'>ORDER ID #</span><br>";
                    MailContent += order_id + "<br><br>";
                    MailContent += "<span style='font-size:16px;'>PAYMENT AMOUNT</span><br>";
                    MailContent += Amount + "<br>";
                    MailContent += "<br>";
                    MailContent += "<span style='font-size:16px;'>PAYMENT METHOD</span><br>";
                    MailContent += "<div style='float:left'><img border='0' height='25' src='https://ci3.googleusercontent.com/proxy/s5NeRjwv0uKNX1xK_W1fQ2h8g7XI4BnmmZEbBC6NUeaLgHWT7wCdI9eA9nzOqumyZ6i98_gmbLYG5_3KfelmjphQEdhnCWeiQLBEpnoy8zLMy9XVmB19fL2XPTYlwyywVceIKLgmGLPuacAEDJzdPAxP3bMmzt-lPg=s0-d-e1-ft#http://image.email.travelers.com/lib/fe5e15707c6c06797c14/m/1/0f174d8e-0d67-43ea-bff9-0212dbd850b8.png' style ='display:block' width = '25' class='CToWUd'></div>&nbsp; Pay U Money";
                    MailContent += "<br><br>";
                    MailContent += "Most payments are reflected on your WEvaha.com a Levanture Inc., Company billing account by the next business day. You can go to WEvaha.com to view your payment activity.<br><br>";
                    MailContent += "If you have any questions about this transaction, please call us at XXX XXX XXXX.</div>";
                    MailContent += "<div style='border-top:1px #CCC solid; text-align:center; padding:25px;'>";
                    MailContent += "<img src='https://www.wevaha.com/img/logo-1.png' width='150' height='39'/><br>";
                    MailContent += "<a href='https://www.wevaha.com/' target='_blank' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#0066FF;'>www.WEvaha.com</a><br>";
                    MailContent += "<div style='font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#666666; padding-top:8px;'>© 2021 WEvaha.com a Levanture Inc., Company.<br>";
                    MailContent += "All rights reserved.</div></div></div>";

                    Email sEmail = new Email();
                    string FromEmailAddress = "registration@wevaha.com";
                    int eResult = sEmail.SentEmail(FromEmailAddress, Email, Mailsubject, MailContent);

                    return RedirectToAction("FailureView", "PayU");                   
                   // Response.Write("Hash value did not matched");
                    // osc_redirect(osc_href_link(FILENAME_CHECKOUT, 'payment' , 'SSL', null, null,true));

                }
            }

            catch (Exception ex)
            {
                //Response.Write("<span style='color:red'>" + ex.Message + "</span>");
                return View("FailureView");
            }

           

        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        private List<MvcHtmlString> PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            HtmlHelper helper = this.GetHtmlHelper();
            List<MvcHtmlString> lstMvcHtmlString = new List<MvcHtmlString>();
            //Set a name for the form
            // string formID = "PostForm";
            //Build the form using the specified data to be posted.
            //StringBuilder strForm = new StringBuilder();
            //strForm.Append("<form id=\"" + formID + "\" name=\"" +
            //               formID + "\" action=\"" + url +
            //               "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {
                lstMvcHtmlString.Add(helper.Hidden(Convert.ToString(key.Key), key.Value));
                //strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                //               "\" value=\"" + key.Value + "\">");
            }


            //strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            //StringBuilder strScript = new StringBuilder();


            //strScript.Append("<script language='javascript'>");
            //strScript.Append("var v" + formID + " = document." +
            //                 formID + ";");
            //strScript.Append("v" + formID + ".submit();");
            //strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return lstMvcHtmlString; //strForm.ToString() + strScript.ToString();
        }

        public ActionResult SucessView()
        {
            ViewBag.TransactionId = TempData["sTransactionId"];
            return View();
        }
        public ActionResult FailureView()
        {
           
            return View();
        }
    }


    public static class Extensions
    {
        //Exntesion method to get html helper for the controller
        public static HtmlHelper GetHtmlHelper(this Controller controller)
        {
            var viewContext = new ViewContext(controller.ControllerContext, new FakeView(), controller.ViewData, controller.TempData, TextWriter.Null);
            return new HtmlHelper(viewContext, new ViewPage());
        }

        public class FakeView : IView
        {
            public void Render(ViewContext viewContext, TextWriter writer)
            {
                throw new InvalidOperationException();
            }
        }

    }
}