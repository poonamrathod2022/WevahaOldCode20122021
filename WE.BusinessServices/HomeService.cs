using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WE.BusinessEntities;
using WE.BusinessServices.Interface;
using WE.DataAccess;

namespace WE.BusinessServices
{
    public class HomeService : IHomeService
    {
        private WEvahaEntities _entities = new WEvahaEntities();
        public int Register(UserProfile usp)
        {
            var Result = new ObjectParameter("ProfileId", typeof(int));
            _entities.USP_WE_INSERT_PROFILE(usp.ProfileName, usp.Email, usp.Password, usp.Gender, usp.SeekingGender, usp.Zipcode, usp.Country, usp.HowDidYouHearAboutUs, usp.ActivationCode, Result);
            _entities.SaveChanges();
            return Convert.ToInt16(Result.Value);
        }

        public int UserRegistration(string ProfileName, string email, string Gender, string SeekingGender, string password, string ActivationCode,string Location)
        {
            var ProfileId = new ObjectParameter("ProfileId", typeof(int));
            _entities.WEvaha_Insert_User_Registration(ProfileId, ProfileName, email, Gender, SeekingGender, password, ActivationCode, Location);
            _entities.SaveChanges();
            return Convert.ToInt32(ProfileId.Value);
        }

        public int UserActivations(Guid activationCode)
        {
            var Result = new ObjectParameter("sResult", typeof(int));
            _entities.WEvaha_Get_UserActivationCode(Result, activationCode);
            _entities.SaveChanges();

            return Convert.ToInt16(Result.Value);
        }

        public int ValidateUser(string Usename, string Password)
        {
            int sIsvalid = 0;
            var IsValid = _entities.WEvaha_Validate_User(Usename, Password);
            foreach (var li in IsValid)
            {
                sIsvalid = (int)li;
            }
            return sIsvalid;
        }

        public int GetUserId(string Usename, string Password)
        {
            int sUserId = 0;
            var UserId = _entities.WEvaha_Get_UserId(Usename, Password);
            foreach (var li in UserId)
            {
                sUserId = (int)li;
            }
            return sUserId;
        }
        public int? IsFullyRegistered(int ProfileId)
        {
            int? isFullyreg = 0;
            isFullyreg = _entities.WE_ISFULLYREGISTERED(ProfileId).FirstOrDefault();
            return isFullyreg;
        }
        public string IsPackageActive(int UserId)
        {
            var rIsPackageActive = _entities.USP_WEVAHA_ISUER_PACKAGE_ACTIVE(UserId).FirstOrDefault();
            return rIsPackageActive;
        }
        public string UserFullName(string Usename, string Password)
        {
            var FullName = new ObjectParameter("Result", typeof(int));
            _entities.WEvaha_User_FullName(FullName, Usename, Password);
            _entities.SaveChanges();
            return Convert.ToString(FullName.Value);
        }
        public string GetUserName(string Username)
        {
            string User_name = string.Empty;

            var UName = _entities.WEvaha_Get_UserName(Username);
            foreach (var li in UName)
            {
                User_name = Convert.ToString(li);
            }
            return User_name;
        }

        public int GetUserID(string Usename)
        {
            int User_id = 0;

            var UName = _entities.WEvaha_Get_ProfileId(Usename).ToList();
            User_id = UName[0].Value;
            return User_id;
        }

        public int GetUserStatus(string Username)
        {
            int UserStatus = 0;

            var UStatus = _entities.WEvaha_Get_UserStatus(Username);
            foreach (var li in UStatus)
            {
                UserStatus = Convert.ToInt16(li);
            }
            return UserStatus;
        }
        public void SaveResetPasswordCode(string Usename, string resetCode)
        {
            _entities.WEvaha_Insert_ResetPasswordCode(Usename, resetCode);
        }

        public int GetUserByResetPaswordCode(string Id)
        {
            int sUserId = 0;
            var UserId = _entities.WEvaha_Get_User_By_ResetPasswordCode(Id);
            foreach (var li in UserId)
            {
                sUserId = (int)li;
            }
            return sUserId;
        }
        public int SaveNewPassword(int user, string pwdEncryptred)
        {

            var sResult = new ObjectParameter("Result", typeof(int));
            _entities.WEvaha_Update_NewPassword(sResult, user, pwdEncryptred);
            _entities.SaveChanges();
            return Convert.ToInt16(sResult.Value);
        }
        public List<SelectListItem> GetMasterData(string param)
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETLOOKUPVALUE(param).ToList();

            foreach (var li in data)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.LookupCode),
                    Text = li.LookupValue
                });

            }
            return _masterdata;
        }
        public string InsertProfilePhotos(int ProfileId, string PhotoNameExt)
        {
            var optPhotoname = new ObjectParameter("Optimizedphotourl", typeof(string));
            _entities.usp_WE_Insert_Profile_Photo(ProfileId, PhotoNameExt, optPhotoname).ToList();
            _entities.SaveChanges();
            return Convert.ToString(optPhotoname.Value);
        }
        public int SaveProfile(DashboardProfileModel DBPM)
        {           
            _entities.USP_WE_INSERT_PROFILE_ABOUTMYSELF(DBPM.PersonalDetails.ProfileId, DBPM.AboutMySelf.Height, DBPM.AboutMySelf.RelationshipStatus, DBPM.AboutMySelf.RelationshipDetails, DBPM.AboutMySelf.Kids, DBPM.AboutMySelf.Pets, DBPM.AboutMySelf.WantChildren, DBPM.AboutMySelf.Education,DBPM.AboutMySelf.EducationDetails, DBPM.AboutMySelf.Smoke, DBPM.AboutMySelf.Drink, DBPM.AboutMySelf.Ethnicity, DBPM.AboutMySelf.Religion, DBPM.AboutMySelf.Language, DBPM.AboutMySelf.Caste, DBPM.AboutMySelf.SubCaste, DBPM.AboutMySelf.Job, DBPM.AboutMySelf.JobDetails, DBPM.AboutMySelf.CityofJob, DBPM.AboutMySelf.StateofJob, DBPM.AboutMySelf.CountryofJob,DBPM.AboutMySelf.FoodPreference);
            _entities.USP_WE_INSERT_PROFILE_PERSONALDETAILS(DBPM.PersonalDetails.ProfileId, DBPM.PersonalDetails.ProfileName, DBPM.PersonalDetails.ProfileLastName, DBPM.PersonalDetails.Email, Convert.ToString(DBPM.PersonalDetails.DateofBirth), Convert.ToString(DBPM.PersonalDetails.Age), DBPM.PersonalDetails.Mobile,DBPM.PersonalDetails.CountryCode, DBPM.PersonalDetails.City, DBPM.PersonalDetails.Country, DBPM.PersonalDetails.State, DBPM.PersonalDetails.Zipcode, DBPM.PersonalDetails.JobCity, DBPM.PersonalDetails.JobCountry, DBPM.PersonalDetails.JobState, DBPM.PersonalDetails.JobZipcode, DBPM.PersonalDetails.IsSameasAbove, DBPM.PersonalDetails.FBLink, DBPM.PersonalDetails.InstaLink, DBPM.PersonalDetails.TwitterLink, DBPM.PersonalDetails.SocialMediaLinks, DBPM.PersonalDetails.Introduction);
            return DBPM.PersonalDetails.ProfileId;
        }
    }
}
