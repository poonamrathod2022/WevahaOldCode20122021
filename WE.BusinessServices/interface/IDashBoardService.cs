using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WE.BusinessEntities;

namespace WE.BusinessServices.Interface
{
    public interface IDashBoardService
    {
        List<SelectListItem> GetMasterData(string MasterType);
        List<SelectListItem> GetCity();
        List<SelectListItem> GetState();
        List<SelectListItem> GetCountry();
        List<SelectListItem> GetEthnicity();
        List<SelectListItem> GetReligion();
        List<SelectListItem> GetSubCaste();
        List<SelectListItem> GetCaste(int LanguageId);
        List<SelectListItem> GetHeight();
        List<SelectListItem> GetProfession();
        List<SelectListItem> GetLanguage();
        List<SelectListItem> AgeFrom();
        List<SelectListItem> AgeTo();       
        List<SelectListItem> GetCasteByLanguage(int LanguageId);
        List<UserProfile> GetSearch(int SeekingGender,string AgeFrom,string AgeTo,string religion,string caste,string Language, int ProfileId);
        List<UserProfile> GetSearchCompatableMatches(int SeekingGender, string AgeFrom, string AgeTo, string religion, string caste, string Language, int ProfileId);
        List<UserProfile> SearchCriteria(string SeekingGender, string Religion, string Language, string Caste,string AgeFrom, string AgeTo, string HFrom, string HTo, string Country, string RelationshipStatus, string Job, string Education, string Smoke, string Drink, string Kids, string FaithInGod, string Pets, string Travel, string Movies, string Shopping,int ProfileId);
        int SaveProfile(DashboardProfileModel DBPM);
        DashboardProfileModel GetUserProfile(int vProfileId,int ProfileId);
        DashboardProfileModel GetMyProfile(int ProfileId);
        string InsertProfilePhotos(int ProfileId, string PhotoNameExt);
        string InsertCoverPhoto(int ProfileId, string PhotoUrl);
        int InsertProfilePic(int ProfileId, string filePath);
        List<ProfilePhoto> GetProfilePhotos(int ProfileId);
        Coverphoto GetCoverPhoto(int ProfileId);
        UserProfilePic GetProfilePic(int ProfileId);
        List<UserProfile> GetWhoViewedMe(int ProfileId);
        bool SetFavourite(int pId,int fId);
        bool RemFavourite(int pId, int fId);
        List<UserProfile> GetUserfavourites(int ProfileId);
        List<Packages> Packages();
        string ViewMobileNumber(int vProfileId, int ProfileId);
        UserProfile GetProfiledetails(int ProfileId);
        string InsertUserPackage(int ProfileId, int PackageId, string Transaction_Id, string Payment_From);
        UserPackageDetails GetUserPackageDetails(int ProfileId);
        List<CompatibleMatches> GetCompatibleMatches(int ProfileId);
        List<RecentVisitors> GetRecentVisitors(int ProfileId);
        List<RecentVisitors> GettotRecentVisitors(int ProfileId);
        List<SelectListItem> States(int CountryId);
        List<SelectListItem> Cities(int StateId);
        List<UserProfile> GetUserCompatableMatches(int ProfileId);
        List<WhoViewedMe> GetDashBoardWhoViewedMe(int ProfileId, int rows);
        int SetDefaultPhoto(int ProfileId, int PhotoId);
        int SaveNewPassword(int user, string pwdEncryptred);
    }
}
