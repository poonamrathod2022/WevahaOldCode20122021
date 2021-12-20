using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WE.BusinessServices;
using WE.BusinessServices.Interface;
using WE.Utilities;
using WEvaha.Models;

namespace WEvaha.Controllers
{
    public class DashBoardController : Controller
    {

        private readonly IDashBoardService _dashBoardservice;
        DashBoardService dashboardService = new DashBoardService();
        DashboardProfileModel DBPM = new DashboardProfileModel();
        public DashBoardController()
        {
            _dashBoardservice = dashboardService;
        }
        // GET: DashBoard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
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
                DBPM.AboutMySelf.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
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
                   if(data.AboutMySelf.Religion!="")
                    {
                        DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion);
                    }
                  
                    DBPM.AboutMySelf.Language = Convert.ToInt16(data.AboutMySelf.Language);
                    DBPM.AboutMySelf.CasteOpti = _dashBoardservice.GetCaste(DBPM.AboutMySelf.Language);
                    
                    DBPM.AboutMySelf.sCasteOpti = new List<SelectListItem>();
                    if (data.AboutMySelf.Caste != "") { DBPM.AboutMySelf.Caste = Convert.ToInt16(data.AboutMySelf.Caste);
                    }                                     
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
                var wViewed = _dashBoardservice.GetDashBoardWhoViewedMe(ProfileId,8);
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
            else
            {
                return Redirect("../Home/Index");
            }
        }
        public ActionResult SetDefaultPhoto()
        {
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
                    if (data.AboutMySelf.Religion != "") {DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion);}                  
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
              
              
                return View(DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
        [HttpPost]
        public JsonResult SetDefaultPhoto(int ProfileId, int PhotoId)
        {
            int sResult = _dashBoardservice.SetDefaultPhoto(ProfileId,PhotoId);
            return Json(sResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMyProfile()
        {
            if (Session["UserId"] != null)
            {
                int ProfileId = Convert.ToInt32(Session["UserId"]);
               
                DBPM.userProfile = new UserProfile();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.PersonalChoices = new Profile_PersonalChoices();
                DBPM.WhoAmI = new Profile_WhoAmI();
                DBPM.SelfDescription = new Profile_SelfDescription();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.PersonalViews = new Profile_PersonalViews();
                DBPM.Hobbies = new Profile_Hobbies();
                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();

                DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
                DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
                DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
                //
                DBPM.PersonalDetails.JobCityOpti = _dashBoardservice.GetCity();
                DBPM.PersonalDetails.JobStateOpti = _dashBoardservice.GetState();
                DBPM.PersonalDetails.JobCountryOpti = _dashBoardservice.GetCountry();
                //
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();

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
                DBPM.AboutMySelf.FoodPreferences = GetMasterData("FOOD_PREFERENCES");
                DBPM.PersonalViews.SuperstitiousOpti = GetMasterData("SUPERSTITIOUS_TYPE");
                DBPM.PersonalViews.AstrologyOpti = GetMasterData("ASTROLOGY_TYPE");
                DBPM.PersonalViews.FoodOpti = GetMasterData("FOOD_TYPE");
                DBPM.PersonalViews.OutsideEatingOpti = GetMasterData("OUTSIDEEATING_TYPE");
                DBPM.PersonalViews.MoviesOpti = GetMasterData("MOVIES_TYPE");
                DBPM.PersonalViews.OnRelaxingOpti = GetMasterData("ONRELAXING_TYPE");
                DBPM.PersonalViews.TravelOpti = GetMasterData("TRAVEL_TYPE");
                DBPM.PersonalViews.ShoppingOpti = GetMasterData("SHOPPING_TYPE");
                DBPM.PersonalViews.SpendingOpti = GetMasterData("SPENDING_TYPE");
                DBPM.PersonalViews.BeingSelfOpti = GetMasterData("BEINGSELF_TYPE");
                DBPM.PersonalViews.HumorOpti = GetMasterData("HUMOR_TYPE");
                DBPM.PersonalViews.AnxiousOpti = GetMasterData("ANXIOUS_TYPE");
                DBPM.PersonalViews.TensionOpti = GetMasterData("TENSION_TYPE");
                DBPM.PersonalViews.SpeakingMindOpti = GetMasterData("SPEAKINGMYMIND_TYPE");
                DBPM.PersonalViews.SadTimesOpti = GetMasterData("SADTIMES_TYPE");
                DBPM.PersonalViews.AngryOpti = GetMasterData("ANGRY_TYPE");
                DBPM.PersonalViews.TalkativeOpti = GetMasterData("TALKATIVE_TYPE");
                DBPM.PersonalViews.FateOpti = GetMasterData("FATE_TYPE");
                DBPM.PersonalViews.SelfControlOpti = GetMasterData("SELFCONTROL_TYPE");
                DBPM.PersonalViews.MisUnderstoodOpti = GetMasterData("MISUNDERSTOOD_TYPE");
                DBPM.PersonalViews.AchieveGoalsOpti = GetMasterData("ACHIEVEGOALS_TYPE");
                DBPM.PersonalViews.CaringOpti = GetMasterData("CARING_TYPE");
                DBPM.PersonalViews.SystematicOpti = GetMasterData("SYSTEMATIC_TYPE");
                DBPM.PersonalViews.CreativityOpti = GetMasterData("CREATIVITY_TYPE");
                DBPM.PersonalViews.ForgivingOpti = GetMasterData("FORGIVING_TYPE");
                DBPM.PersonalViews.SensitivityOpti = GetMasterData("SENSITIVITY_TYPE");
                DBPM.PersonalViews.AdmittingFaultsOpti = GetMasterData("ADMITTINGFAULTS_TYPE");
                DBPM.PersonalViews.EgoOpti = GetMasterData("EGO_TYPE");
                DBPM.PersonalViews.FriendshipOpti = GetMasterData("FRIENDSHIP_TYPE");
                DBPM.PersonalViews.DailyChoresOpti = GetMasterData("DAILYCHORES_TYPE");
                DBPM.PersonalViews.ToleranceOpti = GetMasterData("TOLERANCE_TYPE");
                DBPM.PersonalViews.TechSavvyOpti = GetMasterData("TECHSAVVY_TYPE");
                DBPM.PersonalViews.InternetOpti = GetMasterData("INTERNET_TYPE");
                DBPM.PersonalViews.PoliticsOpti = GetMasterData("POLITICS_TYPE");
                DBPM.PersonalViews.SocietyOpti = GetMasterData("SOCIETY_TYPE");
                DBPM.PersonalViews.TakingInitiativeOpti = GetMasterData("TAKINGINITIATIVE_TYPE");
                DBPM.PersonalViews.FamilyValuesOpti = GetMasterData("FAMILYVALUES_TYPE");
                DBPM.PersonalChoices.GoalsOpti = GetMasterData("PERSONALGOALS_TYPE");
                DBPM.PersonalChoices.FriendsNetworkOpti = GetMasterData("FRIENDSNETWORK_TYPE");
                DBPM.PersonalChoices.FriendLaughOpti = GetMasterData("FRIENDSLAUGH_TYPE");
                DBPM.PersonalChoices.HouseImprovementsOpti = GetMasterData("HOUSEREPAIRS_TYPE");
                DBPM.PersonalChoices.CareAboutOpti = GetMasterData("VOLUNTERINGTIME_TYPE");
                DBPM.PersonalChoices.OrganizedLifeOpti = GetMasterData("ORGANIZEDLIFE_TYPE");
                DBPM.PersonalChoices.FinancesOpti = GetMasterData("FINANCES_TYPE");
                DBPM.PersonalChoices.HomeEntertainingOpti = GetMasterData("HOMEENTERTAIN_TYPE");
                DBPM.PersonalChoices.CaringforChildrenOpti = GetMasterData("CHILDRENCARE_TYPE");
                DBPM.PersonalChoices.RomanceinRelationOpti = GetMasterData("ROMANCE_TYPE");
                DBPM.PersonalChoices.SocializingOpti = GetMasterData("SOCIALIZING_TYPE");
                DBPM.PersonalChoices.HomeEnvironmentOpti = GetMasterData("HOMEENVIRONMENT_TYPE");
                DBPM.PersonalChoices.SharingBeliefsOpti = GetMasterData("BELIEFS_TYPE");
                DBPM.PersonalChoices.PhysicalFitOpti = GetMasterData("PHYSICALFIT_TYPE");
                DBPM.PersonalChoices.CalmDuringCrisisOpti = GetMasterData("CALM_TYPE");
                DBPM.PersonalChoices.ThoughtsAndfeelingsOpti = GetMasterData("FEELINGS_TYPE");
                DBPM.PersonalChoices.HelpingFortunatesOpti = GetMasterData("LESSFORTUNATE_TYPE");
                DBPM.PersonalChoices.ResolveConflictOpti = GetMasterData("CONFLICT_TYPE");
                DBPM.PersonalChoices.AdventuresOpti = GetMasterData("ADVENTURES_TYPE");
                DBPM.PersonalChoices.KnowledgeAndAwarenessOpti = GetMasterData("KNOWLEDGE_TYPE");
                DBPM.PersonalChoices.ProfessionalPlanningOpti = GetMasterData("PERSONALPLANNING_TYPE");
                DBPM.PersonalChoices.EventsUnderstandingOpti = GetMasterData("WORLDEVENTS_TYPE");
                DBPM.PersonalChoices.FindingPleasureOpti = GetMasterData("SIMPLETHINGS_TYPE");
                DBPM.PersonalChoices.CultureAndTraditionOpti = GetMasterData("TRADITION_TYPE");
                DBPM.PersonalChoices.CreativeSolutionsOpti = GetMasterData("CREATIVESOLUTIONS_TYPE");
                DBPM.PersonalChoices.MakingFriendsOpti = GetMasterData("NEWFRIENDS_TYPE");
                DBPM.PersonalChoices.CookingForFamilyOpti = GetMasterData("FAMILYCOOKING_TYPE");
                DBPM.PersonalChoices.ProvideIncomeforFamilyOpti = GetMasterData("EARNINGINCOME_TYPE");
                DBPM.PersonalChoices.HouseholdSchedulesOpti = GetMasterData("HOUSEHOLD_TYPE");
                DBPM.PersonalChoices.BeingAgoodFriendOpti = GetMasterData("GOODFRIEND_TYPE");
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
                if (data.PersonalChoices != null)
                {
                    DBPM.PersonalChoices.Goals = Convert.ToInt16(data.PersonalChoices.Goals);
                    DBPM.PersonalChoices.FriendsNetwork = Convert.ToInt16(data.PersonalChoices.FriendsNetwork);
                    DBPM.PersonalChoices.FriendLaugh = Convert.ToInt16(data.PersonalChoices.FriendLaugh);
                    DBPM.PersonalChoices.HouseImprovements = Convert.ToInt16(data.PersonalChoices.HouseImprovements);
                    DBPM.PersonalChoices.CareAbout = Convert.ToInt16(data.PersonalChoices.CareAbout);
                    DBPM.PersonalChoices.OrganizedLife = Convert.ToInt16(data.PersonalChoices.OrganizedLife);
                    DBPM.PersonalChoices.Finances = Convert.ToInt16(data.PersonalChoices.Finances);
                    DBPM.PersonalChoices.HomeEntertaining = Convert.ToInt16(data.PersonalChoices.HomeEntertaining);
                    DBPM.PersonalChoices.CaringforChildren = Convert.ToInt16(data.PersonalChoices.CaringforChildren);
                    DBPM.PersonalChoices.RomanceinRelation = Convert.ToInt16(data.PersonalChoices.RomanceinRelation);
                    DBPM.PersonalChoices.Socializing = Convert.ToInt16(data.PersonalChoices.Socializing);
                    DBPM.PersonalChoices.HomeEnvironment = Convert.ToInt16(data.PersonalChoices.HomeEnvironment);
                    DBPM.PersonalChoices.SharingBeliefs = Convert.ToInt16(data.PersonalChoices.SharingBeliefs);
                    DBPM.PersonalChoices.PhysicalFit = Convert.ToInt16(data.PersonalChoices.PhysicalFit);
                    DBPM.PersonalChoices.CalmDuringCrisis = Convert.ToInt16(data.PersonalChoices.CalmDuringCrisis);
                    DBPM.PersonalChoices.ThoughtsAndfeelings = Convert.ToInt16(data.PersonalChoices.ThoughtsAndfeelings);
                    DBPM.PersonalChoices.HelpingFortunates = Convert.ToInt16(data.PersonalChoices.HelpingFortunates);
                    DBPM.PersonalChoices.ResolveConflict = Convert.ToInt16(data.PersonalChoices.ResolveConflict);
                    DBPM.PersonalChoices.Adventures = Convert.ToInt16(data.PersonalChoices.Adventures);
                    DBPM.PersonalChoices.KnowledgeAndAwareness = Convert.ToInt16(data.PersonalChoices.KnowledgeAndAwareness);
                    DBPM.PersonalChoices.ProfessionalPlanning = Convert.ToInt16(data.PersonalChoices.ProfessionalPlanning);
                    DBPM.PersonalChoices.EventsUnderstanding = Convert.ToInt16(data.PersonalChoices.EventsUnderstanding);
                    DBPM.PersonalChoices.FindingPleasure = Convert.ToInt16(data.PersonalChoices.FindingPleasure);
                    DBPM.PersonalChoices.CultureAndTradition = Convert.ToInt16(data.PersonalChoices.CultureAndTradition);
                    DBPM.PersonalChoices.CreativeSolutions = Convert.ToInt16(data.PersonalChoices.CreativeSolutions);
                    DBPM.PersonalChoices.MakingFriends = Convert.ToInt16(data.PersonalChoices.MakingFriends);
                    DBPM.PersonalChoices.CookingForFamily = Convert.ToInt16(data.PersonalChoices.CookingForFamily);
                    DBPM.PersonalChoices.ProvideIncomeforFamily = Convert.ToInt16(data.PersonalChoices.ProvideIncomeforFamily);
                    DBPM.PersonalChoices.HouseholdSchedules = Convert.ToInt16(data.PersonalChoices.HouseholdSchedules);
                    DBPM.PersonalChoices.BeingAgoodFriend = Convert.ToInt16(data.PersonalChoices.BeingAgoodFriend);
                }
                if (data.AboutMySelf != null)
                {
                    if (data.AboutMySelf.Height != "") { DBPM.AboutMySelf.Height = Convert.ToInt16(data.AboutMySelf.Height); }
                    DBPM.AboutMySelf.RelationshipStatus = Convert.ToInt16(data.AboutMySelf.RelationshipStatus);
                    DBPM.AboutMySelf.RelationshipDetails = data.AboutMySelf.RelationshipDetails;
                    DBPM.AboutMySelf.Kids = Convert.ToInt16(data.AboutMySelf.Kids);
                    DBPM.AboutMySelf.Pets = Convert.ToInt16(data.AboutMySelf.Pets);
                    DBPM.AboutMySelf.WantChildren = Convert.ToInt16(data.AboutMySelf.WantChildren);
                    DBPM.AboutMySelf.Education = Convert.ToInt16(data.AboutMySelf.Education);
                    DBPM.AboutMySelf.EducationDetails = data.AboutMySelf.EducationDetails;
                    DBPM.AboutMySelf.Smoke = Convert.ToInt16(data.AboutMySelf.Smoke);
                    DBPM.AboutMySelf.Drink = Convert.ToInt16(data.AboutMySelf.Drink);
                    DBPM.AboutMySelf.Ethnicity = Convert.ToInt16(data.AboutMySelf.Ethnicity);
                    if (data.AboutMySelf.Religion != "")
                    {
                        DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion);
                    }
                    if (data.AboutMySelf.Language != "")
                    {
                        DBPM.AboutMySelf.Language = Convert.ToInt16(data.AboutMySelf.Language);
                    }
                    DBPM.AboutMySelf.CasteOpti = _dashBoardservice.GetCaste(DBPM.AboutMySelf.Language);
                    DBPM.AboutMySelf.sCasteOpti = new List<SelectListItem>();
                    if (data.AboutMySelf.Caste != "") { DBPM.AboutMySelf.Caste = Convert.ToInt16(data.AboutMySelf.Caste); }                  
                    DBPM.AboutMySelf.SubCaste = Convert.ToInt16(data.AboutMySelf.SubCaste);
                    DBPM.AboutMySelf.sJob = data.AboutMySelf.Job;
                    //if(data.AboutMySelf.Job!="")
                    //{ DBPM.AboutMySelf.sJob = data.AboutMySelf.Job; }

                    DBPM.AboutMySelf.JobDetails = data.AboutMySelf.JobDetails;
                    DBPM.AboutMySelf.CityofJob = data.AboutMySelf.CityofJob;
                   // DBPM.AboutMySelf.StateofJob= data.AboutMySelf.
                    DBPM.AboutMySelf.CountryofJob = data.AboutMySelf.CountryofJob;
                    DBPM.AboutMySelf.IntroductionAboutMyself = data.AboutMySelf.IntroductionAboutMyself;
                }
                if (data.PersonalDetails != null)
                {
                    DBPM.PersonalDetails.ProfileName = data.PersonalDetails.ProfileName;
                    DBPM.PersonalDetails.ProfileLastName = data.PersonalDetails.ProfileLastName;
                    DBPM.PersonalDetails.Email = data.PersonalDetails.Email;
                    DBPM.PersonalDetails.DateofBirth = data.PersonalDetails.DateofBirth.ToString("dd/MM/yyyy");
                    DBPM.PersonalDetails.Age = Convert.ToInt16(data.PersonalDetails.Age);
                    DBPM.PersonalDetails.Mobile = data.PersonalDetails.Mobile;
                    DBPM.PersonalDetails.Country = data.PersonalDetails.Country;
                    DBPM.PersonalDetails.State = data.PersonalDetails.State;
                    DBPM.PersonalDetails.City = data.PersonalDetails.City;
                    DBPM.PersonalDetails.Zipcode = data.PersonalDetails.Zipcode;
                    // job city
                    DBPM.PersonalDetails.IsSameasAbove = data.PersonalDetails.IsSameasAbove;
                    DBPM.PersonalDetails.JobCountry = string.IsNullOrEmpty(data.PersonalDetails.JobCountry)? null : data.PersonalDetails.JobCountry.Replace(" ", "");
                    DBPM.PersonalDetails.JobState = string.IsNullOrEmpty(data.PersonalDetails.JobState) ? null : data.PersonalDetails.JobState.Replace(" ", "");
                    DBPM.PersonalDetails.JobCity = string.IsNullOrEmpty(data.PersonalDetails.JobCity) ? null : data.PersonalDetails.JobCity.Replace(" ", "");
                    DBPM.PersonalDetails.JobZipcode = data.PersonalDetails.JobZipcode;
                  
                    //
                    DBPM.PersonalDetails.FBLink = data.PersonalDetails.FBLink;
                    DBPM.PersonalDetails.InstaLink = data.PersonalDetails.InstaLink;
                    DBPM.PersonalDetails.TwitterLink = data.PersonalDetails.TwitterLink;
                    DBPM.PersonalDetails.SocialMediaLinks = data.PersonalDetails.SocialMediaLinks;
                    DBPM.PersonalDetails.Introduction = data.PersonalDetails.Introduction;
                }
                if (data.PersonalViews != null)
                {
                    DBPM.PersonalViews.FaithInGod = data.PersonalViews.FaithInGod;
                    DBPM.PersonalViews.Religion = data.PersonalViews.Religion;
                    DBPM.PersonalViews.Caste = data.PersonalViews.Caste;
                    DBPM.PersonalViews.Superstitious = data.PersonalViews.Superstitious;
                    DBPM.PersonalViews.Astrology = data.PersonalViews.Astrology;
                    DBPM.PersonalViews.Food = data.PersonalViews.Food;
                    DBPM.PersonalViews.OutsideEating = data.PersonalViews.OutsideEating;
                    DBPM.PersonalViews.Movies = data.PersonalViews.Movies;
                    DBPM.PersonalViews.OnRelaxing = data.PersonalViews.OnRelaxing;
                    DBPM.PersonalViews.Travel = data.PersonalViews.Travel;
                    DBPM.PersonalViews.Shopping = data.PersonalViews.Shopping;
                    DBPM.PersonalViews.Spending = data.PersonalViews.Spending;
                    DBPM.PersonalViews.BeingSelf = data.PersonalViews.BeingSelf;
                    DBPM.PersonalViews.Humor = data.PersonalViews.Humor;
                    DBPM.PersonalViews.Anxious = data.PersonalViews.Anxious;
                    DBPM.PersonalViews.Tension = data.PersonalViews.Tension;
                    DBPM.PersonalViews.SpeakingMind = data.PersonalViews.SpeakingMind;
                    DBPM.PersonalViews.SadTimes = data.PersonalViews.SadTimes;
                    DBPM.PersonalViews.Angry = data.PersonalViews.Angry;
                    DBPM.PersonalViews.Talkative = data.PersonalViews.Talkative;
                    DBPM.PersonalViews.Fate = data.PersonalViews.Fate;
                    DBPM.PersonalViews.SelfControl = data.PersonalViews.SelfControl;
                    DBPM.PersonalViews.MisUnderstood = data.PersonalViews.MisUnderstood;
                    DBPM.PersonalViews.AchieveGoals = data.PersonalViews.AchieveGoals;
                    DBPM.PersonalViews.Caring = data.PersonalViews.Caring;
                    DBPM.PersonalViews.Systematic = data.PersonalViews.Systematic;
                    DBPM.PersonalViews.Creativity = data.PersonalViews.Creativity;
                    DBPM.PersonalViews.Forgiving = data.PersonalViews.Forgiving;
                    DBPM.PersonalViews.Sensitivity = data.PersonalViews.Sensitivity;
                    DBPM.PersonalViews.AdmittingFaults = data.PersonalViews.AdmittingFaults;
                    DBPM.PersonalViews.Ego = data.PersonalViews.Ego;
                    DBPM.PersonalViews.Friendship = data.PersonalViews.Friendship;
                    DBPM.PersonalViews.DailyChores = data.PersonalViews.DailyChores;
                    DBPM.PersonalViews.Tolerance = data.PersonalViews.Tolerance;
                    DBPM.PersonalViews.TechSavvy = data.PersonalViews.TechSavvy;
                    DBPM.PersonalViews.Internet = data.PersonalViews.Internet;
                    DBPM.PersonalViews.Politics = data.PersonalViews.Politics;
                    DBPM.PersonalViews.Society = data.PersonalViews.Society;
                    DBPM.PersonalViews.TakingInitiative = data.PersonalViews.TakingInitiative;
                    DBPM.PersonalViews.FamilyValues = data.PersonalViews.FamilyValues;
                }
                if (data.Hobbies != null)
                {
                    DBPM.Hobbies.CarsAndDriving = data.Hobbies.CarsAndDriving;
                    DBPM.Hobbies.SpendFreeTime = data.Hobbies.SpendFreeTime;
                    DBPM.Hobbies.SportsILike = data.Hobbies.SportsILike;
                    DBPM.Hobbies.InstrumentsIPlay = data.Hobbies.InstrumentsIPlay;
                    DBPM.Hobbies.Music = data.Hobbies.Music;
                    DBPM.Hobbies.Vacations = data.Hobbies.Vacations;
                    DBPM.Hobbies.Favorite = data.Hobbies.Favorite;
                    DBPM.Hobbies.TVShows = data.Hobbies.TVShows;
                    DBPM.Hobbies.Movies = data.Hobbies.Movies;
                    DBPM.Hobbies.Books = data.Hobbies.Books;
                    DBPM.Hobbies.Travel = data.Hobbies.Travel;
                    DBPM.Hobbies.Excercising = data.Hobbies.Excercising;
                    DBPM.Hobbies.Hiking = data.Hobbies.Hiking;
                    DBPM.Hobbies.Driving = data.Hobbies.Driving;
                }
                if (data.SelfDescription != null)
                {
                    DBPM.SelfDescription.PeopleNoticeFirstThing = data.SelfDescription.PeopleNoticeFirstThing;
                    DBPM.SelfDescription.MostThankful = data.SelfDescription.MostThankful;
                    DBPM.SelfDescription.MostInfluentPerson = data.SelfDescription.MostInfluentPerson;
                    DBPM.SelfDescription.CannotLiveWithout = data.SelfDescription.CannotLiveWithout;
                    DBPM.SelfDescription.SpendLeisureTime = data.SelfDescription.SpendLeisureTime;
                    DBPM.SelfDescription.MostProudOf = data.SelfDescription.MostProudOf;
                    DBPM.SelfDescription.LovablePerson = data.SelfDescription.LovablePerson;
                    DBPM.SelfDescription.ImportantQualityofaPerson = data.SelfDescription.ImportantQualityofaPerson;
                    DBPM.SelfDescription.AdditionalInfo = data.SelfDescription.AdditionalInfo;
                }
                if (data.WhoAmI != null)
                {
                    DBPM.WhoAmI.ThankkfulFor = data.WhoAmI.ThankkfulFor;
                    DBPM.WhoAmI.LifeSkills = data.WhoAmI.LifeSkills;
                    DBPM.WhoAmI.ThingsCantLiveWithout = data.WhoAmI.ThingsCantLiveWithout;
                    DBPM.WhoAmI.FriendsDescribedAs = data.WhoAmI.FriendsDescribedAs;
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

                return View(DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
       
        public ActionResult GetSearch(string Url)
        {

            string sUrl = AESEncrytDecry.DecryptStringAES(Url);
            string username = "http://localhost:59506/DashBoard/GetSearch?" + sUrl;
            username = Regex.Replace(username, @"\s", "");
            //"../DashBoard/GetSearch?AgeFrom=" +
            Uri myUri = new Uri(username);
            string AgeFrom; string AgeTo; string religion; string caste; string Language; int PageIndex; string From; int SeekingGender;
            AgeFrom = HttpUtility.ParseQueryString(myUri.Query).Get("AgeFrom");
            AgeTo = HttpUtility.ParseQueryString(myUri.Query).Get("AgeTo");
            religion =HttpUtility.ParseQueryString(myUri.Query).Get("religion");
            caste = HttpUtility.ParseQueryString(myUri.Query).Get("caste");
            Language = HttpUtility.ParseQueryString(myUri.Query).Get("Language");
            PageIndex = Convert.ToInt16(HttpUtility.ParseQueryString(myUri.Query).Get("PageIndex"));
            From = HttpUtility.ParseQueryString(myUri.Query).Get("From");
            SeekingGender = Convert.ToInt16(HttpUtility.ParseQueryString(myUri.Query).Get("SeekingGender"));
            DashboardProfileModel DPM = new DashboardProfileModel();
            DPM.PersonalDetails = new Profile_PersonalDetails();
            DPM.AboutMySelf = new Profile_AboutMySelf();
            string sReligion; string sCaste; string sLanguage;
            DPM.UserProfilelistdetails = new List<UserProfile>();
            DPM.UserProfileCompatablelistdetails = new List<UserProfile>();
            if (religion == "Religion") { sReligion = null; } else { sReligion = religion; }
            if (caste == "Caste" || caste == "undefined") { sCaste = null; } else { sCaste = caste; }
            if (Language == "Language"|| Language == "") { sLanguage = null; } else { sLanguage = Language; }
            if (AgeFrom == "AgeFrom") { AgeFrom = null; }
            if (AgeTo == "AgeTo") { AgeTo = null; }
            int ProfileId = Convert.ToInt32(Session["UserId"]);

            int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
            int totalPage = 0;
            int totalRecord = 0;
            int ctotalPage = 0;
            int ctotalRecord = 0;
            totalRecord = _dashBoardservice.GetSearch(SeekingGender,AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).Count();
            
                var DSM = _dashBoardservice.GetSearch(SeekingGender, AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                foreach (var li in DSM)
                {

                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.Gender = (int)li.Gender;
                    up.GenderName = li.GenderName;
                    up.SeekingGender = (int)li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    DPM.UserProfilelistdetails.Add(up);
                }
            if (totalRecord < 11)
            {
                ctotalRecord = _dashBoardservice.GetSearchCompatableMatches(SeekingGender, AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).Count();
                var cDSM = _dashBoardservice.GetSearchCompatableMatches(SeekingGender, AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                ctotalPage = (ctotalRecord / pageSize) + ((ctotalRecord % pageSize) > 0 ? 1 : 0);

                foreach (var li in cDSM)
                {

                    UserProfile up = new UserProfile();
                     up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.Gender = (int) li.Gender;
                    up.SeekingGender = (int) li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    DPM.UserProfileCompatablelistdetails.Add(up);
                }
            }
                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                DPM.AboutMySelf.AgeFromOption = _dashBoardservice.AgeFrom();
                DPM.userProfile = new UserProfile();
                DPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                DPM.userProfile.SeekingGender = SeekingGender;
                DPM.AboutMySelf.AgeFrom = string.IsNullOrEmpty(AgeFrom) ? 0 : Convert.ToInt16(AgeFrom); //Convert.ToInt16(AgeFrom);
                DPM.AboutMySelf.AgeToOption = _dashBoardservice.AgeTo();
                DPM.AboutMySelf.AgeTo = string.IsNullOrEmpty(AgeTo) ? 0 : Convert.ToInt16(AgeTo);//Convert.ToInt16(AgeTo);
                DPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DPM.AboutMySelf.Religion = sReligion == "null" ? 0 : Convert.ToInt16(sReligion);//Convert.ToInt16(sReligion);
                DPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DPM.AboutMySelf.Language = sLanguage == "null"|| sLanguage =="" ? 0 : Convert.ToInt16(sLanguage);//Convert.ToInt16(sLanguage);
                DPM.AboutMySelf.CasteOpti = _dashBoardservice.GetCasteByLanguage(DPM.AboutMySelf.Language);
                DPM.AboutMySelf.Caste = sCaste == "null" ? 0 : Convert.ToInt32(sCaste);//Convert.ToInt16(sCaste);
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
               DPM.UserPackageStatus = package.Status == null ? 0 : 1;
               ViewBag.Sourcepage = "BasicSearch";
                DPM.coverPhoto = new Coverphoto();
                DPM.userprofilePic = new UserProfilePic();
               var sCoverPhoto = _dashBoardservice.GetCoverPhoto(ProfileId);
                DPM.coverPhoto.CoverPhotoId = sCoverPhoto.CoverPhotoId;
                DPM.coverPhoto.CoverPhotoURL = sCoverPhoto.CoverPhotoURL;

                var sProfilePic = _dashBoardservice.GetProfilePic(ProfileId);
                DPM.userprofilePic.UserProfilePicId = sProfilePic.UserProfilePicId;
                DPM.userprofilePic.UserProfilePicUrl = sProfilePic.UserProfilePicUrl;
            if (From == "getMyProfile")
                {
                    return View(DPM);
                }
                else
                {
                    return PartialView("_SearchResult", DPM);
                }
        }
        [HttpPost]
        public ActionResult SaveProfile(WE.BusinessEntities.DashboardProfileModel DBPM)
        {
            if (Session["UserId"] != null)
            {

                DBPM.PersonalDetails.ProfileId = Convert.ToInt16(Session["UserId"]);
                DBPM.PersonalDetails.Age = CalculateAge(DBPM.PersonalDetails.DateofBirth);
                if (string.IsNullOrEmpty(DBPM.PersonalDetails.Introduction)) { DBPM.PersonalDetails.Introduction = "Hi! While I’m still figuring this all out, here’s something I know for sure - I’m excited to be here! The chance to meet Unique, Engaging, and Interesting people is pretty dang neat. If you think you might fit that thought, drop me a message."; }
                if (DBPM.PersonalDetails.ProfileId != 0)
                {
                    _dashBoardservice.SaveProfile(DBPM);
                }
                return RedirectToAction("GetMyProfile");
            }
            else
            {
                return Redirect("../Home/Index");
            }

        }
        public List<SelectListItem> GetMasterData(string MasterType)
        {
            List<SelectListItem> MasterData = new List<SelectListItem>();
            MasterData = _dashBoardservice.GetMasterData(MasterType); //_dashBoardservice.GetMasterData(MasterType);
            return MasterData;
        }


        public ActionResult SearchCriteria()
        {
            if (Session["UserId"] != null)
            {
            int ProfileId = Convert.ToInt32(Session["UserId"]);
            DBPM.userProfile = new UserProfile();
            DBPM.PersonalDetails = new Profile_PersonalDetails();
            DBPM.PersonalChoices = new Profile_PersonalChoices();
            DBPM.WhoAmI = new Profile_WhoAmI();
            DBPM.SelfDescription = new Profile_SelfDescription();
            DBPM.AboutMySelf = new Profile_AboutMySelf();
            DBPM.PersonalViews = new Profile_PersonalViews();
            DBPM.Hobbies = new Profile_Hobbies();
            DBPM.profilePhotos = new List<ProfilePhoto>();
            DBPM.coverPhoto = new Coverphoto();
            DBPM.userprofilePic = new UserProfilePic();
            DBPM.PersonalDetails.CityOpti = _dashBoardservice.GetCity();
            DBPM.PersonalDetails.StateOpti = _dashBoardservice.GetState();
            DBPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();           
            DBPM.AboutMySelf.SubCasteOpti = _dashBoardservice.GetSubCaste();
            DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
            DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
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
            DBPM.PersonalViews.SuperstitiousOpti = GetMasterData("SUPERSTITIOUS_TYPE");
            DBPM.PersonalViews.AstrologyOpti = GetMasterData("ASTROLOGY_TYPE");
            DBPM.PersonalViews.FoodOpti = GetMasterData("FOOD_TYPE");
            DBPM.PersonalViews.OutsideEatingOpti = GetMasterData("OUTSIDEEATING_TYPE");
            DBPM.PersonalViews.MoviesOpti = GetMasterData("MOVIES_TYPE");
            DBPM.PersonalViews.OnRelaxingOpti = GetMasterData("ONRELAXING_TYPE");
            DBPM.PersonalViews.TravelOpti = GetMasterData("TRAVEL_TYPE");
            DBPM.PersonalViews.ShoppingOpti = GetMasterData("SHOPPING_TYPE");
            DBPM.PersonalViews.SpendingOpti = GetMasterData("SPENDING_TYPE");
            DBPM.PersonalViews.BeingSelfOpti = GetMasterData("BEINGSELF_TYPE");
            DBPM.PersonalViews.HumorOpti = GetMasterData("HUMOR_TYPE");
            DBPM.PersonalViews.AnxiousOpti = GetMasterData("ANXIOUS_TYPE");
            DBPM.PersonalViews.TensionOpti = GetMasterData("TENSION_TYPE");
            DBPM.PersonalViews.SpeakingMindOpti = GetMasterData("SPEAKINGMYMIND_TYPE");
            DBPM.PersonalViews.SadTimesOpti = GetMasterData("SADTIMES_TYPE");
            DBPM.PersonalViews.AngryOpti = GetMasterData("ANGRY_TYPE");
            DBPM.PersonalViews.TalkativeOpti = GetMasterData("TALKATIVE_TYPE");
            DBPM.PersonalViews.FateOpti = GetMasterData("FATE_TYPE");
            DBPM.PersonalViews.SelfControlOpti = GetMasterData("SELFCONTROL_TYPE");
            DBPM.PersonalViews.MisUnderstoodOpti = GetMasterData("MISUNDERSTOOD_TYPE");
            DBPM.PersonalViews.AchieveGoalsOpti = GetMasterData("ACHIEVEGOALS_TYPE");
            DBPM.PersonalViews.CaringOpti = GetMasterData("CARING_TYPE");
            DBPM.PersonalViews.SystematicOpti = GetMasterData("SYSTEMATIC_TYPE");
            DBPM.PersonalViews.CreativityOpti = GetMasterData("CREATIVITY_TYPE");
            DBPM.PersonalViews.ForgivingOpti = GetMasterData("FORGIVING_TYPE");
            DBPM.PersonalViews.SensitivityOpti = GetMasterData("SENSITIVITY_TYPE");
            DBPM.PersonalViews.AdmittingFaultsOpti = GetMasterData("ADMITTINGFAULTS_TYPE");
            DBPM.PersonalViews.EgoOpti = GetMasterData("EGO_TYPE");
            DBPM.PersonalViews.FriendshipOpti = GetMasterData("FRIENDSHIP_TYPE");
            DBPM.PersonalViews.DailyChoresOpti = GetMasterData("DAILYCHORES_TYPE");
            DBPM.PersonalViews.ToleranceOpti = GetMasterData("TOLERANCE_TYPE");
            DBPM.PersonalViews.TechSavvyOpti = GetMasterData("TECHSAVVY_TYPE");
            DBPM.PersonalViews.InternetOpti = GetMasterData("INTERNET_TYPE");
            DBPM.PersonalViews.PoliticsOpti = GetMasterData("POLITICS_TYPE");
            DBPM.PersonalViews.SocietyOpti = GetMasterData("SOCIETY_TYPE");
            DBPM.PersonalViews.TakingInitiativeOpti = GetMasterData("TAKINGINITIATIVE_TYPE");
            DBPM.PersonalViews.FamilyValuesOpti = GetMasterData("FAMILYVALUES_TYPE");
            DBPM.PersonalChoices.GoalsOpti = GetMasterData("PERSONALGOALS_TYPE");
            DBPM.PersonalChoices.FriendsNetworkOpti = GetMasterData("FRIENDSNETWORK_TYPE");
            DBPM.PersonalChoices.FriendLaughOpti = GetMasterData("FRIENDSLAUGH_TYPE");
            DBPM.PersonalChoices.HouseImprovementsOpti = GetMasterData("HOUSEREPAIRS_TYPE");
            DBPM.PersonalChoices.CareAboutOpti = GetMasterData("VOLUNTERINGTIME_TYPE");
            DBPM.PersonalChoices.OrganizedLifeOpti = GetMasterData("ORGANIZEDLIFE_TYPE");
            DBPM.PersonalChoices.FinancesOpti = GetMasterData("FINANCES_TYPE");
            DBPM.PersonalChoices.HomeEntertainingOpti = GetMasterData("HOMEENTERTAIN_TYPE");
            DBPM.PersonalChoices.CaringforChildrenOpti = GetMasterData("CHILDRENCARE_TYPE");
            DBPM.PersonalChoices.RomanceinRelationOpti = GetMasterData("ROMANCE_TYPE");
            DBPM.PersonalChoices.SocializingOpti = GetMasterData("SOCIALIZING_TYPE");
            DBPM.PersonalChoices.HomeEnvironmentOpti = GetMasterData("HOMEENVIRONMENT_TYPE");
            DBPM.PersonalChoices.SharingBeliefsOpti = GetMasterData("BELIEFS_TYPE");
            DBPM.PersonalChoices.PhysicalFitOpti = GetMasterData("PHYSICALFIT_TYPE");
            DBPM.PersonalChoices.CalmDuringCrisisOpti = GetMasterData("CALM_TYPE");
            DBPM.PersonalChoices.ThoughtsAndfeelingsOpti = GetMasterData("FEELINGS_TYPE");
            DBPM.PersonalChoices.HelpingFortunatesOpti = GetMasterData("LESSFORTUNATE_TYPE");
            DBPM.PersonalChoices.ResolveConflictOpti = GetMasterData("CONFLICT_TYPE");
            DBPM.PersonalChoices.AdventuresOpti = GetMasterData("ADVENTURES_TYPE");
            DBPM.PersonalChoices.KnowledgeAndAwarenessOpti = GetMasterData("KNOWLEDGE_TYPE");
            DBPM.PersonalChoices.ProfessionalPlanningOpti = GetMasterData("PERSONALPLANNING_TYPE");
            DBPM.PersonalChoices.EventsUnderstandingOpti = GetMasterData("WORLDEVENTS_TYPE");
            DBPM.PersonalChoices.FindingPleasureOpti = GetMasterData("SIMPLETHINGS_TYPE");
            DBPM.PersonalChoices.CultureAndTraditionOpti = GetMasterData("TRADITION_TYPE");
            DBPM.PersonalChoices.CreativeSolutionsOpti = GetMasterData("CREATIVESOLUTIONS_TYPE");
            DBPM.PersonalChoices.MakingFriendsOpti = GetMasterData("NEWFRIENDS_TYPE");
            DBPM.PersonalChoices.CookingForFamilyOpti = GetMasterData("FAMILYCOOKING_TYPE");
            DBPM.PersonalChoices.ProvideIncomeforFamilyOpti = GetMasterData("EARNINGINCOME_TYPE");
            DBPM.PersonalChoices.HouseholdSchedulesOpti = GetMasterData("HOUSEHOLD_TYPE");
            DBPM.PersonalChoices.BeingAgoodFriendOpti = GetMasterData("GOODFRIEND_TYPE");
            DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
            FillData(ProfileId);
             var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
             DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
            return View(DBPM);
               
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
        [HttpPost]
        public ActionResult SearchCriteria(string SeekingGender, string Religion, string Language, string Caste,string AgeFrom, string AgeTo, string HFrom, string HTo, string Country, string RelationshipStatus, string Job, string Education, string Smoke, string Drink, string Kids, string FaithInGod, string Pets, string Travel, string Movies, string Shopping, int PageIndex)
        {
            if (Session["UserId"] != null)
            {
                DashboardProfileModel DPM = new DashboardProfileModel();
                DPM.UserProfilelistdetails = new List<UserProfile>();
                DPM.UserProfileCompatablelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);

                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;
                int ctotalRecord = 0;
                int ctotalPage = 0;
                totalRecord = _dashBoardservice.SearchCriteria(SeekingGender, Religion, Language, Caste, AgeFrom, AgeTo, HFrom, HTo, Country, RelationshipStatus, Job, Education, Smoke, Drink, Kids, FaithInGod, Pets, Travel, Movies, Shopping, ProfileId).Count();
            
                var DSM = _dashBoardservice.SearchCriteria(SeekingGender, Religion, Language, Caste, AgeFrom, AgeTo, HFrom, HTo, Country, RelationshipStatus, Job, Education, Smoke, Drink, Kids, FaithInGod, Pets, Travel, Movies, Shopping, ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                foreach (var li in DSM)
                {
                    if (li.ProfileId != ProfileId)
                    {
                        UserProfile up = new UserProfile();
                        up.ProfileId = li.ProfileId;
                        up.ProfileName = li.ProfileName;
                        up.Gender = (int)li.Gender;
                        up.SeekingGender = (int)li.SeekingGender;
                        up.Age = Convert.ToInt16(li.Age);
                        up.Caste = li.Caste;
                        up.SubCaste = li.SubCaste;
                        up.City = li.City;
                        up.Country = li.Country;
                        up.State = li.State;
                        up.Religion = li.Religion;
                        up.Height = li.Height;
                        up.MaritialStatus = li.MaritialStatus;
                        up.LanguagesKnown = li.LanguagesKnown;
                        up.Occupation = li.Occupation;
                        up.Introduction = li.Introduction;
                        up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                        up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                        DPM.UserProfilelistdetails.Add(up);
                    }
                }
                if (totalRecord <= 5)
                {
                ctotalRecord = _dashBoardservice.GetSearchCompatableMatches(Convert.ToInt16(SeekingGender), AgeFrom, AgeTo, Religion, Caste, Language, ProfileId).Count();
                var cDSM = _dashBoardservice.GetSearchCompatableMatches(Convert.ToInt16(SeekingGender), AgeFrom, AgeTo, Religion, Caste, Language, ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                ctotalPage = (ctotalRecord / pageSize) + ((ctotalRecord % pageSize) > 0 ? 1 : 0);
                //var mDSM= cDSM.Except(DSM).ToList();
                var mDSM= DSM.Where(item => cDSM.Contains(item));
                foreach (var li in mDSM)
                {

                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.Gender = (int)li.Gender;
                    up.SeekingGender = (int)li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    DPM.UserProfileCompatablelistdetails.Add(up);
                }
            }
                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                ViewBag.Sourcepage = "AdvancedSearch";
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
                DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
                return PartialView("_SearchResult", DPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
        public ActionResult ProfilebyId(string vprofilId)
        {
            if (Session["UserId"] != null)
            {
                string[] splitString = vprofilId.Split('!');
                string vconvId = splitString[0].Trim();
                //vid is which profile you have to show
                int vId = Convert.ToInt32(vconvId);
                int pId = Convert.ToInt32(Session["UserId"]);
                DashboardProfileModel DPM = new DashboardProfileModel();
                DPM.userProfile = new UserProfile();
                DPM.PersonalDetails = new Profile_PersonalDetails();
                DPM.PersonalChoices = new Profile_PersonalChoices();
                DPM.WhoAmI = new Profile_WhoAmI();
                DPM.SelfDescription = new Profile_SelfDescription();
                DPM.AboutMySelf = new Profile_AboutMySelf();               
                DPM.PersonalViews = new Profile_PersonalViews();
                DPM.Hobbies = new Profile_Hobbies();
                DPM.profilePhotos = new List<ProfilePhoto>();
                DPM.coverPhoto = new Coverphoto();
                DPM.userprofilePic = new UserProfilePic();
                DPM.AboutMySelf.CasteOpti = new List<SelectListItem>();
                //
              
                DPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();

                DPM.AboutMySelf.SubCasteOpti = _dashBoardservice.GetSubCaste();
                DPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DPM.AboutMySelf.HeightOpti = _dashBoardservice.GetHeight();
                DPM.AboutMySelf.RelationshipStatusOpti = GetMasterData("RELATIONSHIPSTATUS_TYPE");
                DPM.AboutMySelf.EducationOpti = GetMasterData("EDUCATION_TYPE");
                DPM.AboutMySelf.ProfessionOpti = GetMasterData("PROFESSION_TYPE");
                DPM.AboutMySelf.JobOpti = GetMasterData("PROFESSION_TYPE");
                DPM.AboutMySelf.KidsOpti = GetMasterData("KIDS_TYPE");
                DPM.AboutMySelf.PetsOpti = GetMasterData("CHILDREN_TYPE");
                DPM.AboutMySelf.SmokeOpti = GetMasterData("SMOKE_TYPE");
                DPM.AboutMySelf.DrinkOpti = GetMasterData("DRINK_TYPE");
                DPM.AboutMySelf.EthnicityOpti = _dashBoardservice.GetEthnicity();
                DPM.AboutMySelf.ProfessionOpti = _dashBoardservice.GetProfession();
                //DPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion);
                //DPM.AboutMySelf.Language = Convert.ToInt16(data.AboutMySelf.Language);
              
                //DPM.AboutMySelf.Caste = Convert.ToInt16(DPM.AboutMySelf.Caste);
            
                //
                DPM.PersonalDetails.JobCityOpti = _dashBoardservice.GetCity();
                DPM.PersonalDetails.JobStateOpti = _dashBoardservice.GetState();
                DPM.PersonalDetails.JobCountryOpti = _dashBoardservice.GetCountry();
                DPM.PersonalViews.FaithInGodOpti = GetMasterData("FAITHINGOD_TYPE");
                DPM.PersonalViews.ReligionOpti = GetMasterData("RELIGION_TYPE");
                DPM.PersonalViews.CasteOpti = GetMasterData("CASTE_TYPE");

                DPM.PersonalViews.SuperstitiousOpti = GetMasterData("SUPERSTITIOUS_TYPE");
                DPM.PersonalViews.AstrologyOpti = GetMasterData("ASTROLOGY_TYPE");
                DPM.PersonalViews.FoodOpti = GetMasterData("FOOD_TYPE");
                DPM.PersonalViews.OutsideEatingOpti = GetMasterData("OUTSIDEEATING_TYPE");
                DPM.PersonalViews.MoviesOpti = GetMasterData("MOVIES_TYPE");
                DPM.PersonalViews.OnRelaxingOpti = GetMasterData("ONRELAXING_TYPE");
                DPM.PersonalViews.TravelOpti = GetMasterData("TRAVEL_TYPE");
                DPM.PersonalViews.ShoppingOpti = GetMasterData("SHOPPING_TYPE");
                DPM.PersonalViews.SpendingOpti = GetMasterData("SPENDING_TYPE");
                DPM.PersonalViews.BeingSelfOpti = GetMasterData("BEINGSELF_TYPE");
                DPM.PersonalViews.HumorOpti = GetMasterData("HUMOR_TYPE");
                DPM.PersonalViews.AnxiousOpti = GetMasterData("ANXIOUS_TYPE");
                DPM.PersonalViews.TensionOpti = GetMasterData("TENSION_TYPE");
                DPM.PersonalViews.SpeakingMindOpti = GetMasterData("SPEAKINGMYMIND_TYPE");
                DPM.PersonalViews.SadTimesOpti = GetMasterData("SADTIMES_TYPE");
                DPM.PersonalViews.AngryOpti = GetMasterData("ANGRY_TYPE");
                DPM.PersonalViews.TalkativeOpti = GetMasterData("TALKATIVE_TYPE");
                DPM.PersonalViews.FateOpti = GetMasterData("FATE_TYPE");
                DPM.PersonalViews.SelfControlOpti = GetMasterData("SELFCONTROL_TYPE");
                DPM.PersonalViews.MisUnderstoodOpti = GetMasterData("MISUNDERSTOOD_TYPE");
                DPM.PersonalViews.AchieveGoalsOpti = GetMasterData("ACHIEVEGOALS_TYPE");
                DPM.PersonalViews.CaringOpti = GetMasterData("CARING_TYPE");
                DPM.PersonalViews.SystematicOpti = GetMasterData("SYSTEMATIC_TYPE");
                DPM.PersonalViews.CreativityOpti = GetMasterData("CREATIVITY_TYPE");
                DPM.PersonalViews.ForgivingOpti = GetMasterData("FORGIVING_TYPE");
                DPM.PersonalViews.SensitivityOpti = GetMasterData("SENSITIVITY_TYPE");
                DPM.PersonalViews.AdmittingFaultsOpti = GetMasterData("ADMITTINGFAULTS_TYPE");
                DPM.PersonalViews.EgoOpti = GetMasterData("EGO_TYPE");
                DPM.PersonalViews.FriendshipOpti = GetMasterData("FRIENDSHIP_TYPE");
                DPM.PersonalViews.DailyChoresOpti = GetMasterData("DAILYCHORES_TYPE");
                DPM.PersonalViews.ToleranceOpti = GetMasterData("TOLERANCE_TYPE");
                DPM.PersonalViews.TechSavvyOpti = GetMasterData("TECHSAVVY_TYPE");
                DPM.PersonalViews.InternetOpti = GetMasterData("INTERNET_TYPE");
                DPM.PersonalViews.PoliticsOpti = GetMasterData("POLITICS_TYPE");
                DPM.PersonalViews.SocietyOpti = GetMasterData("SOCIETY_TYPE");
                DPM.PersonalViews.TakingInitiativeOpti = GetMasterData("TAKINGINITIATIVE_TYPE");
                DPM.PersonalViews.FamilyValuesOpti = GetMasterData("FAMILYVALUES_TYPE");
                DPM.PersonalChoices.GoalsOpti = GetMasterData("PERSONALGOALS_TYPE");
                DPM.PersonalChoices.FriendsNetworkOpti = GetMasterData("FRIENDSNETWORK_TYPE");
                DPM.PersonalChoices.FriendLaughOpti = GetMasterData("FRIENDSLAUGH_TYPE");
                DPM.PersonalChoices.HouseImprovementsOpti = GetMasterData("HOUSEREPAIRS_TYPE");
                DPM.PersonalChoices.CareAboutOpti = GetMasterData("VOLUNTERINGTIME_TYPE");
                DPM.PersonalChoices.OrganizedLifeOpti = GetMasterData("ORGANIZEDLIFE_TYPE");
                DPM.PersonalChoices.FinancesOpti = GetMasterData("FINANCES_TYPE");
                DPM.PersonalChoices.HomeEntertainingOpti = GetMasterData("HOMEENTERTAIN_TYPE");
                DPM.PersonalChoices.CaringforChildrenOpti = GetMasterData("CHILDRENCARE_TYPE");
                DPM.PersonalChoices.RomanceinRelationOpti = GetMasterData("ROMANCE_TYPE");
                DPM.PersonalChoices.SocializingOpti = GetMasterData("SOCIALIZING_TYPE");
                DPM.PersonalChoices.HomeEnvironmentOpti = GetMasterData("HOMEENVIRONMENT_TYPE");
                DPM.PersonalChoices.SharingBeliefsOpti = GetMasterData("BELIEFS_TYPE");
                DPM.PersonalChoices.PhysicalFitOpti = GetMasterData("PHYSICALFIT_TYPE");
                DPM.PersonalChoices.CalmDuringCrisisOpti = GetMasterData("CALM_TYPE");
                DPM.PersonalChoices.ThoughtsAndfeelingsOpti = GetMasterData("FEELINGS_TYPE");
                DPM.PersonalChoices.HelpingFortunatesOpti = GetMasterData("LESSFORTUNATE_TYPE");
                DPM.PersonalChoices.ResolveConflictOpti = GetMasterData("CONFLICT_TYPE");
                DPM.PersonalChoices.AdventuresOpti = GetMasterData("ADVENTURES_TYPE");
                DPM.PersonalChoices.KnowledgeAndAwarenessOpti = GetMasterData("KNOWLEDGE_TYPE");
                DPM.PersonalChoices.ProfessionalPlanningOpti = GetMasterData("PERSONALPLANNING_TYPE");
                DPM.PersonalChoices.EventsUnderstandingOpti = GetMasterData("WORLDEVENTS_TYPE");
                DPM.PersonalChoices.FindingPleasureOpti = GetMasterData("SIMPLETHINGS_TYPE");
                DPM.PersonalChoices.CultureAndTraditionOpti = GetMasterData("TRADITION_TYPE");
                DPM.PersonalChoices.CreativeSolutionsOpti = GetMasterData("CREATIVESOLUTIONS_TYPE");
                DPM.PersonalChoices.MakingFriendsOpti = GetMasterData("NEWFRIENDS_TYPE");
                DPM.PersonalChoices.CookingForFamilyOpti = GetMasterData("FAMILYCOOKING_TYPE");
                DPM.PersonalChoices.ProvideIncomeforFamilyOpti = GetMasterData("EARNINGINCOME_TYPE");
                DPM.PersonalChoices.HouseholdSchedulesOpti = GetMasterData("HOUSEHOLD_TYPE");
                DPM.PersonalChoices.BeingAgoodFriendOpti = GetMasterData("GOODFRIEND_TYPE");
                //
                var data = _dashBoardservice.GetUserProfile(vId, pId);
                if (data.PersonalDetails != null)
                {
                    DPM.PersonalDetails.ProfileId = data.PersonalDetails.ProfileId;
                    DPM.PersonalDetails.ProfileName = data.PersonalDetails.ProfileName;
                    DPM.PersonalDetails.ProfileLastName = data.PersonalDetails.ProfileLastName;

                    DPM.PersonalDetails.DateofBirth = Convert.ToString(data.PersonalDetails.DateofBirth);
                    DPM.PersonalDetails.Email = data.PersonalDetails.Email;
                    //
                    DPM.PersonalDetails.CountryOpti = _dashBoardservice.GetCountry();
                    DPM.PersonalDetails.Country = data.PersonalDetails.Country;
                    string countryid = DPM.PersonalDetails.Country != null ? DPM.PersonalDetails.Country : null;
                    DPM.PersonalDetails.StateOpti = _dashBoardservice.States(Convert.ToInt16(countryid));
                    DPM.PersonalDetails.State = data.PersonalDetails.State;
                    string Stateid = DPM.PersonalDetails.State != null ? DPM.PersonalDetails.State : null;
                    DPM.PersonalDetails.CityOpti = _dashBoardservice.Cities(Convert.ToInt16(Stateid));
                    DPM.PersonalDetails.City = data.PersonalDetails.City;
                    DPM.PersonalDetails.Zipcode = data.PersonalDetails.Zipcode;
                    // job
                    DPM.PersonalDetails.IsSameasAbove = data.PersonalDetails.IsSameasAbove;
                    DPM.PersonalDetails.JobCountryOpti = _dashBoardservice.GetCountry();
                    if (data.PersonalDetails.JobCountry != null) { DPM.PersonalDetails.JobCountry = data.PersonalDetails.JobCountry.Replace(" ", String.Empty); }                   
                    string jobcountryid = DPM.PersonalDetails.JobCountry != null ? DPM.PersonalDetails.JobCountry : null;
                    DPM.PersonalDetails.JobStateOpti = _dashBoardservice.States(Convert.ToInt16(jobcountryid));
                    if (data.PersonalDetails.JobState != null) { DPM.PersonalDetails.JobState = data.PersonalDetails.JobState.Replace(" ", String.Empty); }                    
                    string jobStateid = DPM.PersonalDetails.JobState != null ? DPM.PersonalDetails.JobState : null;
                    DPM.PersonalDetails.JobCityOpti = _dashBoardservice.Cities(Convert.ToInt16(jobStateid));
                    if (data.PersonalDetails.JobCity!=null) { DPM.PersonalDetails.JobCity = data.PersonalDetails.JobCity.Replace(" ", String.Empty); }                    
                    DPM.PersonalDetails.JobZipcode = data.PersonalDetails.JobZipcode;
                    DPM.PersonalDetails.SocialMediaLinks = data.PersonalDetails.SocialMediaLinks;
                }
                ///
                if (data.userProfile != null)
                {
                    DPM.userProfile.IsFavorate = Convert.ToInt16(data.userProfile.IsFavorate);
                    DPM.AboutMySelf.IntroductionAboutMyself = data.AboutMySelf.IntroductionAboutMyself;
                    DPM.AboutMySelf.dHeight = data.AboutMySelf.Height;
                    DPM.AboutMySelf.dRelationshipStatus = data.AboutMySelf.RelationshipStatus;
                    DPM.AboutMySelf.dKids = data.AboutMySelf.Kids;
                    DPM.AboutMySelf.dPets = data.AboutMySelf.Pets;
                    DPM.AboutMySelf.dWantChildren = data.AboutMySelf.WantChildren;
                    DPM.AboutMySelf.dEducation = data.AboutMySelf.Education;
                    DPM.AboutMySelf.dSmoke = data.AboutMySelf.Smoke;
                    DPM.AboutMySelf.dDrink = data.AboutMySelf.Drink;
                    DPM.AboutMySelf.dEthnicity = data.AboutMySelf.Ethnicity;
                    DPM.AboutMySelf.dReligion = data.AboutMySelf.Religion;
                    DPM.AboutMySelf.Language = Convert.ToInt16(data.AboutMySelf.Language);
                    DPM.AboutMySelf.CasteOpti = _dashBoardservice.GetCaste(DPM.AboutMySelf.Language);
                    if (data.AboutMySelf.Caste != "") { DPM.AboutMySelf.Caste = Convert.ToInt16(data.AboutMySelf.Caste); }
                    DPM.AboutMySelf.sJob = data.AboutMySelf.Job; 
                  
                }
                if (data.WhoAmI != null)
                {
                    DPM.WhoAmI.ThankkfulFor = data.WhoAmI.ThankkfulFor;
                    DPM.WhoAmI.LifeSkills = data.WhoAmI.LifeSkills;
                    DPM.WhoAmI.ThingsCantLiveWithout = data.WhoAmI.ThingsCantLiveWithout;
                    DPM.WhoAmI.FriendsDescribedAs = data.WhoAmI.FriendsDescribedAs;
                    DPM.Hobbies.CarsAndDriving = data.Hobbies.CarsAndDriving;
                    DPM.Hobbies.SpendFreeTime = data.Hobbies.SpendFreeTime;
                    DPM.Hobbies.SportsILike = data.Hobbies.SportsILike;
                    DPM.Hobbies.InstrumentsIPlay = data.Hobbies.InstrumentsIPlay;
                    DPM.Hobbies.Music = data.Hobbies.Music;
                    DPM.Hobbies.Vacations = data.Hobbies.Vacations;
                    DPM.Hobbies.Favorite = data.Hobbies.Favorite;
                    DPM.Hobbies.TVShows = data.Hobbies.TVShows;
                    DPM.Hobbies.Movies = data.Hobbies.Movies;
                    DPM.Hobbies.Books = data.Hobbies.Books;
                    DPM.Hobbies.Travel = data.Hobbies.Travel;
                    DPM.Hobbies.Excercising = data.Hobbies.Excercising;
                    DPM.Hobbies.Hiking = data.Hobbies.Hiking;
                    DPM.Hobbies.Driving = data.Hobbies.Driving;
                    DPM.Hobbies.Beaches = data.Hobbies.Beaches;
                }
                //
                // 
                if (data.PersonalChoices != null)
                {
                    DPM.PersonalChoices.Goals = Convert.ToInt16(data.PersonalChoices.Goals);
                    DPM.PersonalChoices.FriendsNetwork = Convert.ToInt16(data.PersonalChoices.FriendsNetwork);
                    DPM.PersonalChoices.FriendLaugh = Convert.ToInt16(data.PersonalChoices.FriendLaugh);
                    DPM.PersonalChoices.HouseImprovements = Convert.ToInt16(data.PersonalChoices.HouseImprovements);
                    DPM.PersonalChoices.CareAbout = Convert.ToInt16(data.PersonalChoices.CareAbout);
                    DPM.PersonalChoices.OrganizedLife = Convert.ToInt16(data.PersonalChoices.OrganizedLife);
                    DPM.PersonalChoices.Finances = Convert.ToInt16(data.PersonalChoices.Finances);
                    DPM.PersonalChoices.HomeEntertaining = Convert.ToInt16(data.PersonalChoices.HomeEntertaining);
                    DPM.PersonalChoices.CaringforChildren = Convert.ToInt16(data.PersonalChoices.CaringforChildren);
                    DPM.PersonalChoices.RomanceinRelation = Convert.ToInt16(data.PersonalChoices.RomanceinRelation);
                    DPM.PersonalChoices.Socializing = Convert.ToInt16(data.PersonalChoices.Socializing);
                    DPM.PersonalChoices.HomeEnvironment = Convert.ToInt16(data.PersonalChoices.HomeEnvironment);
                    DPM.PersonalChoices.SharingBeliefs = Convert.ToInt16(data.PersonalChoices.SharingBeliefs);
                    DPM.PersonalChoices.PhysicalFit = Convert.ToInt16(data.PersonalChoices.PhysicalFit);
                    DPM.PersonalChoices.CalmDuringCrisis = Convert.ToInt16(data.PersonalChoices.CalmDuringCrisis);
                    DPM.PersonalChoices.ThoughtsAndfeelings = Convert.ToInt16(data.PersonalChoices.ThoughtsAndfeelings);
                    DPM.PersonalChoices.HelpingFortunates = Convert.ToInt16(data.PersonalChoices.HelpingFortunates);
                    DPM.PersonalChoices.ResolveConflict = Convert.ToInt16(data.PersonalChoices.ResolveConflict);
                    DPM.PersonalChoices.Adventures = Convert.ToInt16(data.PersonalChoices.Adventures);
                    DPM.PersonalChoices.KnowledgeAndAwareness = Convert.ToInt16(data.PersonalChoices.KnowledgeAndAwareness);
                    DPM.PersonalChoices.ProfessionalPlanning = Convert.ToInt16(data.PersonalChoices.ProfessionalPlanning);
                    DPM.PersonalChoices.EventsUnderstanding = Convert.ToInt16(data.PersonalChoices.EventsUnderstanding);
                    DPM.PersonalChoices.FindingPleasure = Convert.ToInt16(data.PersonalChoices.FindingPleasure);
                    DPM.PersonalChoices.CultureAndTradition = Convert.ToInt16(data.PersonalChoices.CultureAndTradition);
                    DPM.PersonalChoices.CreativeSolutions = Convert.ToInt16(data.PersonalChoices.CreativeSolutions);
                    DPM.PersonalChoices.MakingFriends = Convert.ToInt16(data.PersonalChoices.MakingFriends);
                    DPM.PersonalChoices.CookingForFamily = Convert.ToInt16(data.PersonalChoices.CookingForFamily);
                    DPM.PersonalChoices.ProvideIncomeforFamily = Convert.ToInt16(data.PersonalChoices.ProvideIncomeforFamily);
                    DPM.PersonalChoices.HouseholdSchedules = Convert.ToInt16(data.PersonalChoices.HouseholdSchedules);
                    DPM.PersonalChoices.BeingAgoodFriend = Convert.ToInt16(data.PersonalChoices.BeingAgoodFriend);
                }
                if (data.SelfDescription != null)
                {
                    DPM.SelfDescription.PeopleNoticeFirstThing = data.SelfDescription.PeopleNoticeFirstThing;
                    DPM.SelfDescription.MostThankful = data.SelfDescription.MostThankful;
                    DPM.SelfDescription.MostInfluentPerson = data.SelfDescription.MostInfluentPerson;
                    DPM.SelfDescription.CannotLiveWithout = data.SelfDescription.CannotLiveWithout;
                    DPM.SelfDescription.SpendLeisureTime = data.SelfDescription.SpendLeisureTime;
                    DPM.SelfDescription.MostProudOf = data.SelfDescription.MostProudOf;
                    DPM.SelfDescription.LovablePerson = data.SelfDescription.LovablePerson;
                    DPM.SelfDescription.ImportantQualityofaPerson = data.SelfDescription.ImportantQualityofaPerson;
                    DPM.SelfDescription.AdditionalInfo = data.SelfDescription.AdditionalInfo;
                    DPM.PersonalViews.FaithInGod = data.PersonalViews.FaithInGod;

                }
                if (data.PersonalViews != null)
                {

                    DPM.PersonalViews.Religion = data.PersonalViews.Religion;
                    DPM.PersonalViews.Caste = data.PersonalViews.Caste;
                    DPM.PersonalViews.Superstitious = data.PersonalViews.Superstitious;
                    DPM.PersonalViews.Astrology = data.PersonalViews.Astrology;
                    DPM.PersonalViews.Food = data.PersonalViews.Food;
                    DPM.PersonalViews.OutsideEating = data.PersonalViews.OutsideEating;
                    DPM.PersonalViews.Movies = data.PersonalViews.Movies;
                    DPM.PersonalViews.OnRelaxing = data.PersonalViews.OnRelaxing;
                    DPM.PersonalViews.Travel = data.PersonalViews.Travel;
                    DPM.PersonalViews.Shopping = data.PersonalViews.Shopping;
                    DPM.PersonalViews.Spending = data.PersonalViews.Spending;
                    DPM.PersonalViews.BeingSelf = data.PersonalViews.BeingSelf;
                    DPM.PersonalViews.Humor = data.PersonalViews.Humor;
                    DPM.PersonalViews.Anxious = data.PersonalViews.Anxious;
                    DPM.PersonalViews.Tension = data.PersonalViews.Tension;
                    DPM.PersonalViews.SpeakingMind = data.PersonalViews.SpeakingMind;
                    DPM.PersonalViews.SadTimes = data.PersonalViews.SadTimes;
                    DPM.PersonalViews.Angry = data.PersonalViews.Angry;
                    DPM.PersonalViews.Talkative = data.PersonalViews.Talkative;
                    DPM.PersonalViews.Fate = data.PersonalViews.Fate;
                    DPM.PersonalViews.SelfControl = data.PersonalViews.SelfControl;
                    DPM.PersonalViews.MisUnderstood = data.PersonalViews.MisUnderstood;
                    DPM.PersonalViews.AchieveGoals = data.PersonalViews.AchieveGoals;
                    DPM.PersonalViews.Caring = data.PersonalViews.Caring;
                    DPM.PersonalViews.Systematic = data.PersonalViews.Systematic;
                    DPM.PersonalViews.Creativity = data.PersonalViews.Creativity;
                    DPM.PersonalViews.Forgiving = data.PersonalViews.Forgiving;
                    DPM.PersonalViews.Sensitivity = data.PersonalViews.Sensitivity;
                    DPM.PersonalViews.AdmittingFaults = data.PersonalViews.AdmittingFaults;
                    DPM.PersonalViews.Ego = data.PersonalViews.Ego;
                    DPM.PersonalViews.Friendship = data.PersonalViews.Friendship;
                    DPM.PersonalViews.DailyChores = data.PersonalViews.DailyChores;
                    DPM.PersonalViews.Tolerance = data.PersonalViews.Tolerance;
                    DPM.PersonalViews.TechSavvy = data.PersonalViews.TechSavvy;
                    DPM.PersonalViews.Internet = data.PersonalViews.Internet;
                    DPM.PersonalViews.Politics = data.PersonalViews.Politics;
                    DPM.PersonalViews.Society = data.PersonalViews.Society;
                    DPM.PersonalViews.TakingInitiative = data.PersonalViews.TakingInitiative;
                    DPM.PersonalViews.FamilyValues = data.PersonalViews.FamilyValues;
                }
                var sProfilesPhotos = _dashBoardservice.GetProfilePhotos(vId);
                foreach (var li in sProfilesPhotos)
                {
                    ProfilePhoto pl = new ProfilePhoto();
                    pl.PhotoId = li.PhotoId;
                    pl.ProfileId = li.ProfileId;
                    pl.PhotoUrl = li.PhotoUrl;
                    pl.ThumbnailUrl = li.ThumbnailUrl;
                    DPM.profilePhotos.Add(pl);
                }
                var sCoverPhoto = _dashBoardservice.GetCoverPhoto(vId);
                DPM.coverPhoto.CoverPhotoId = sCoverPhoto.CoverPhotoId;
                DPM.coverPhoto.CoverPhotoURL = sCoverPhoto.CoverPhotoURL;

                var sProfilePic = _dashBoardservice.GetProfilePic(vId);
                DPM.userprofilePic.UserProfilePicId = sProfilePic.UserProfilePicId;
                DPM.userprofilePic.UserProfilePicUrl = sProfilePic.UserProfilePicUrl;

                
                var package = _dashBoardservice.GetUserPackageDetails(pId);
                DPM.UserPackageStatus = package.Status==null ? 0:1;
                return View(DPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }


        private static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        private void GenerateThumbnails(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }
        // [HttpPost]
        public JsonResult UploadFiles()
        {
            if (Session["UserId"] != null)
            {
            }
            else {
                Response.Redirect("../Home/Index");
            }
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

                int ProfileId = Convert.ToInt32(Session["UserId"]);
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
        public ActionResult WhoViewedMe()
        {
            if (Session["UserId"] != null)
            {
                DBPM.userProfile = new UserProfile();               
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.UserProfilelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                int PageIndex = 1;

                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                FillData(ProfileId);
                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;
                totalRecord = _dashBoardservice.GetWhoViewedMe(ProfileId).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                var DSM = _dashBoardservice.GetWhoViewedMe(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                foreach (var li in DSM)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                  
                    up.Gender = (int)li.Gender;
                    up.SeekingGender = (int)li.SeekingGender;
                   
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                   
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    DBPM.UserProfilelistdetails.Add(up);
                }
                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                ViewBag.Sourcepage = "WhoViewedMe";
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
                DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
                return View(DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }

        }
        [HttpPost]
        public ActionResult pWhoViewedMe(int PageIndex)
        {
            if (Session["UserId"] != null)
            {
                DBPM.userProfile = new UserProfile();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                //  DashboardProfileModel DPM = new DashboardProfileModel();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.UserProfilelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                if (PageIndex < 0 || PageIndex == 0)
                {
                    PageIndex = 1;
                }
                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                FillData(ProfileId);
                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;
                totalRecord = _dashBoardservice.GetWhoViewedMe(ProfileId).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                var DSM = _dashBoardservice.GetWhoViewedMe(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();
                foreach (var li in DSM)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    // up.Email = li.EmailId;
                    up.Gender = (int)li.Gender;
                    up.SeekingGender = (int)li.SeekingGender;
                    // up.DateofBirth =(DateTime)li.DateofBirth;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    //up.Mobile = li.;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    //up.Zipcode = li.Zipcode;
                    //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                    //up.ProfilePhotoUrl = li.ph
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    DBPM.UserProfilelistdetails.Add(up);
                }
                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                ViewBag.Sourcepage = "WhoViewedMe";
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
                DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
                return PartialView("_SearchResult", DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }

        }

        public ActionResult SetFavourite(string vprofilId)
        {
            if (Session["UserId"] != null)
            {
                string[] splitString = vprofilId.Split('!');

                string vconvId = splitString[0].Trim();
                //vid is which profile you have to showvar
                int vId = Convert.ToInt32(vconvId);
                int pId = Convert.ToInt32(Session["UserId"]);
                var Fav = _dashBoardservice.SetFavourite(pId, vId);
                return Json(Fav, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
        public ActionResult RemFavourite(string vprofilId)
        {
            if (Session["UserId"] != null)
            {
                string[] splitString = vprofilId.Split('!');

                string vconvId = splitString[0].Trim();
                //vid is which profile you have to showvar
                int vId = Convert.ToInt32(vconvId);
                int pId = Convert.ToInt32(Session["UserId"]);
                var Fav = _dashBoardservice.RemFavourite(pId, vId);
                return Json(Fav, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }


        public ActionResult BestLiked(int PageIndex)
        {
            if (Session["UserId"] != null)
            {
                if (PageIndex == 0)
                {
                    PageIndex = 1;
                }

                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;
               
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.UserProfilelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                DBPM.userProfile = new UserProfile();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                FillData(ProfileId);

                totalRecord = _dashBoardservice.GetUserfavourites(ProfileId).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                var DSM = _dashBoardservice.GetUserfavourites(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();

                foreach (var li in DSM)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.Gender = (int)li.Gender;
                    up.SeekingGender = (int)li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = li.IsFavorate;
                    DBPM.UserProfilelistdetails.Add(up);
                }

                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                ViewBag.Sourcepage = "Favorates";
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
                DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
                return View("GetUserFavourite", DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }

        public ActionResult GetUserFavourite(int PageIndex)
        {
            if (Session["UserId"] != null)
            {
                if (PageIndex == 0)
                {
                    PageIndex = 1;
                }

                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;
                //  DashboardProfileModel DPM = new DashboardProfileModel();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.UserProfilelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                totalRecord = _dashBoardservice.GetUserfavourites(ProfileId).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                var DSM = _dashBoardservice.GetUserfavourites(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((PageIndex - 1) * pageSize)).Take(pageSize).ToList();

                foreach (var li in DSM)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    // up.Email = li.EmailId;
                    up.Gender = (int)li.Gender;
                    up.SeekingGender = (int)li.SeekingGender;
                    // up.DateofBirth =(DateTime)li.DateofBirth;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    //up.Mobile = li.;
                    up.City = li.City;
                    up.Country = li.Country;
                    up.State = li.State;
                    up.Religion = li.Religion;
                    //up.Zipcode = li.Zipcode;
                    //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                    //up.ProfilePhotoUrl = li.ph
                    up.Height = li.Height;
                    up.MaritialStatus = li.MaritialStatus;
                    up.LanguagesKnown = li.LanguagesKnown;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                    up.IsFavorate = li.IsFavorate;
                    DBPM.UserProfilelistdetails.Add(up);
                }

                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = PageIndex;
                ViewBag.Sourcepage = "Favorates";
                var package = _dashBoardservice.GetUserPackageDetails(ProfileId);
                DBPM.UserPackageStatus = package.Status == null ? 0 : 1;
                return PartialView("_SearchResult", DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }


        [HttpPost]
        public JsonResult CoverPhotoUpload()
        {
            if (Session["UserId"] != null)
            {
            }
            else { Response.Redirect("../Home/Index"); }
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
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

                int ProfileId = Convert.ToInt32(Session["UserId"]);
                var fileName = Path.GetFileName(file.FileName);
                var _ext = Path.GetExtension(file.FileName);
                string PhotoUrl = _dashBoardservice.InsertCoverPhoto(ProfileId, _ext);
                string ModifiedPhotoUrl = "CoverPhotos_" + PhotoUrl;
                fname = Path.Combine(Server.MapPath("~/CoverPhotos/"), ModifiedPhotoUrl);
                file.SaveAs(fname);

            }
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProfilePicUpload()
        {
            return View();
        }
        [HttpPost]
        public JsonResult saveProfilePic(string image)
        {
            if (Session["UserId"] != null) { }
            else { Response.Redirect("../Home/Index"); }                      
            int ProfileId = Convert.ToInt32(Session["UserId"]);
            string filePath ="ProfilePic_"+ ProfileId + ".jpg";          
            string convert = image.Replace("data:image/png;base64,", String.Empty);
            var bytess = Convert.FromBase64String(convert);
            string fname;
            fname = Path.Combine(Server.MapPath("~/ProfilePics/"), filePath);
           
            using (var imageFile = new FileStream(fname, FileMode.Create))
            {
                imageFile.Write(bytess, 0, bytess.Length);
                imageFile.Flush();
            }
            string custfilepath = "../ProfilePics/" + filePath;
            int sResult = _dashBoardservice.InsertProfilePic(ProfileId, custfilepath);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
        [HttpPost]
        public JsonResult ViewMibileNumber(int vProfileId)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("../Home/Index");
            }
            string Sreturnvalue = string.Empty;
            int ProfileId = Convert.ToInt32(Session["UserId"]);
            var sResult = _dashBoardservice.ViewMobileNumber(vProfileId, ProfileId);
            var sProfile = _dashBoardservice.GetProfilePic(vProfileId);
            var sProfilePicUrl = sProfile.UserProfilePicUrl;
            if (sResult != "Package Not Found") {
                string[] b = sResult.Split('#');
                Sreturnvalue = b[0] + "," + b[1] + "," + sProfilePicUrl;
            } else { Sreturnvalue = "Package Not Found" + "," + sProfilePicUrl;  }
            
            string[] a = Sreturnvalue.Split(',');

            return Json(a, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        public JsonResult GetCastebyLanguage(int LanguageId)
        {
            var CasteList = _dashBoardservice.GetCasteByLanguage(LanguageId);
            var CasteData = CasteList.Select(m => new SelectListItem()
            {
                Text = m.Text.ToString(),
                Value = m.Value.ToString(),
            });
            var nCasteData = CasteData.Select(x => new { Value = x.Value, Text = x.Text });
            return Json(nCasteData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult States(int CountryId)
        {
            var StatesList = _dashBoardservice.States(CountryId);
            var StatesData = StatesList.Select(m => new SelectListItem()
            {
                Text = m.Text.ToString(),
                Value = m.Value.ToString(),
            });
            var nStatesData = StatesData.Select(x => new { Value = x.Value, Text = x.Text });
            return Json(nStatesData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Cities(int StateId)
        {
            var CitiesList = _dashBoardservice.Cities(StateId);
            var CasteData = CitiesList.Select(m => new SelectListItem()
            {
                Text = m.Text.ToString(),
                Value = m.Value.ToString(),
            });
            var nCitiesData = CitiesList.Select(x => new { Value = x.Value, Text = x.Text });
            return Json(nCitiesData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Packages()
        {
            if (Session["UserId"] != null)
            {
                DashboardProfileModel DPM = new DashboardProfileModel();
                DPM.LPackages = new List<Packages>();
                var sPackageDetails = _dashBoardservice.Packages();

                foreach (var li in sPackageDetails)
                {
                    Packages ipkg = new Packages();
                    ipkg.PackageId = li.PackageId;
                    ipkg.PackageName = li.PackageName;
                    ipkg.PackagePrice = li.PackagePrice;
                    ipkg.PerMonth = li.PerMonth;
                    ipkg.PackageDuration = li.PackageDuration;
                    ipkg.ByCountry = li.ByCountry;
                    DPM.LPackages.Add(ipkg);
                }
                return View(DPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
      
        //get user details
        public UserProfile UserProfile(int ProfileId)
        {
            //if (Session["UserId"] == null)
            //{
            //    Response.Redirect("../Home/Index");
            //}
            UserProfile userProfile = new UserProfile();
            //int ProfileId = Convert.ToInt16(Session["UserId"]);
            var data = _dashBoardservice.GetMyProfile(ProfileId);
            userProfile.ProfileId = ProfileId;

            if (userProfile != null)
            {
                userProfile.ProfileName = data.PersonalDetails.ProfileName;
                userProfile.ProfileLastName = data.PersonalDetails.ProfileLastName;
                userProfile.Email = data.userProfile.Email;
                userProfile.DateofBirth = data.userProfile.DateofBirth;
                userProfile.Gender = data.userProfile.Gender;
                userProfile.SeekingGender = data.userProfile.SeekingGender;
                userProfile.Zipcode = data.userProfile.Zipcode;
                userProfile.Country = data.userProfile.Country;
                userProfile.HowDidYouHearAboutUs = data.userProfile.HowDidYouHearAboutUs;
            }
            return userProfile;
        }

        public string Payment()
        {          
                int ProfileId = Convert.ToInt32(Session["UserId"]);
                InsertUserPackage(ProfileId, 9, "Special_Free_3Months", "FREE");
                Session["IsUserAcitive"] = "Active";
                return "success";       

        }

        public string InsertUserPackage(int ProfileId, int PackageId, string Transaction_Id, string Payment_From)
        {
            var data = _dashBoardservice.InsertUserPackage(ProfileId,PackageId,Transaction_Id,Payment_From);
            return data;
        }
        public ActionResult GetUserPackageDetails()
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("../Home/Index");
            }
            UserPackageDetails upd = new UserPackageDetails();
            int ProfileId = Convert.ToInt16(Session["UserId"]);
            var userpkgdet= _dashBoardservice.GetUserPackageDetails(ProfileId);
            upd.ProfileId = userpkgdet.ProfileId;
            upd.PackageId = userpkgdet.PackageId;
            upd.PackageName = userpkgdet.PackageName;
            upd.TrasactionId = userpkgdet.TrasactionId;
            upd.ByCountry = userpkgdet.ByCountry;
            upd.PackageDuration = userpkgdet.PackageDuration;
            upd.PackageStartDate = Convert.ToString(userpkgdet.PackageStartDate);
            upd.PackageEndDate = Convert.ToString(userpkgdet.PackageEndDate);
            upd.PackagePrice = userpkgdet.PackagePrice;
            upd.PayMode = userpkgdet.PayMode;
            upd.Status = userpkgdet.Status;
            return View(upd);
        }

        public void FillData(int ProfileId)
        {
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

        }

       public ActionResult CompatableMatches(int pageIndex,int From)
        {
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
                DBPM.UserProfilelistdetails = new List<UserProfile>();

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
                    if (data.AboutMySelf.Religion != "") { DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion); }                   
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
                //geting full Compatable matches

             
               int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
               int totalPage = 0;
               int totalRecord = 0;
               
                  
                    totalRecord = _dashBoardservice.GetUserCompatableMatches(ProfileId).Count();
                    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                    var DSM = _dashBoardservice.GetUserCompatableMatches(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();

                    foreach (var li in DSM)
                    {
                        UserProfile up = new UserProfile();
                        up.ProfileId = li.ProfileId;
                        up.ProfileName = li.ProfileName;
                        // up.Email = li.EmailId;
                        up.Gender = (int)li.Gender;
                        up.SeekingGender = (int)li.SeekingGender;
                        // up.DateofBirth =(DateTime)li.DateofBirth;
                        up.Age = Convert.ToInt16(li.Age);
                        up.Caste = li.Caste;
                        up.SubCaste = li.SubCaste;
                        //up.Mobile = li.;
                        up.City = li.City;
                        up.Country = li.Country;
                        up.State = li.State;
                        up.Religion = li.Religion;
                        //up.Zipcode = li.Zipcode;
                        //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                        //up.ProfilePhotoUrl = li.ph
                        up.Height = li.Height;
                        up.MaritialStatus = li.MaritialStatus;
                        up.LanguagesKnown = li.LanguagesKnown;
                        up.Occupation = li.Occupation;
                        up.Introduction = li.Introduction;
                        up.ProfilePhotoUrl = li.ProfilePhotoUrl;
                        up.IsFavorate = li.IsFavorate;
                        DBPM.UserProfilelistdetails.Add(up);
                    }

                    ViewBag.dbCount = totalPage;
                    ViewBag.Currentpage = pageIndex;
                    ViewBag.Sourcepage = "CompatableMatches";
                     if (From == 1) { return View(DBPM); } else { return PartialView("_SearchResult", DBPM); }
                    
                }
                else
                {
                    return Redirect("../Home/Index");
                }
        }

        public ActionResult RecentVisitors(int pageIndex, int From)
        {
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
                DBPM.UserProfilelistdetails = new List<UserProfile>();

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
                    if (data.AboutMySelf.Religion != "") { DBPM.AboutMySelf.Religion = Convert.ToInt16(data.AboutMySelf.Religion); }
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
                //geting full Compatable matches


                int pageSize = Convert.ToInt16(ConfigurationManager.AppSettings["pagesize"]);
                int totalPage = 0;
                int totalRecord = 0;


                totalRecord = _dashBoardservice.GettotRecentVisitors(ProfileId).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);

                var DSM = _dashBoardservice.GettotRecentVisitors(ProfileId).OrderByDescending(a => a.ProfileId).Skip(((pageIndex - 1) * pageSize)).Take(pageSize).ToList();

                foreach (var lis in DSM)
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

                ViewBag.dbCount = totalPage;
                ViewBag.Currentpage = pageIndex;
                ViewBag.Sourcepage = "RecentVisitors";



                if (From == 1) { return View(DBPM); } else { return PartialView("_SearchResult", DBPM); }

            }
            else
            {
                return Redirect("../Home/Index");
            }
        }
        public ActionResult ChangePassword()
        {
            DBPM.resetPassword = new ResetPassword();
            int UserId = Convert.ToInt16(Session["UserId"]);
            ResetPassword RP = new ResetPassword();
            DBPM.resetPassword.UserId=UserId;

            if (Session["UserId"] != null)
            {
                DBPM.userProfile = new UserProfile();
                DBPM.PersonalDetails = new Profile_PersonalDetails();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.UserProfilelistdetails = new List<UserProfile>();
                int ProfileId = Convert.ToInt32(Session["UserId"]);         

                DBPM.profilePhotos = new List<ProfilePhoto>();
                DBPM.coverPhoto = new Coverphoto();
                DBPM.userprofilePic = new UserProfilePic();
                DBPM.AboutMySelf = new Profile_AboutMySelf();
                DBPM.AboutMySelf.ReligionOpti = _dashBoardservice.GetReligion();
                DBPM.AboutMySelf.LanguageOpti = _dashBoardservice.GetLanguage();
                DBPM.userProfile.GenderOpti = _dashBoardservice.GetMasterData("IAM_TYPE");
                FillData(ProfileId); 
                return View(DBPM);
            }
            else
            {
                return Redirect("../Home/Index");
            }
            
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SaveChangePassword(string NewPassword)
        {
            var passwordsalt = ConfigurationManager.AppSettings["PasswordSalt"].ToString();
            var pwdEncryptred = EncryptandDecrypt.Encrypt(NewPassword.Trim(), passwordsalt.Trim()); //for encryption
            int sResult = _dashBoardservice.SaveNewPassword(Convert.ToInt16(Session["UserId"]), pwdEncryptred.Trim());
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

    }
}