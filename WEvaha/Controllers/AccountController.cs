using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WE.BusinessEntities;
using WE.BusinessServices;
using WE.BusinessServices.Interface;
using WE.Utilities;


namespace WEvaha.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private readonly IAccountService _Accountservice;
        AccountService accountService = new AccountService();
        public AccountController()
        {
            _Accountservice = accountService;
        }
        string ResponseMessage = string.Empty;

      
        [HttpPost] [Route("api/Account/Register")]
        public ActionResult Register(UserProfile usp)
        {
          
            _Accountservice.Register(usp);
            return View();
           
        }
       
        [HttpPost]
        [Route("api/Account/ValidateLogin")]
        public ActionResult ValidateLogin(LoginEntity lgenty)
        {
           
            var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
            var strEncrypt = EncryptandDecrypt.Encrypt(lgenty.Password, passwordsalt);// for Decryption
            int data = _Accountservice.login(lgenty.UserName, strEncrypt);
            if (data == 1)
            {
                //response = Request.CreateResponse(HttpStatusCode.OK, value: TokenManager.GenerateToken(lgenty.Password));
            }
            else if (data == 0)
            {
                //response = Request.CreateErrorResponse(HttpStatusCode.BadGateway, message: "User Not Activated");
            }
            else if (data == 2)
            {
                //response = Request.CreateErrorResponse(HttpStatusCode.NoContent, message: "User Requested for Forget Password");
            }
            else if (data == 4)
            {
                //response = Request.CreateErrorResponse(HttpStatusCode.NotImplemented, message: "User name and password is invalid");
            }
            else if (data == 5)
            { }
            return View();
        }

      
        public ActionResult Activation(String ActivationCode)
        {
            Guid guid = new Guid(ActivationCode);
            int data = _Accountservice.UserActivations(guid);
            if (data == 1)
            {
                //return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            else
            {
               // return Request.CreateErrorResponse(HttpStatusCode.BadGateway, message: "ActivationCode invalid");
            }
            return View();
        }
       
        [HttpPost]
        public ActionResult ChangePassword(int ProfileId, string Password)
        {
            UserProfile usp = new UserProfile();
           
            usp.ProfileId = ProfileId;
            var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
            var strDecryptred = EncryptandDecrypt.Encrypt(Password, passwordsalt);// for encryption
            usp.Password = "";
            usp.Password = strDecryptred;
            try
            {
                int cp = _Accountservice.ChangePassword(usp);
                if (cp == 1)
                {
                    ResponseMessage = "Password Changed Successfully..!";
                    //response = Request.CreateResponse(HttpStatusCode.OK, cp);
                }
                else
                {
                    ResponseMessage = "No User Exists...!";
                    //response = Request.CreateResponse(HttpStatusCode.OK, ResponseMessage);
                }
            }
            catch (Exception ex)
            {
                ResponseMessage = "No Lawyer Exists...!";
            }
            return View();
        }
        public List<SelectListItem> GetMasterData(string MasterType)
        {
            List<SelectListItem> MasterData = new List<SelectListItem>();
            MasterData = _Accountservice.GetMasterData(MasterType);
            return MasterData;
        }
    }
}