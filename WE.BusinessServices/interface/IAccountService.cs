using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WE.BusinessEntities;

namespace WE.BusinessServices.Interface
{
    public interface IAccountService
    {
        int Register(UserProfile usp);
        int login(string Username, string Pasword);
        int Log(string Exception, string InnerException, string Uri);
        int UserActivations(Guid ActivationCode);
        int ChangePassword(UserProfile usp);
        List<SelectListItem> GetMasterData(string MasterType);
    }
}
