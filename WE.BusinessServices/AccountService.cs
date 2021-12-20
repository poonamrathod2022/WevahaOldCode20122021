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
    public class AccountService : IAccountService
    {
        private WEvahaEntities _entities = new WEvahaEntities();
        public int Register(UserProfile usp)
        {
            var Result = new ObjectParameter("ProfileId", typeof(int));
            _entities.USP_WE_INSERT_PROFILE(usp.ProfileName,usp.Email, usp.Password, usp.Gender, usp.SeekingGender, usp.Zipcode, (usp.Country), usp.HowDidYouHearAboutUs, usp.ActivationCode, Result);
            _entities.SaveChanges();
            return Convert.ToInt16(Result.Value);
        }
        public int Log(string LException, string LInnerException, string LUri)
        {
            var Result = new ObjectParameter("LogId", typeof(int));
            _entities.USP_INSERT_LOG(LException, LInnerException, LUri, Result);
            _entities.SaveChanges();
            return Convert.ToInt16(Result.Value);
        }
        public int login(string Username, string Pasword)
        {
            int result = 0;

            var sResult = _entities.WE_VALIDATE_USER(Username, Pasword).FirstOrDefault();

            if (Convert.ToInt16(sResult) == 0)
            {
                //User registered But Not Activated
                result = 0;
            }
            else if (Convert.ToInt16(sResult) == 1)
            {
                //User registered and Activated
                result = 1;
            }
            else if (Convert.ToInt16(sResult) == 2)
            {
                //User Requested for Forget Password
                result = 2;//user in database but Not Activated
            }
            else if (Convert.ToInt16(sResult) == 3)
            {
                //User registered But Not Activated
                result = 3;
            }
            else if (Convert.ToInt16(sResult) == 4)
            {
                //Invalid UserId and Password
                result = 4;
            }
            return result;

        }

        public int UserActivations(Guid activationCode)
        {
            var Result = new ObjectParameter("sResult", typeof(int));
            _entities.USP_WE_USER_ACTIVATION(Result, activationCode);
            _entities.SaveChanges();

            return Convert.ToInt16(Result.Value);
        }
        public int ChangePassword(UserProfile usp)
        {
            int result = 0;
            result = _entities.USP_WE_CHANGE_PASSWORD(usp.ProfileId, usp.Password);
            return result;
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
    }
}
