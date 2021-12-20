using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WE.BusinessEntities;

namespace WE.BusinessServices.Interface
{
    public interface IHomeService
    {
        int UserRegistration(string ProfileName, string email, string Gender, string SeekingGender, string password, string ActivationCode, string Location);
        int UserActivations(Guid ActivationCode);
        int ValidateUser(string Usename, string Password);
        int GetUserId(string Usename, string Password);
        int? IsFullyRegistered(int ProfileId);
        string IsPackageActive(int UserId);
        string GetUserName(string Usename);
        int GetUserID(string Usename);
        string UserFullName(string Usename, string Password);

        void SaveResetPasswordCode(string Usename, string resetCode);
        int GetUserByResetPaswordCode(string id);
        int SaveNewPassword(int user, string pwdEncryptred);

        int GetUserStatus(string Usename);
        string InsertProfilePhotos(int ProfileId, string PhotoNameExt);
        int SaveProfile(DashboardProfileModel DBPM);

    }
}
