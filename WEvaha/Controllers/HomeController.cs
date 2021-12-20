using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WE.BusinessServices;
using WE.BusinessServices.Interface;
using WE.Utilities;
using WEvaha.Models;

namespace WEvaha.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _Homeservice;
        private readonly IDashBoardService _dashBoardservice;
        HomeService homeService = new HomeService();
        DashBoardService dashBoardService = new DashBoardService();
        public readonly IChatService _chatService;
        ChatService chatService = new ChatService();
        public HomeController()
        {
            _Homeservice = homeService;
            _dashBoardservice = dashBoardService;
            _chatService = chatService;
        }

        // GET: Home
        public ActionResult Index()
        {
            Session.Abandon();
            DashboardProfileModel DBPM = new DashboardProfileModel();
            DBPM.userProfile = new UserProfile();
            DBPM.PersonalDetails = new Profile_PersonalDetails();
            DBPM.AboutMySelf = new Profile_AboutMySelf();
            DBPM.PersonalViews = new Profile_PersonalViews();
            DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
            DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
            DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
            DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
            DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();

            DBPM.AboutMySelf.FoodPreferences= _dashBoardservice.GetMasterData("FOOD_PREFERENCES");
            DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
            DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
            DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
            DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
            //
            DBPM.PersonalDetails.JobCityOpti = _dashBoardservice.GetCity();
            DBPM.PersonalDetails.JobStateOpti = _dashBoardservice.GetState();
            DBPM.PersonalDetails.JobCountryOpti = _dashBoardservice.GetCountry();
            //
            DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
            DBPM.AboutMySelf.sCasteOpti = new List<SelectListItem>();
            DBPM.AboutMySelf.SubCasteOpti = _dashBoardservice.GetSubCaste();
            DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
            DBPM.AboutMySelf.HeightOpti = _dashBoardservice.GetHeight();
            DBPM.AboutMySelf.RelationshipStatusOpti = GetMasterData("RELATIONSHIPSTATUS_TYPE");
            DBPM.AboutMySelf.EducationOpti = GetMasterData("EDUCATION_TYPE");
            DBPM.AboutMySelf.ProfessionOpti = GetMasterData("PROFESSION_TYPE");
            DBPM.AboutMySelf.JobOpti = GetMasterData("PROFESSION_TYPE");
            DBPM.AboutMySelf.KidsOpti = GetMasterData("KIDS_TYPE");
            DBPM.AboutMySelf.PetsOpti = GetMasterData("CHILDREN_TYPE");
            DBPM.AboutMySelf.SmokeOpti = GetMasterData("SMOKE_TYPE");
            DBPM.AboutMySelf.DrinkOpti = GetMasterData("DRINK_TYPE");
            DBPM.AboutMySelf.EthnicityOpti = _dashBoardservice.GetEthnicity();
            DBPM.AboutMySelf.ProfessionOpti = _dashBoardservice.GetProfession();
            DBPM.PersonalViews.FaithInGodOpti = GetMasterData("FAITHINGOD_TYPE");
            DBPM.PersonalViews.ReligionOpti = GetMasterData("RELIGION_TYPE");
            DBPM.PersonalViews.CasteOpti = GetMasterData("CASTE_TYPE");

            return View(DBPM);
        }
        public ActionResult Register()
        {
            return View();
        }

       // [HttpPost]
        public ActionResult SaveRegister(string email, string Gender, string SeekingGender, string password,string Location)
        {

            string ProfileName = null;
            var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
            var pwdEncryptred = EncryptandDecrypt.Encrypt(password, passwordsalt); //for encryption
            Guid activationCode = Guid.NewGuid();
            int UserId = _Homeservice.UserRegistration(ProfileName, email.Trim(), Gender, SeekingGender, pwdEncryptred.Trim(), Convert.ToString(activationCode), Location);

            if (UserId != -2)
            {
                var Mailsubject = "WEvaha.com | Trusted Indian Matrimony | Find Lakhs of Profiles | Register for Free Today";
                var MailContent = "<div style='border:2px #090 solid; padding:25px 0px 15px 0px;'><div style='text-align:center; border-bottom:#F00 1px solid;'>";
                MailContent += "<img src='www.wevaha.com/img/logo-1.png'><br/><br/></div><div style='padding:25px;'><br/><br/>";
                MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; color:#F00; font-weight:bold;'>";
                MailContent += "Congratulations! You have successfully registered.</span>";
                MailContent += "<p style='font-family:Arial, Helvetica, sans-serif; font-size:16px; color:#000; line-height:25px;'>";
                MailContent += "Dear User, <br/><br/>";
                MailContent += "A Warm welcome from WEvaha – thank you for registering.<br/><br/>";
                MailContent += "The key to a successful marriage is a successful relationship. Vital Characteristics are based on knowledge and experience, and are more likely to alter based on life-events and results we get. You can find matches on the default criteria or you can have WEvaha.com application choose the match for you based on the 5 sectors or you can create a search agent.<br/><br/>";
                MailContent += "<p style='font-family:Arial, Helvetica, sans-serif; font-size:16px; color:#000; line-height:25px;'>Can’t wait to start? Please ";
                MailContent += "<a href ='" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here</a> to activate your account, and get ready to find someone special for you.<br/><br/></p><br/></p>";

                MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:15px; font-style:italic; color:#666; font-weight:700;'>Kind ";
                MailContent += "Regards</span><br/><br/>";
                MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; color:#000; font-weight:700;'>WEvaha Team </span><br/></div></div>";

                Email sEmail = new Email();
                string FromEmailAddress = "registration@wevaha.com";
                int sResult = sEmail.SentEmail(FromEmailAddress, email, Mailsubject, MailContent);
            }

            return Json(UserId, JsonRequestBehavior.AllowGet);           
        }

        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(string Usename, string Password)
        {
            var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
            var pwdEncryptred = EncryptandDecrypt.Encrypt(Password.Trim(), passwordsalt); //for encryption   
            int? IsFullyRegister = 0;

            int Isvalid = _Homeservice.ValidateUser(Usename.Trim(), pwdEncryptred.Trim());
            int UserId = _Homeservice.GetUserId(Usename.Trim(), pwdEncryptred.Trim());
            if (Isvalid == 1)
            {
                IsFullyRegister = _Homeservice.IsFullyRegistered(UserId);
                if (IsFullyRegister == 1)
                {
                    var UserFullName = _Homeservice.UserFullName(Usename.Trim(), pwdEncryptred.Trim());
                    Session["UserName"] = Usename;
                    Session["UserFullName"] = UserFullName;                    
                }
                else
                {
                    Isvalid = 5;
                    ViewBag.Email = Usename;
                }
            }
           
            Session["UserId"] = UserId;
            string IsUserAcitive = _Homeservice.IsPackageActive(UserId);
            Session["IsUserAcitive"] = IsUserAcitive;
            Session["UserUnReadMessages"] = _chatService.GetChatUsersMsgCountResult(UserId).Take(10).ToList();
            return Json(Isvalid, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult IsFullyRegistered()
        {
            DashboardProfileModel DBPM = new DashboardProfileModel();
            if (Session["UserId"] != null)
            {               
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                DBPM.userProfile = new UserProfile();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();
                DBPM.compatibleMatches = new List<CompatibleMatches>();
                DBPM.recentVisitors = new List<RecentVisitors>();
                DBPM.whoViewedMe = new List<WhoViewedMe>();
                DBPM.PersonalViews = new Profile_PersonalViews();

                DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
                DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
                DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                var data = _dashBoardservice.GetMyProfile(ProfileId);
                DBPM.userProfile.ProfileId = ProfileId;

                if (DBPM.userProfile != null)
                {
                    DBPM.userProfile.ProfileName = data.PersonalDetails.ProfileName;
                    DBPM.userProfile.ProfileLastName = data.PersonalDetails.ProfileLastName;
                    DBPM.userProfile.Email = data.userProfile.Email;
                    DBPM.userProfile.DateofBirth = data.userProfile.DateofBirth;
                    DBPM.userProfile.Gender = data.userProfile.Gender;
                    DBPM.userProfile.SeekingGender = data.userProfile.SeekingGender;
                    DBPM.userProfile.Zipcode = data.userProfile.Zipcode;
                    DBPM.userProfile.Country = data.userProfile.Country;
                    DBPM.userProfile.HowDidYouHearAboutUs = data.userProfile.HowDidYouHearAboutUs;
                }

                if (data.AboutMySelf != null)
                {
                    if (data.AboutMySelf.Religion != "")
                    {
                        DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion);
                    }

                    DBPM.AboutMySelf.Language = Convert.ToInt16(data.AboutMySelf.Language);
                    DBPM.AboutMySelf.CasteOpti = _dashBoardservice.GetCaste(DBPM.AboutMySelf.Language);
                    DBPM.AboutMySelf.sCasteOpti = new List<SelectListItem>();
                    if (data.AboutMySelf.Caste != "") { DBPM.AboutMySelf.Caste = Convert.ToInt16(data.AboutMySelf.Caste); }
                }
                if (data.PersonalDetails != null)
                {
                    DBPM.PersonalDetails.ProfileName = data.PersonalDetails.ProfileName;
                    DBPM.PersonalDetails.ProfileLastName = data.PersonalDetails.ProfileLastName;
                    DBPM.PersonalDetails.Email = data.PersonalDetails.Email;
                    DBPM.PersonalDetails.DateofBirth = data.PersonalDetails.DateofBirth.ToString("dd/MM/yyyy");
                    DBPM.PersonalDetails.Age = Convert.ToInt16(data.PersonalDetails.Age);
                    DBPM.PersonalDetails.Mobile = data.PersonalDetails.Mobile;
                    DBPM.PersonalDetails.City = data.PersonalDetails.City;
                    DBPM.PersonalDetails.Country = data.PersonalDetails.Country;
                    DBPM.PersonalDetails.State = data.PersonalDetails.State;
                    DBPM.PersonalDetails.Zipcode = data.PersonalDetails.Zipcode;
                    DBPM.PersonalDetails.FBLink = data.PersonalDetails.FBLink;
                    DBPM.PersonalDetails.InstaLink = data.PersonalDetails.InstaLink;
                    DBPM.PersonalDetails.TwitterLink = data.PersonalDetails.TwitterLink;
                    DBPM.PersonalDetails.SocialMediaLinks = data.PersonalDetails.SocialMediaLinks;
                    DBPM.PersonalDetails.Introduction = data.PersonalDetails.Introduction;
                }

                var sProfilesPhotos = _dashBoardservice.GetProfilePhotos(ProfileId);
                foreach (var li in sProfilesPhotos)
                {
                    ProfilePhoto pl = new ProfilePhoto();
                    pl.PhotoId = li.PhotoId;
                    pl.ProfileId = li.ProfileId;
                    pl.PhotoUrl = li.PhotoUrl;
                    pl.ThumbnailUrl = li.ThumbnailUrl;
                    DBPM.profilePhotos.Add(pl);
                }

                var sCoverPhoto = _dashBoardservice.GetCoverPhoto(ProfileId);
                DBPM.coverPhoto.CoverPhotoId = sCoverPhoto.CoverPhotoId;
                DBPM.coverPhoto.CoverPhotoURL = sCoverPhoto.CoverPhotoURL;

                var sProfilePic = _dashBoardservice.GetProfilePic(ProfileId);
                DBPM.userprofilePic.UserProfilePicId = sProfilePic.UserProfilePicId;
                DBPM.userprofilePic.UserProfilePicUrl = sProfilePic.UserProfilePicUrl;
                // //geting top 8 Compatable matches
                var rComp = _dashBoardservice.GetCompatibleMatches(ProfileId);
                foreach (var li in rComp)
                {
                    CompatibleMatches cm = new CompatibleMatches();
                    cm.ProfileId = Convert.ToInt16(li.ProfileId);
                    cm.ProfileName = li.ProfileName;
                    cm.ProfileLastName = li.ProfileLastName;
                    cm.Age = li.Age;
                    cm.Height = li.Height;
                    cm.City = li.City;
                    cm.StateName = li.StateName;
                    cm.Country = li.Country;
                    cm.MotherTongue = li.MotherTongue;
                    cm.DefaultPhotoUrl = li.DefaultPhotoUrl;
                    DBPM.compatibleMatches.Add(cm);

                }
                var rVisit = _dashBoardservice.GetRecentVisitors(ProfileId);
                foreach (var lis in rVisit)
                {
                    RecentVisitors rv = new RecentVisitors();
                    rv.ProfileId = Convert.ToInt16(lis.ProfileId);
                    rv.ViewedProfileId = Convert.ToInt16(lis.ViewedProfileId);
                    rv.ProfileName = lis.ProfileName;
                    rv.ProfileLastName = lis.ProfileLastName;
                    rv.Age = lis.Age;
                    rv.Height = lis.Height;
                    rv.City = lis.City;
                    rv.StateName = lis.StateName;
                    rv.Country = lis.Country;
                    rv.MotherTongue = lis.MotherTongue;
                    rv.DefaultPhotoUrl = lis.DefaultPhotoUrl;
                    DBPM.recentVisitors.Add(rv);
                }
                var wViewed = _dashBoardservice.GetDashBoardWhoViewedMe(ProfileId, 8);
                foreach (var lis in wViewed)
                {
                    WhoViewedMe Wv = new WhoViewedMe();
                    Wv.ProfileId = Convert.ToInt16(lis.ProfileId);
                    Wv.ViewedProfileId = Convert.ToInt16(lis.ViewedProfileId);
                    Wv.ProfileName = lis.ProfileName;
                    Wv.ProfileLastName = lis.ProfileLastName;
                    Wv.Age = lis.Age;
                    Wv.Height = lis.Height;
                    Wv.City = lis.City;
                    Wv.StateName = lis.StateName;
                    Wv.Country = lis.Country;
                    Wv.MotherTongue = lis.MotherTongue;
                    Wv.DefaultPhotoUrl = lis.DefaultPhotoUrl;
                    Wv.Gender = lis.Gender;
                    DBPM.whoViewedMe.Add(Wv);
                }


                //DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
                //DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
                //DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
                //DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                //DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();

                DBPM.AboutMySelf.FoodPreferences = _dashBoardservice.GetMasterData("FOOD_PREFERENCES");
                //DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
                DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
                DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
                //
                DBPM.PersonalDetails.JobCityOpti = _dashBoardservice.GetCity();
                DBPM.PersonalDetails.JobStateOpti = _dashBoardservice.GetState();
                DBPM.PersonalDetails.JobCountryOpti = _dashBoardservice.GetCountry();
                //
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DBPM.AboutMySelf.sCasteOpti = new List<SelectListItem>();
                DBPM.AboutMySelf.SubCasteOpti = _dashBoardservice.GetSubCaste();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.AboutMySelf.HeightOpti = _dashBoardservice.GetHeight();
                DBPM.AboutMySelf.RelationshipStatusOpti = GetMasterData("RELATIONSHIPSTATUS_TYPE");
                DBPM.AboutMySelf.EducationOpti = GetMasterData("EDUCATION_TYPE");
                DBPM.AboutMySelf.ProfessionOpti = GetMasterData("PROFESSION_TYPE");
                DBPM.AboutMySelf.JobOpti = GetMasterData("PROFESSION_TYPE");
                DBPM.AboutMySelf.KidsOpti = GetMasterData("KIDS_TYPE");
                DBPM.AboutMySelf.PetsOpti = GetMasterData("CHILDREN_TYPE");
                DBPM.AboutMySelf.SmokeOpti = GetMasterData("SMOKE_TYPE");
                DBPM.AboutMySelf.DrinkOpti = GetMasterData("DRINK_TYPE");
                DBPM.AboutMySelf.EthnicityOpti = _dashBoardservice.GetEthnicity();
                DBPM.AboutMySelf.ProfessionOpti = _dashBoardservice.GetProfession();
                DBPM.PersonalViews.FaithInGodOpti = GetMasterData("FAITHINGOD_TYPE");
                DBPM.PersonalViews.ReligionOpti = GetMasterData("RELIGION_TYPE");
                DBPM.PersonalViews.CasteOpti = GetMasterData("CASTE_TYPE");
               
                int? IsFullyRegister = _Homeservice.IsFullyRegistered(Convert.ToInt16(ProfileId));
                DBPM.IsFullyRegistered =Convert.ToInt32(IsFullyRegister);
                }
            return PartialView("_PopUpContent", DBPM);
         
        }

        public ActionResult Activation(string Id)
        {
            string message = string.Empty;
            Guid guid = new Guid(Id);
            if (Id != null)
            {
                int userActivationCode = _Homeservice.UserActivations(guid);
                if (userActivationCode == 1)
                {
                    message = "Activation successful.";
                }
                else
                {
                    message = "Activation Failed.";
                }
            }
            return RedirectToAction("Thanks");
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Home/ResetPassword?id=" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            int UserId = _Homeservice.GetUserID(EmailID);
            if (UserId>0)
            {
                _Homeservice.SaveResetPasswordCode(EmailID, resetCode);
                var Mailsubject = "WEvaha.com Password Reset Request";
                Email sEmail = new Email();
                var MailContent = "<div style='border:2px #090 solid; padding:25px 0px 15px 0px;'>";
                MailContent += "<div style='text-align:center; border-bottom:#F00 1px solid;'>";
                MailContent += "<img src='http://www.wevaha.com/img/logo-1.png'><br/><br/></div>";
                MailContent += "<div style='padding:25px;'><br/><br/>";
                MailContent += "<p style='font-family:Arial, Helvetica, sans-serif; font-size:18px; color:#000; line-height:25px; font-weight:bold;'>Dear User,<br/><br/>";
                MailContent += "<p style='font-family:Arial, Helvetica, sans-serif; font-size:18px; color:#000; line-height:25px;'>";
                MailContent += "You recently requested to reset your password for your WEvaha Account. Click the button below to reset it.</p><br/><div style='padding-left:10px;'>";
                MailContent += "<a href='" + link + "' style='background:#F00; padding:10px 25px; border-radius:10px; color:#fff; font-family:Arial, Helvetica, sans-serif;font-size:18px;text-decoration:none;'>Reset your password</a></div><br/>";
                MailContent += "<p style='font-family:Arial, Helvetica, sans-serif; font-size:18px; color:#000; line-height:25px;'>";
                MailContent += "If you did not request a password reset, please ignore this email or reply to let us know.";
                MailContent += "</p><br/><br/><br />";
                MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-style:italic; color:#666; font-weight:700;'>Thanks,</span><br/><br/>";
                MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:18px; color:#000; font-weight:700;'>WEvaha Team </span><br/></div></div>";

                string FromEmailAddress = "registration@wevaha.com";
                int sResult = sEmail.SentEmail(FromEmailAddress, EmailID, Mailsubject, MailContent);
                ViewBag.Message = "Reset password link has been sent to your email id.";
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                int Userstatus = _Homeservice.GetUserStatus(EmailID);
                if (Userstatus == 2)
                {
                    return Json("Request", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public ActionResult ResetPassword(string id)
        {
            ResetPassword model = new ResetPassword();
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }


            var user = _Homeservice.GetUserByResetPaswordCode(id);
            if (user != 0)
            {

                model.ResetCode = id;
                return View(model);

            }
            else
            { return View(); }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveResetPassword(ResetPassword model)
        {
            var message = "";


            var user = _Homeservice.GetUserByResetPaswordCode(model.ResetCode);
            if (user != 0)
            {
                //pass userId and password
                var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
                var pwdEncryptred = EncryptandDecrypt.Encrypt(model.NewPassword.Trim(), passwordsalt.Trim()); //for encryption

                int sResult = _Homeservice.SaveNewPassword(user, pwdEncryptred.Trim());

                message = "New password updated successfully";
            }


            ViewBag.Message = message;
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportAbuseForm(string Category, string Subject, string Message, string Complaint, string Email)
        {
            var Mailsubject = "WEvaha.com Report Abuse Form Complaint";
            Email sEmail = new Email();

            var MailContent = "<div style='border:2px #06F solid; padding:25px 0px 15px 0px;'>";
            MailContent += "<div style='text-align:center; border-bottom:#F00 1px solid;'>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:30px; font-weight:bold; color:#03F;'>WEvaha</span><span style='font-family:Arial, Helvetica, sans-serif; font-size:30px; font-weight:bold; color:#F00;'></span>";
            MailContent += "<span style = 'font-family:Arial, Helvetica, sans-serif; font-size:20px; font-weight:bold; color:#F00;' > </ span ><br/>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold; color:#333;'></span><br/>";
            MailContent += "<br /></div><div style='padding:25px;'>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:17px;color:#000;font-weight:bold;'>Hello Team,</span><br/><br/>";
            MailContent += "<div style='padding-bottom:10px;'><span style ='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#03F;'>Please find the Complaint details from Report Abuse Form, the Details are </span>";
            MailContent += "<div style='padding-bottom:10px;'><span style ='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;color:#03F;'>Category: </span>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#333;'>" + Category + "</span></div>";
            MailContent += "<div style='padding-bottom:10px;'><span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#03F;'>Subject : </span>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#333;'>" + Subject + "</span></div>";

            MailContent += "<div style='padding-bottom:10px;'><span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#03F;'>Message : </span>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#333;'>" + Message + "</span></div>";

            MailContent += "<div style='padding-bottom:10px;'><span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#03F;'>Complaint : </span>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#333;'>" + Complaint + "</span></div>";

            MailContent += "<div style='padding-bottom:10px;'><span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#03F;'>Email : </span>";
            MailContent += "<span style='font-family:Arial, Helvetica, sans-serif; font-size:16px; font-weight:bold;  color:#333;'>" + Email + "</span></div></div></div>";

            string FromEmailAddress = "registration@wevaha.com";
            int sResult = sEmail.SentEmail(FromEmailAddress, Email, Mailsubject, MailContent);
            ViewBag.Message = "Reset password link has been sent to your email id.";
            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        public ActionResult SaveProfile(string ProfileName, string ProfileId, string ProfileLastName, string DateofBirth, string Mobile, string Countrycode, string RelationshipStatus, string Job, string Religion, string Language, string Caste, string FoodPreference, string Height, string Education, string Country, string State, string City, string Zipcode, string IsSameasAbove, string JobCountry, string JobState, string JobCity, string JobZipcode, string Introduction,string Email, string RelationshipDetails, string JobDetails, string EducationDetails)
        {
            string status = string.Empty;
            WE.BusinessEntities.DashboardProfileModel DBPM = new WE.BusinessEntities.DashboardProfileModel();
            DBPM.userProfile = new WE.BusinessEntities.UserProfile();
            DBPM.PersonalDetails = new WE.BusinessEntities.Profile_PersonalDetails();
            DBPM.AboutMySelf = new WE.BusinessEntities.Profile_AboutMySelf();
            if(Convert.ToInt16(ProfileId)==0)
            {
                DBPM.PersonalDetails.ProfileId = Convert.ToInt32(Session["UserId"]);
            }
            else
            {
                DBPM.PersonalDetails.ProfileId = Convert.ToInt32(ProfileId);
            }
            //DBPM.PersonalDetails.ProfileId = Convert.ToInt32(ProfileId);
            DBPM.PersonalDetails.ProfileLastName = ProfileLastName;
            DBPM.PersonalDetails.ProfileName = ProfileName;
            DBPM.PersonalDetails.DateofBirth =Convert.ToDateTime(DateofBirth);
            DBPM.PersonalDetails.Mobile = Mobile;
            DBPM.PersonalDetails.CountryCode = Countrycode;
            DBPM.AboutMySelf.RelationshipStatus = RelationshipStatus;
            DBPM.AboutMySelf.Job = Job;
            DBPM.AboutMySelf.Religion = Religion;
            DBPM.AboutMySelf.Language = Language;
            DBPM.AboutMySelf.Caste = Caste;
            DBPM.AboutMySelf.FoodPreference = FoodPreference;
            DBPM.AboutMySelf.Height = Height;
            DBPM.AboutMySelf.Education = Education;
            DBPM.PersonalDetails.Country = Country;
            DBPM.PersonalDetails.State = State;
            DBPM.PersonalDetails.City = City;
            DBPM.PersonalDetails.Zipcode = Zipcode;
            DBPM.PersonalDetails.IsSameasAbove =Convert.ToBoolean(IsSameasAbove);
            DBPM.PersonalDetails.JobCountry = JobCountry;
            DBPM.PersonalDetails.JobState = JobState;
            DBPM.PersonalDetails.JobCity = JobCity;
            DBPM.PersonalDetails.JobZipcode = JobZipcode;
            DBPM.PersonalDetails.Introduction = Introduction;
            DBPM.AboutMySelf.RelationshipDetails = RelationshipDetails;
            DBPM.AboutMySelf.JobDetails = JobDetails;
            DBPM.AboutMySelf.EducationDetails = EducationDetails;         
           
            if (DBPM.PersonalDetails.ProfileId != 0)
            {
                DBPM.PersonalDetails.Age = CalculateAge(Convert.ToDateTime(DBPM.PersonalDetails.DateofBirth));
                if (string.IsNullOrEmpty(DBPM.PersonalDetails.Introduction)) { DBPM.PersonalDetails.Introduction = "Hi! While I’m still figuring this all out, here’s something I know for sure - I’m excited to be here! The chance to meet Unique, Engaging, and Interesting people is pretty dang neat. If you think you might fit that thought, drop me a message."; }
                if (DBPM.PersonalDetails.ProfileId != 0)
                {
                    _Homeservice.SaveProfile(DBPM);
                }
                status = "success";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            else
            {
                status = "failure";
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;
           
            for (int i = 0; i < files.Count; i++)
            {
                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";    
                //string filename = Path.GetFileName(Request.Files[i].FileName);    

                HttpPostedFileBase file = files[i];
                string fname;

                // Checking for Internet Explorer    
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                    FileName = file.FileName;
                }
                int ProfileId = 0;
                if (Convert.ToInt32(Request.Form["ProfileId"])!=0)
                {
                    ProfileId = Convert.ToInt32(Request.Form["ProfileId"]);
                }
                else
                {
                    ProfileId = Convert.ToInt32(Session["UserId"]);
                }
                
                var fileName = Path.GetFileName(file.FileName);
                var _ext = Path.GetExtension(file.FileName);
                string PhotoUrl = _dashBoardservice.InsertProfilePhotos(ProfileId, _ext);
                string ModifiedPhotoUrl = "ProfilePhoto_" + PhotoUrl;
                fname = Path.Combine(Server.MapPath("~/ProfilePhotos/"), ModifiedPhotoUrl);
                file.SaveAs(fname);

                string modifiesThumbUrl = "Thumbnails_" + PhotoUrl;
                var thumbName = Path.Combine(Server.MapPath("~/Thumbnails/"), modifiesThumbUrl);
                Image img = Image.FromFile(fname);
                int imgHeight = 100;
                int imgWidth = 100;
                if (img.Width < img.Height)
                {
                    //portrait image  
                    imgHeight = 100;
                    var imgRatio = (float)imgHeight / (float)img.Height;
                    imgWidth = Convert.ToInt32(img.Height * imgRatio);
                }
                else if (img.Height < img.Width)
                {
                    //landscape image  
                    imgWidth = 100;
                    var imgRatio = (float)imgWidth / (float)img.Width;
                    imgHeight = Convert.ToInt32(img.Height * imgRatio);
                }
                Image thumb = img.GetThumbnailImage(imgWidth, imgHeight, () => false, IntPtr.Zero);
                thumb.Save(thumbName);

            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }
        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }
        public ActionResult Thanks()
        {
            return View();
        }

        public ActionResult AboutUs()
        {
            return View();
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult ReportAbuse()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsofUse()
        {
            return View();
        }

        public ActionResult SafetyTips()
        {
            return View();
        }

        public ActionResult Guidelines()
        {
            return View();
        }
        public ActionResult FAQ()
        {
            return View();
        }
        public ActionResult DatingProfile()
        {
            return View();
        }

        public List<SelectListItem> GetMasterData(string MasterType)
        {
            List<SelectListItem> MasterData = new List<SelectListItem>();
            MasterData = _dashBoardservice.GetMasterData(MasterType); //_dashBoardservice.GetMasterData(MasterType);
            return MasterData;
        }
    }
}