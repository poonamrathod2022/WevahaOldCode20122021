using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WE.BusinessServices.Interface;
using WE.DataAccess;
using System.Web.Mvc;
using WE.BusinessEntities;
using System.Data.Entity.Core.Objects;
using System.Globalization;

namespace WE.BusinessServices
{
    public class DashBoardService : IDashBoardService
    {
        private WEvahaEntities _entities = new WEvahaEntities();
        public List<SelectListItem> GetMasterData(string param)
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETLOOKUPVALUE(param).ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.LookupValueId),
                    Text = li.LookupValue
                });

            }
            return _masterdata;
        }
       
        public List<SelectListItem> GetCountry()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETCOUNTRY().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.CountryId),
                    Text = li.CountryName
                });

            }
            return _masterdata;
        }
        public List<SelectListItem> GetState()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETSTATE().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.StateId),
                    Text = li.StateName
                });

            }
            return _masterdata;
        }

        public List<SelectListItem> GetCity()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETCITY().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.CityId),
                    Text = li.CityName
                });

            }
            return _masterdata;
        }
        public List<SelectListItem> GetEthnicity()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_ETHNICITY().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.EthnicityId),
                    Text = li.Ethnicity
                });

            }
            return _masterdata;
        }

        public List<SelectListItem> GetReligion()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_RELIGION().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.ReligionId),
                    Text = li.ReligionName
                });

            }
            return _masterdata;
        }

        public List<SelectListItem> GetSubCaste()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_SUB_CASTE().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.SubcasteId),
                    Text = li.SubcasteName
                });

            }
            return _masterdata;
        }

        public List<SelectListItem> GetCaste(int LanguageId)
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_CASTE_BY_LANGUAGE(LanguageId).ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.CasteId),
                    Text = li.CasteName
                });

            }
            return _masterdata;
        }

        //get language
        public List<SelectListItem> GetLanguage()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_LANGUAGE().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.LanguageId),
                    Text = li.LanguageName
                });

            }
            return _masterdata;
        }


        //
        //get casteBy language
        public List<SelectListItem> GetCasteByLanguage(int LanguageId)
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_CASTE_BY_LANGUAGE(LanguageId).ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.CasteId),
                    Text = li.CasteName
                });

            }
            return _masterdata;
        }
        //

        public List<SelectListItem> GetHeight()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_HIGHT().ToList();

            foreach (var li in data)
            {

                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.HeightId),
                    Text = li.Height
                });

            }
            return _masterdata;
        }
        public List<SelectListItem> GetProfession()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GETOCCUPATION().ToList();

            foreach (var li in data)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.PROFESSIONID),
                    Text = li.PROFESSION
                });

            }
            return _masterdata;
        }
      
        public List<SelectListItem> AgeTo()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();           
            for(int i=18;i<80; i++)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(i),
                    Text = Convert.ToString(i)
                });
            }            
            return _masterdata;
        }

        public List<SelectListItem> AgeFrom()
        {

            List<SelectListItem> _masterdata = new List<SelectListItem>();
            for (int i = 18; i < 80; i++)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(i),
                    Text = Convert.ToString(i)
                });
            }
            return _masterdata;
        }


        public List<Packages> Packages()
        {
            List<Packages> _masterdata = new List<Packages>();

            var data = _entities.USP_WE_GET_PACKAGES().ToList();

            foreach (var li in data)
            {
                Packages pg = new Packages();
                pg.PackageId = li.PackageId;
                pg.ByCountry = (int)li.ByCountry;
                pg.PackageName = li.PackageName;
                pg.PackageDuration = (int)li.PackageDuration;
                pg.PackagePrice = string.Format("{0:0.00}", li.PackagePrice);
                pg.PerMonth = string.Format("{0:0.00}", li.PerMonth);
                pg.Status = (int)li.Status;
                _masterdata.Add(pg);
            }
            return _masterdata;
        }

        public List<UserProfile> GetSearch(int SeekingGender,string AgeFrom, string AgeTo, string religion, string caste, string Language, int ProfileId)
       {
            string sReligion = string.Empty; string sCaste = string.Empty; string sLanguage = string.Empty;
            if (religion == "Religion"|| religion=="null"||religion== "undefined") { sReligion = null; } else { sReligion = religion; }
            if (caste == "Caste" || caste == "null") { sCaste = null; } else { sCaste = caste; }
            if (Language == "Language" || Language == "null"|| Language == "") { sLanguage = null; } else { sLanguage = Language; }
            if (AgeFrom == "Age From") { AgeFrom = null; }
            if (AgeTo == "Age To") { AgeTo = null; }
            List<UserProfile> _masterdata = new List<UserProfile>();

            var data = _entities.USP_WE_GET_BASICSEARCH(SeekingGender,AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).ToList();

            foreach (var li in data)
            {
                if (li.ProfileId != ProfileId)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.GenderName = li.Gender;
                    up.SeekingGenderName = li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    //up.Mobile = li.;
                    up.City = li.City;
                    up.CountryName = li.Country;
                    up.State = li.StateName;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.RelationshipStatus;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.PhotoURL;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    _masterdata.Add(up);
                }
            }
            return _masterdata;
        }

        public List<UserProfile> GetSearchCompatableMatches(int SeekingGender, string AgeFrom, string AgeTo, string religion, string caste, string Language, int ProfileId)
        {
            string sReligion = string.Empty; string sCaste = string.Empty; string sLanguage = string.Empty;
            if (religion == "Religion" || religion == "null" || religion == "undefined") { sReligion = null; } else { sReligion = religion; }
            if (caste == "Caste" || caste == "null") { sCaste = null; } else { sCaste = caste; }
            if (Language == "Language" || Language == "null") { sLanguage = null; } else { sLanguage = Language; }
            if (AgeFrom == "Age From") { AgeFrom = null; }
            if (AgeTo == "Age To") { AgeTo = null; }
            List<UserProfile> _masterdata = new List<UserProfile>();

            var data = _entities.USP_WE_GET_COMPATABLE_MATCHES_IN_BASICSEARCH(SeekingGender, AgeFrom, AgeTo, sReligion, sCaste, sLanguage, ProfileId).ToList();

            foreach (var li in data)
            {
                if (li.ProfileId != ProfileId)
                {
                    UserProfile up = new UserProfile();
                    up.ProfileId = li.ProfileId;
                    up.ProfileName = li.ProfileName;
                    up.GenderName = li.Gender;
                    up.SeekingGenderName = li.SeekingGender;
                    up.Age = Convert.ToInt16(li.Age);
                    up.Caste = li.Caste;
                    up.SubCaste = li.SubCaste;
                    //up.Mobile = li.;
                    up.City = li.City;
                    up.CountryName = li.Country;
                    up.State = li.StateName;
                    up.Religion = li.Religion;
                    up.Height = li.Height;
                    up.MaritialStatus = li.RelationshipStatus;
                    up.Occupation = li.Occupation;
                    up.Introduction = li.Introduction;
                    up.ProfilePhotoUrl = li.PhotoURL;
                    up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                    _masterdata.Add(up);
                }
            }
            return _masterdata;
        }

        
        public List<UserProfile> SearchCriteria(string SeekingGender, string Religion, string Language, string Caste,string AgeFrom, string AgeTo, string HFrom, string HTo, string Country, string RelationshipStatus, string Job, string Education, string Smoke, string Drink, string Kids, string FaithInGod, string Pets, string Travel, string Movies, string Shopping,int ProfileId)
        {
            List<UserProfile> _masterdata = new List<UserProfile>();
            if (Religion == "") { Religion = null; } if (Language == "") { Language = null; } if (Caste == "0"|| Caste=="") { Caste = null; }      
            if (AgeFrom == "Age From") { AgeFrom = null; } if (AgeTo == "Age To") { AgeTo = null; } if (HFrom == "") { HFrom = null; } if (HTo == "") { HTo = null; } if (Country == "") { Country = null; } if (RelationshipStatus == "") { RelationshipStatus = null; } if (Job == "") { Job = null; } if (Education == "") { Education = null; }
            if (Smoke == "") { Smoke = null; } if (Drink == "") { Drink = null; } if (Kids == "") { Kids = null; } if (FaithInGod == "") { FaithInGod = null; } if (Pets == "") { Pets = null; } if (Travel == "") { Travel = null; } if (Movies == "") { Movies = null; } if (Shopping == "") { Shopping = null; }

            var data = _entities.USP_WE_GET_ADVANCESEARCH(SeekingGender,Religion,Language,Caste,AgeFrom, AgeTo, HFrom, HTo, Country, RelationshipStatus, Job, Education, Smoke, Drink, Kids, FaithInGod, Pets, Travel, Movies, Shopping, ProfileId).ToList();

            foreach (var li in data)
            {
                UserProfile up = new UserProfile();
                up.ProfileId = li.ProfileId;
                up.ProfileName = li.ProfileName;
                // up.Email = li.EmailId;
                up.GenderName = li.Gender;
                up.SeekingGenderName = li.SeekingGender;
                // up.DateofBirth =(DateTime)li.DateofBirth;
                up.Age = Convert.ToInt16(li.Age);
                up.Caste = li.Caste;
                up.SubCaste = li.SubCaste;
                //up.Mobile = li.;
                up.City = li.City;
                up.CountryName = li.Country;
                up.State = li.StateName;
                up.Religion = li.Religion;
                //up.Zipcode = li.Zipcode;
                //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                //up.ProfilePhotoUrl = li.ph
                up.Height = li.Height;
                up.MaritialStatus = li.RelationshipStatus;
                //up.LanguagesKnown = li.LanguagesKnown;
                up.Occupation = li.Occupation;
                up.Introduction = li.Introduction;
                up.ProfilePhotoUrl = li.PhotoURL;
                up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                _masterdata.Add(up);
            }
            return _masterdata;
        }
        public int SaveProfile(DashboardProfileModel DBPM)
        {
            //DBPM.PersonalDetails.ProfileId = 1;

            _entities.USP_WE_INSERT_PROFILE_ABOUTMYSELF(DBPM.PersonalDetails.ProfileId, DBPM.AboutMySelf.Height, DBPM.AboutMySelf.RelationshipStatus, DBPM.AboutMySelf.RelationshipDetails, DBPM.AboutMySelf.Kids, DBPM.AboutMySelf.Pets, DBPM.AboutMySelf.WantChildren, DBPM.AboutMySelf.Education, DBPM.AboutMySelf.EducationDetails, DBPM.AboutMySelf.Smoke, DBPM.AboutMySelf.Drink, DBPM.AboutMySelf.Ethnicity, DBPM.AboutMySelf.Religion, DBPM.AboutMySelf.Language, DBPM.AboutMySelf.Caste, DBPM.AboutMySelf.SubCaste, DBPM.AboutMySelf.FoodPreference, DBPM.AboutMySelf.Job, DBPM.AboutMySelf.JobDetails, DBPM.AboutMySelf.CityofJob, DBPM.AboutMySelf.StateofJob, DBPM.AboutMySelf.CountryofJob);
            _entities.USP_WE_INSERT_PROFILE_HOBBIES(DBPM.PersonalDetails.ProfileId, DBPM.Hobbies.CarsAndDriving, DBPM.Hobbies.SpendFreeTime, DBPM.Hobbies.SportsILike, DBPM.Hobbies.InstrumentsIPlay, DBPM.Hobbies.Music, DBPM.Hobbies.Vacations, DBPM.Hobbies.Favorite, DBPM.Hobbies.TVShows, DBPM.Hobbies.Movies, DBPM.Hobbies.Books, DBPM.Hobbies.Travel, DBPM.Hobbies.Excercising, DBPM.Hobbies.Hiking, DBPM.Hobbies.Driving, DBPM.Hobbies.Beaches);
            _entities.USP_WE_INSERT_PROFILE_PERSONALCHOICES(DBPM.PersonalDetails.ProfileId, DBPM.PersonalChoices.Goals, DBPM.PersonalChoices.FriendsNetwork, DBPM.PersonalChoices.FriendLaugh, DBPM.PersonalChoices.HouseImprovements, DBPM.PersonalChoices.CareAbout, DBPM.PersonalChoices.OrganizedLife, DBPM.PersonalChoices.Finances, DBPM.PersonalChoices.HomeEntertaining, DBPM.PersonalChoices.CaringforChildren, DBPM.PersonalChoices.RomanceinRelation, DBPM.PersonalChoices.Socializing, DBPM.PersonalChoices.HomeEnvironment, DBPM.PersonalChoices.SharingBeliefs, DBPM.PersonalChoices.PhysicalFit, DBPM.PersonalChoices.CalmDuringCrisis, DBPM.PersonalChoices.ThoughtsAndfeelings, DBPM.PersonalChoices.HelpingFortunates, DBPM.PersonalChoices.ResolveConflict, DBPM.PersonalChoices.Adventures, DBPM.PersonalChoices.KnowledgeAndAwareness, DBPM.PersonalChoices.ProfessionalPlanning, DBPM.PersonalChoices.EventsUnderstanding, DBPM.PersonalChoices.FindingPleasure, DBPM.PersonalChoices.CultureAndTradition, DBPM.PersonalChoices.CreativeSolutions, DBPM.PersonalChoices.MakingFriends, DBPM.PersonalChoices.CookingForFamily, DBPM.PersonalChoices.ProvideIncomeforFamily, DBPM.PersonalChoices.HouseholdSchedules, DBPM.PersonalChoices.BeingAgoodFriend);
            _entities.USP_WE_INSERT_PROFILE_PERSONALDETAILS(DBPM.PersonalDetails.ProfileId, DBPM.PersonalDetails.ProfileName, DBPM.PersonalDetails.ProfileLastName, DBPM.PersonalDetails.Email, Convert.ToString(DBPM.PersonalDetails.DateofBirth), Convert.ToString(DBPM.PersonalDetails.Age), DBPM.PersonalDetails.Mobile, DBPM.PersonalDetails.CountryCode,DBPM.PersonalDetails.City, DBPM.PersonalDetails.Country, DBPM.PersonalDetails.State, DBPM.PersonalDetails.Zipcode, DBPM.PersonalDetails.JobCity, DBPM.PersonalDetails.JobCountry, DBPM.PersonalDetails.JobState, DBPM.PersonalDetails.JobZipcode, DBPM.PersonalDetails.IsSameasAbove, DBPM.PersonalDetails.FBLink, DBPM.PersonalDetails.InstaLink, DBPM.PersonalDetails.TwitterLink, DBPM.PersonalDetails.SocialMediaLinks, DBPM.PersonalDetails.Introduction);
            _entities.USP_WE_INSERT_PROFILE_PERSONALVIEWS(DBPM.PersonalDetails.ProfileId, DBPM.PersonalViews.FaithInGod, DBPM.PersonalViews.Religion, DBPM.PersonalViews.Caste, DBPM.PersonalViews.Superstitious, DBPM.PersonalViews.Astrology, DBPM.PersonalViews.Food, DBPM.PersonalViews.OutsideEating, DBPM.PersonalViews.Movies, DBPM.PersonalViews.OnRelaxing, DBPM.PersonalViews.Travel, DBPM.PersonalViews.Shopping, DBPM.PersonalViews.Spending, DBPM.PersonalViews.BeingSelf, DBPM.PersonalViews.Humor, DBPM.PersonalViews.Anxious, DBPM.PersonalViews.Tension, DBPM.PersonalViews.SpeakingMind, DBPM.PersonalViews.SadTimes, DBPM.PersonalViews.Angry, DBPM.PersonalViews.Talkative, DBPM.PersonalViews.Fate, DBPM.PersonalViews.SelfControl, DBPM.PersonalViews.MisUnderstood, DBPM.PersonalViews.AchieveGoals, DBPM.PersonalViews.Caring, DBPM.PersonalViews.Systematic, DBPM.PersonalViews.Creativity, DBPM.PersonalViews.Forgiving, DBPM.PersonalViews.Sensitivity, DBPM.PersonalViews.AdmittingFaults, DBPM.PersonalViews.Ego, DBPM.PersonalViews.Friendship, DBPM.PersonalViews.DailyChores, DBPM.PersonalViews.Tolerance, DBPM.PersonalViews.TechSavvy, DBPM.PersonalViews.Internet, DBPM.PersonalViews.Politics, DBPM.PersonalViews.Society, DBPM.PersonalViews.TakingInitiative, DBPM.PersonalViews.FamilyValues);
            _entities.USP_WE_INSERT_PROFILE_SELFDESCRIPTION(DBPM.PersonalDetails.ProfileId, DBPM.SelfDescription.PeopleNoticeFirstThing, DBPM.SelfDescription.MostThankful, DBPM.SelfDescription.MostInfluentPerson, DBPM.SelfDescription.CannotLiveWithout, DBPM.SelfDescription.SpendLeisureTime, DBPM.SelfDescription.MostProudOf, DBPM.SelfDescription.LovablePerson, DBPM.SelfDescription.ImportantQualityofaPerson, DBPM.SelfDescription.AdditionalInfo);
            _entities.USP_WE_INSERT_PROFILE_WHOAMI(DBPM.PersonalDetails.ProfileId, DBPM.WhoAmI.ThankkfulFor, DBPM.WhoAmI.LifeSkills, DBPM.WhoAmI.ThingsCantLiveWithout, DBPM.WhoAmI.FriendsDescribedAs);
            return DBPM.PersonalDetails.ProfileId;

        }
        public DashboardProfileModel GetUserProfile(int vProfileId, int ProfileId)
        {
            DashboardProfileModel DPM = new DashboardProfileModel();
            DPM.userProfile = new UserProfile();
            DPM.PersonalDetails = new Profile_PersonalDetails();
            DPM.AboutMySelf = new Profile_AboutMySelf();
            DPM.WhoAmI = new Profile_WhoAmI();
            DPM.Hobbies = new Profile_Hobbies();
            DPM.PersonalChoices = new Profile_PersonalChoices();
            DPM.SelfDescription = new Profile_SelfDescription();
            DPM.PersonalViews = new Profile_PersonalViews();

         var data = _entities.USP_WE_GET_PROFILE_BY_ID(vProfileId, ProfileId);

            foreach (var li in data)
            {
                DPM.PersonalDetails.ProfileId = li.ProfileId;
                DPM.PersonalDetails.ProfileName = li.ProfileName;
                DPM.PersonalDetails.ProfileLastName = li.ProfileLastName;
                //DPM.PersonalDetails.DateofBirth = DateTime.Parse(li.DateofBirth);  
                if (li.DateofBirth!=null) {
                    DPM.PersonalDetails.DateofBirth = DateTime.ParseExact(li.DateofBirth, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                }            
                DPM.PersonalDetails.Email = li.EmailId;
                DPM.PersonalDetails.Country = li.Country;
                DPM.PersonalDetails.State = li.State;
                DPM.PersonalDetails.City = li.City;
                DPM.PersonalDetails.Zipcode = li.Zipcode;
                //job
                DPM.PersonalDetails.IsSameasAbove = Convert.ToBoolean(li.IsSameasAbove);
                DPM.PersonalDetails.JobCountry = li.JobCountry;
                DPM.PersonalDetails.JobState = li.JobState;
                DPM.PersonalDetails.JobCity = li.JobCity;
                DPM.PersonalDetails.JobZipcode = li.JobZipcode;
                DPM.PersonalDetails.SocialMediaLinks = li.SocialMediaLink;

                DPM.userProfile.IsFavorate = Convert.ToInt16(li.IsFavorate);
                DPM.AboutMySelf.IntroductionAboutMyself = li.Introduction;
                DPM.AboutMySelf.Height = li.Height;
                DPM.AboutMySelf.RelationshipStatus = li.RelationshipStatus;
                DPM.AboutMySelf.Kids = li.Kids;
                DPM.AboutMySelf.Pets = li.Pets;
                DPM.AboutMySelf.WantChildren = li.WantChildren;
                DPM.AboutMySelf.Education = li.Education;
                DPM.AboutMySelf.Smoke = li.Smoke;
                DPM.AboutMySelf.Drink = li.Drink;
                DPM.AboutMySelf.Ethnicity = li.Ethnicity;
                DPM.AboutMySelf.Religion = li.Religion;
                DPM.AboutMySelf.Language = li.MotherTongue;
                DPM.AboutMySelf.Caste = li.Caste;
                DPM.AboutMySelf.Job = li.Job;
                DPM.WhoAmI.ThankkfulFor = li.ThankfulFor;
                DPM.WhoAmI.LifeSkills = li.LifeSkills;
                DPM.WhoAmI.ThingsCantLiveWithout = li.ThingsCantLiveWithout;
                DPM.WhoAmI.FriendsDescribedAs = li.FriendsDescribedAs;
                DPM.Hobbies.CarsAndDriving = li.CarsAndDriving;
                DPM.Hobbies.SpendFreeTime = li.SpendFreeTime;
                DPM.Hobbies.SportsILike = li.SportsILike;
                DPM.Hobbies.InstrumentsIPlay = li.InstrumentsIPlay;
                DPM.Hobbies.Music = li.Music;
                DPM.Hobbies.Vacations = li.Vacations;
                DPM.Hobbies.Favorite = li.Favorite;
                DPM.Hobbies.TVShows = li.TVShows;
                DPM.Hobbies.Movies = li.Movies;
                DPM.Hobbies.Books = li.Books;
                DPM.Hobbies.Travel = li.Travel;
                DPM.Hobbies.Excercising = li.Excercising;
                DPM.Hobbies.Hiking = li.Hiking;
                DPM.Hobbies.Driving = li.Driving;
                DPM.Hobbies.Beaches = li.Beaches;
                ////
                
            }
            var sData = _entities.USP_WE_GET_PROFILE_SELEFDESCRIPTION_BY_ID(vProfileId).FirstOrDefault();
            if (sData != null)
            {
                DPM.PersonalChoices.Goals = sData.Goals;
                DPM.PersonalChoices.FriendsNetwork = sData.FriendsNetwork;
                DPM.PersonalChoices.FriendLaugh = sData.FriendLaugh;
                DPM.PersonalChoices.HouseImprovements = sData.HouseImprovements;
                DPM.PersonalChoices.CareAbout = sData.CareAbout;
                DPM.PersonalChoices.OrganizedLife = sData.OrganizedLife;
                DPM.PersonalChoices.Finances = sData.Finances;
                DPM.PersonalChoices.HomeEntertaining = sData.HomeEntertaining;
                DPM.PersonalChoices.CaringforChildren = sData.CaringforChildren;
                DPM.PersonalChoices.RomanceinRelation = sData.RomanceinRelation;
                DPM.PersonalChoices.Socializing = sData.Socializing;
                DPM.PersonalChoices.HomeEnvironment = sData.HomeEnvironment;
                DPM.PersonalChoices.SharingBeliefs = sData.SharingBeliefs;
                DPM.PersonalChoices.PhysicalFit = sData.PhysicalFit;
                DPM.PersonalChoices.CalmDuringCrisis = sData.CalmDuringCrisis;
                DPM.PersonalChoices.ThoughtsAndfeelings = sData.ThoughtsAndfeelings;
                DPM.PersonalChoices.HelpingFortunates = sData.HelpingFortunates;
                DPM.PersonalChoices.ResolveConflict = sData.ResolveConflict;
                DPM.PersonalChoices.Adventures = sData.Adventures;
                DPM.PersonalChoices.KnowledgeAndAwareness = sData.KnowledgeAndAwareness;
                DPM.PersonalChoices.ProfessionalPlanning = sData.ProfessionalPlanning;
                DPM.PersonalChoices.EventsUnderstanding = sData.EventsUnderstanding;
                DPM.PersonalChoices.FindingPleasure = sData.FindingPleasure;
                DPM.PersonalChoices.CultureAndTradition = sData.CultureAndTradition;
                DPM.PersonalChoices.CreativeSolutions = sData.CreativeSolutions;
                DPM.PersonalChoices.MakingFriends = sData.MakingFriends;
                DPM.PersonalChoices.CookingForFamily = sData.CookingForFamily;
                DPM.PersonalChoices.ProvideIncomeforFamily = sData.ProvideIncomeforFamily;
                DPM.PersonalChoices.HouseholdSchedules = sData.HouseholdSchedules;
                DPM.PersonalChoices.BeingAgoodFriend = sData.BeingAgoodFriend;

                DPM.SelfDescription.PeopleNoticeFirstThing = sData.PeopleNoticeFirstThing;
                DPM.SelfDescription.MostThankful = sData.MostThankful;
                DPM.SelfDescription.MostInfluentPerson = sData.MostInfluentPerson;
                DPM.SelfDescription.CannotLiveWithout = sData.CannotLiveWithout;
                DPM.SelfDescription.SpendLeisureTime = sData.SpendLeisureTime;
                DPM.SelfDescription.MostProudOf = sData.MostProudOf;
                DPM.SelfDescription.LovablePerson = sData.LovablePerson;
                DPM.SelfDescription.ImportantQualityofaPerson = sData.ImportantQualityofaPerson;
                DPM.SelfDescription.AdditionalInfo = sData.AdditionalInfo;

                DPM.PersonalViews.FaithInGod = Convert.ToInt16(sData.FaithInGod);
                DPM.PersonalViews.Religion = Convert.ToInt16(sData.sReligion);
                DPM.PersonalViews.Caste = Convert.ToInt16(sData.Caste);
                DPM.PersonalViews.Superstitious = Convert.ToInt16(sData.Superstitious);
                DPM.PersonalViews.Astrology = Convert.ToInt16(sData.Astrology);
                DPM.PersonalViews.Food = Convert.ToInt16(sData.Food);
                DPM.PersonalViews.OutsideEating = Convert.ToInt16(sData.OutsideEating);
                DPM.PersonalViews.Movies = Convert.ToInt16(sData.sMovies);
                DPM.PersonalViews.OnRelaxing = Convert.ToInt16(sData.OnRelaxing);
                DPM.PersonalViews.Travel = Convert.ToInt16(sData.Travel);
                DPM.PersonalViews.Shopping = Convert.ToInt16(sData.Shopping);
                DPM.PersonalViews.Spending = Convert.ToInt16(sData.Spending);
                DPM.PersonalViews.BeingSelf = Convert.ToInt16(sData.BeingSelf);
                DPM.PersonalViews.Humor = Convert.ToInt16(sData.Humor);
                DPM.PersonalViews.Anxious = Convert.ToInt16(sData.Anxious);
                DPM.PersonalViews.Tension = Convert.ToInt16(sData.Tension);
                DPM.PersonalViews.SpeakingMind = Convert.ToInt16(sData.SpeakingMind);
                DPM.PersonalViews.SadTimes = Convert.ToInt16(sData.SadTimes);
                DPM.PersonalViews.Angry = Convert.ToInt16(sData.Angry);
                DPM.PersonalViews.Talkative = Convert.ToInt16(sData.Talkative);
                DPM.PersonalViews.Fate = Convert.ToInt16(sData.Fate);
                DPM.PersonalViews.SelfControl = Convert.ToInt16(sData.SelfControl);
                DPM.PersonalViews.MisUnderstood = Convert.ToInt16(sData.MisUnderstood);
                DPM.PersonalViews.AchieveGoals = Convert.ToInt16(sData.AchieveGoals);
                DPM.PersonalViews.Caring = Convert.ToInt16(sData.Caring);
                DPM.PersonalViews.Systematic = Convert.ToInt16(sData.Systematic);
                DPM.PersonalViews.Creativity = Convert.ToInt16(sData.Creativity);
                DPM.PersonalViews.Forgiving = Convert.ToInt16(sData.Forgiving);
                DPM.PersonalViews.Sensitivity = Convert.ToInt16(sData.Sensitivity);
                DPM.PersonalViews.AdmittingFaults = Convert.ToInt16(sData.AdmittingFaults);
                DPM.PersonalViews.Ego = Convert.ToInt16(sData.Ego);
                DPM.PersonalViews.Friendship = Convert.ToInt16(sData.Friendship);
                DPM.PersonalViews.DailyChores = Convert.ToInt16(sData.DailyChores);
                DPM.PersonalViews.Tolerance = Convert.ToInt16(sData.Tolerance);
                DPM.PersonalViews.TechSavvy = Convert.ToInt16(sData.TechSavvy);
                DPM.PersonalViews.Internet = Convert.ToInt16(sData.Internet);
                DPM.PersonalViews.Politics = Convert.ToInt16(sData.Politics);
                DPM.PersonalViews.Society = Convert.ToInt16(sData.Society);
                DPM.PersonalViews.TakingInitiative = Convert.ToInt16(sData.TakingInitiative);
                DPM.PersonalViews.FamilyValues = Convert.ToInt16(sData.FamilyValues);
            }
            //
            return DPM;
        }

        public DashboardProfileModel GetMyProfile(int ProfileId)
        {
            DashboardProfileModel DPM = new DashboardProfileModel();

            DPM.PersonalDetails = new Profile_PersonalDetails();
            DPM.AboutMySelf = new Profile_AboutMySelf();
            DPM.WhoAmI = new Profile_WhoAmI();
            DPM.Hobbies = new Profile_Hobbies();
            DPM.userProfile = new UserProfile();
            DPM.PersonalDetails = new Profile_PersonalDetails();
            DPM.PersonalViews = new Profile_PersonalViews();
            DPM.SelfDescription = new Profile_SelfDescription();
            DPM.PersonalChoices = new Profile_PersonalChoices();

            var dUserProfile = _entities.USP_WE_GET_USER_PROFILE_BY_ID(ProfileId).ToList();
            if (dUserProfile.Count > 0)
            {
                DPM.userProfile.ProfileId = dUserProfile[0].ProfileId;
                DPM.userProfile.ProfileName = dUserProfile[0].ProfileName;
                DPM.userProfile.ProfileLastName = dUserProfile[0].ProfileLastName;
                DPM.userProfile.Email = dUserProfile[0].EmailId;
                DPM.userProfile.DateofBirth = dUserProfile[0].DateofBirth;
                DPM.userProfile.Gender = Convert.ToInt16(dUserProfile[0].Gender);
                DPM.userProfile.SeekingGender = Convert.ToInt16(dUserProfile[0].SeekingGender);
                DPM.userProfile.Zipcode = dUserProfile[0].Zipcode;
                DPM.userProfile.Country = Convert.ToInt16(dUserProfile[0].Country);
                DPM.userProfile.HowDidYouHearAboutUs = dUserProfile[0].HowDidYouHearAboutUs;
               // DPM.userProfile.IsFavorate=dUserProfile[0].is
            }
            var dAboutmyself = _entities.USP_WE_GET_PROFILE_ABOUTMYSELF_BY_ID(ProfileId).ToList();
            if (dAboutmyself.Count > 0)
            {
                DPM.AboutMySelf.Height = dAboutmyself[0].Height;
                DPM.AboutMySelf.RelationshipStatus = dAboutmyself[0].RelationshipStatus;
                DPM.AboutMySelf.RelationshipDetails= dAboutmyself[0].RelationshipDetails;
                DPM.AboutMySelf.Kids = dAboutmyself[0].Kids;
                DPM.AboutMySelf.Pets = dAboutmyself[0].Pets;
                DPM.AboutMySelf.WantChildren = dAboutmyself[0].WantChildren;
                DPM.AboutMySelf.Education = dAboutmyself[0].Education;
                DPM.AboutMySelf.EducationDetails = dAboutmyself[0].EducationDetails;
                DPM.AboutMySelf.Smoke = dAboutmyself[0].Smoke;
                DPM.AboutMySelf.Drink = dAboutmyself[0].Drink;
                DPM.AboutMySelf.Ethnicity = dAboutmyself[0].Ethnicity;
                DPM.AboutMySelf.Religion = dAboutmyself[0].Religion;
                DPM.AboutMySelf.Language = dAboutmyself[0].MotherTongue;//As per Req i have Mapped MotherToungue col to Language
                DPM.AboutMySelf.Caste = dAboutmyself[0].Caste;
                DPM.AboutMySelf.SubCaste = dAboutmyself[0].SubCaste;
                DPM.AboutMySelf.Job = dAboutmyself[0].Job;
                DPM.AboutMySelf.JobDetails = dAboutmyself[0].JobDetails;
                DPM.AboutMySelf.CityofJob = dAboutmyself[0].CityofJob;
                DPM.AboutMySelf.StateofJob = dAboutmyself[0].StateofJob;
                DPM.AboutMySelf.CountryofJob = dAboutmyself[0].CountryofJob;
                DPM.AboutMySelf.IntroductionAboutMyself = dAboutmyself[0].IntroductionAboutMyself;

                var dPersonalDetails = _entities.USP_WE_GET_PROFILE_PERSONALDETAILS_BY_ID(ProfileId).ToList();
                DPM.PersonalDetails.ProfileName = dPersonalDetails[0].ProfileName;
                DPM.PersonalDetails.ProfileLastName = dUserProfile[0].ProfileLastName;
                DPM.PersonalDetails.Email = dPersonalDetails[0].Email;
                //DPM.PersonalDetails.DateofBirth = Convert.ToDateTime(dPersonalDetails[0].DateofBirth.Remove(dPersonalDetails[0].DateofBirth.Length - 2));
                DPM.PersonalDetails.Age = Convert.ToInt16(dPersonalDetails[0].Age);
                DPM.PersonalDetails.Mobile = dPersonalDetails[0].Mobile;

                DPM.PersonalDetails.City = dPersonalDetails[0].City;
                DPM.PersonalDetails.Country = dPersonalDetails[0].Country;
                DPM.PersonalDetails.State = dPersonalDetails[0].State;
                DPM.PersonalDetails.Zipcode = dPersonalDetails[0].Zipcode;
                //
                DPM.PersonalDetails.IsSameasAbove = Convert.ToBoolean(dPersonalDetails[0].IsSameasAbove);
                DPM.PersonalDetails.JobCity = dPersonalDetails[0].JobCity;
                DPM.PersonalDetails.JobCountry = dPersonalDetails[0].JobCountry;
                DPM.PersonalDetails.JobState = dPersonalDetails[0].JobState;
                DPM.PersonalDetails.JobZipcode = dPersonalDetails[0].JobZipcode;
                //
                DPM.PersonalDetails.FBLink = dPersonalDetails[0].FBLink;
                DPM.PersonalDetails.InstaLink = dPersonalDetails[0].InstaLink;
                DPM.PersonalDetails.TwitterLink = dPersonalDetails[0].TwitterLink;
                DPM.PersonalDetails.SocialMediaLinks = dPersonalDetails[0].SocialMediaLink;
                DPM.PersonalDetails.Introduction = dPersonalDetails[0].Introduction;

                var dPersonalViews = _entities.USP_WE_GET_PROFILE_PERSONALVIEWS_BY_ID(ProfileId).ToList();
                if (dPersonalViews.Count > 0)
                {
                    DPM.PersonalViews.FaithInGod = Convert.ToInt16(dPersonalViews[0].FaithInGod);
                    DPM.PersonalViews.Religion = Convert.ToInt16(dPersonalViews[0].Religion);
                    DPM.PersonalViews.Caste = Convert.ToInt16(dPersonalViews[0].Caste);
                    DPM.PersonalViews.Superstitious = Convert.ToInt16(dPersonalViews[0].Superstitious);
                    DPM.PersonalViews.Astrology = Convert.ToInt16(dPersonalViews[0].Astrology);
                    DPM.PersonalViews.Food = Convert.ToInt16(dPersonalViews[0].Food);
                    DPM.PersonalViews.OutsideEating = Convert.ToInt16(dPersonalViews[0].OutsideEating);
                    DPM.PersonalViews.Movies = Convert.ToInt16(dPersonalViews[0].Movies);
                    DPM.PersonalViews.OnRelaxing = Convert.ToInt16(dPersonalViews[0].OnRelaxing);
                    DPM.PersonalViews.Travel = Convert.ToInt16(dPersonalViews[0].Travel);
                    DPM.PersonalViews.Shopping = Convert.ToInt16(dPersonalViews[0].Shopping);
                    DPM.PersonalViews.Spending = Convert.ToInt16(dPersonalViews[0].Spending);
                    DPM.PersonalViews.BeingSelf = Convert.ToInt16(dPersonalViews[0].BeingSelf);
                    DPM.PersonalViews.Humor = Convert.ToInt16(dPersonalViews[0].Humor);
                    DPM.PersonalViews.Anxious = Convert.ToInt16(dPersonalViews[0].Anxious);
                    DPM.PersonalViews.Tension = Convert.ToInt16(dPersonalViews[0].Tension);
                    DPM.PersonalViews.SpeakingMind = Convert.ToInt16(dPersonalViews[0].SpeakingMind);
                    DPM.PersonalViews.SadTimes = Convert.ToInt16(dPersonalViews[0].SadTimes);
                    DPM.PersonalViews.Angry = Convert.ToInt16(dPersonalViews[0].Angry);
                    DPM.PersonalViews.Talkative = Convert.ToInt16(dPersonalViews[0].Talkative);
                    DPM.PersonalViews.Fate = Convert.ToInt16(dPersonalViews[0].Fate);
                    DPM.PersonalViews.SelfControl = Convert.ToInt16(dPersonalViews[0].SelfControl);
                    DPM.PersonalViews.MisUnderstood = Convert.ToInt16(dPersonalViews[0].MisUnderstood);
                    DPM.PersonalViews.AchieveGoals = Convert.ToInt16(dPersonalViews[0].AchieveGoals);
                    DPM.PersonalViews.Caring = Convert.ToInt16(dPersonalViews[0].Caring);
                    DPM.PersonalViews.Systematic = Convert.ToInt16(dPersonalViews[0].Systematic);
                    DPM.PersonalViews.Creativity = Convert.ToInt16(dPersonalViews[0].Creativity);
                    DPM.PersonalViews.Forgiving = Convert.ToInt16(dPersonalViews[0].Forgiving);
                    DPM.PersonalViews.Sensitivity = Convert.ToInt16(dPersonalViews[0].Sensitivity);
                    DPM.PersonalViews.AdmittingFaults = Convert.ToInt16(dPersonalViews[0].AdmittingFaults);
                    DPM.PersonalViews.Ego = Convert.ToInt16(dPersonalViews[0].Ego);
                    DPM.PersonalViews.Friendship = Convert.ToInt16(dPersonalViews[0].Friendship);
                    DPM.PersonalViews.DailyChores = Convert.ToInt16(dPersonalViews[0].DailyChores);
                    DPM.PersonalViews.Tolerance = Convert.ToInt16(dPersonalViews[0].Tolerance);
                    DPM.PersonalViews.TechSavvy = Convert.ToInt16(dPersonalViews[0].TechSavvy);
                    DPM.PersonalViews.Internet = Convert.ToInt16(dPersonalViews[0].Internet);
                    DPM.PersonalViews.Politics = Convert.ToInt16(dPersonalViews[0].Politics);
                    DPM.PersonalViews.Society = Convert.ToInt16(dPersonalViews[0].Society);
                    DPM.PersonalViews.TakingInitiative = Convert.ToInt16(dPersonalViews[0].TakingInitiative);
                    DPM.PersonalViews.FamilyValues = Convert.ToInt16(dPersonalViews[0].FamilyValues);
                }
                var sPersonalChoices = _entities.USP_WE_GET_PROFILE_PERSONALCHOICES_BY_ID(ProfileId).ToList();
                if (sPersonalChoices.Count > 0)
                {
                    DPM.PersonalChoices.Goals = Convert.ToString(sPersonalChoices[0].Goals);
                    DPM.PersonalChoices.FriendsNetwork = sPersonalChoices[0].FriendsNetwork;
                    DPM.PersonalChoices.FriendLaugh = sPersonalChoices[0].FriendLaugh;
                    DPM.PersonalChoices.HouseImprovements = sPersonalChoices[0].HouseImprovements;
                    DPM.PersonalChoices.CareAbout = sPersonalChoices[0].CareAbout;
                    DPM.PersonalChoices.OrganizedLife = sPersonalChoices[0].OrganizedLife;
                    DPM.PersonalChoices.Finances = sPersonalChoices[0].Finances;
                    DPM.PersonalChoices.HomeEntertaining = sPersonalChoices[0].HomeEntertaining;
                    DPM.PersonalChoices.CaringforChildren = sPersonalChoices[0].CaringforChildren;
                    DPM.PersonalChoices.RomanceinRelation = sPersonalChoices[0].RomanceinRelation;
                    DPM.PersonalChoices.Socializing = sPersonalChoices[0].Socializing;
                    DPM.PersonalChoices.HomeEnvironment = sPersonalChoices[0].HomeEnvironment;
                    DPM.PersonalChoices.SharingBeliefs = sPersonalChoices[0].SharingBeliefs;
                    DPM.PersonalChoices.PhysicalFit = sPersonalChoices[0].PhysicalFit;
                    DPM.PersonalChoices.CalmDuringCrisis = sPersonalChoices[0].CalmDuringCrisis;
                    DPM.PersonalChoices.ThoughtsAndfeelings = sPersonalChoices[0].ThoughtsAndfeelings;
                    DPM.PersonalChoices.HelpingFortunates = sPersonalChoices[0].HelpingFortunates;
                    DPM.PersonalChoices.ResolveConflict = sPersonalChoices[0].ResolveConflict;
                    DPM.PersonalChoices.KnowledgeAndAwareness = sPersonalChoices[0].KnowledgeAndAwareness;
                    DPM.PersonalChoices.ProfessionalPlanning = sPersonalChoices[0].ProfessionalPlanning;
                    DPM.PersonalChoices.EventsUnderstanding = sPersonalChoices[0].EventsUnderstanding;
                    DPM.PersonalChoices.FindingPleasure = sPersonalChoices[0].FindingPleasure;
                    DPM.PersonalChoices.CultureAndTradition = sPersonalChoices[0].CultureAndTradition;
                    DPM.PersonalChoices.CreativeSolutions = sPersonalChoices[0].CreativeSolutions;
                    DPM.PersonalChoices.MakingFriends = sPersonalChoices[0].MakingFriends;
                    DPM.PersonalChoices.CookingForFamily = sPersonalChoices[0].CookingForFamily;
                    DPM.PersonalChoices.ProvideIncomeforFamily = sPersonalChoices[0].ProvideIncomeforFamily;
                    DPM.PersonalChoices.HouseholdSchedules = sPersonalChoices[0].HouseholdSchedules;
                    DPM.PersonalChoices.BeingAgoodFriend = sPersonalChoices[0].BeingAgoodFriend;
                }

                var dProfileHobbies = _entities.USP_WE_GET_PROFILE_HOBBIES_BY_ID(ProfileId).ToList();
                if (dProfileHobbies.Count > 0)
                {
                    DPM.Hobbies.CarsAndDriving = dProfileHobbies[0].CarsAndDriving;
                    DPM.Hobbies.SpendFreeTime = dProfileHobbies[0].SpendFreeTime;
                    DPM.Hobbies.SportsILike = dProfileHobbies[0].SportsILike;
                    DPM.Hobbies.InstrumentsIPlay = dProfileHobbies[0].InstrumentsIPlay;
                    DPM.Hobbies.Music = dProfileHobbies[0].Music;
                    DPM.Hobbies.Vacations = dProfileHobbies[0].Vacations;
                    DPM.Hobbies.Favorite = dProfileHobbies[0].Favorite;
                    DPM.Hobbies.TVShows = dProfileHobbies[0].TVShows;
                    DPM.Hobbies.Movies = dProfileHobbies[0].Movies;
                    DPM.Hobbies.Books = dProfileHobbies[0].Books;
                    DPM.Hobbies.Travel = dProfileHobbies[0].Travel;
                    DPM.Hobbies.Excercising = dProfileHobbies[0].Excercising;
                    DPM.Hobbies.Hiking = dProfileHobbies[0].Hiking;
                    DPM.Hobbies.Driving = dProfileHobbies[0].Driving;
                }
                var dSelfdescription = _entities.USP_WE_GET_PROFILE_SELFDESCRIPTION_BY_ID(ProfileId).ToList();
                if (dSelfdescription.Count > 0)
                {
                    DPM.SelfDescription.PeopleNoticeFirstThing = dSelfdescription[0].PeopleNoticeFirstThing;
                    DPM.SelfDescription.MostThankful = dSelfdescription[0].MostThankful;
                    DPM.SelfDescription.MostInfluentPerson = dSelfdescription[0].MostInfluentPerson;
                    DPM.SelfDescription.CannotLiveWithout = dSelfdescription[0].CannotLiveWithout;
                    DPM.SelfDescription.SpendLeisureTime = dSelfdescription[0].SpendLeisureTime;
                    DPM.SelfDescription.MostProudOf = dSelfdescription[0].MostProudOf;
                    DPM.SelfDescription.LovablePerson = dSelfdescription[0].LovablePerson;
                    DPM.SelfDescription.ImportantQualityofaPerson = dSelfdescription[0].ImportantQualityofaPerson;
                    DPM.SelfDescription.AdditionalInfo = dSelfdescription[0].AdditionalInfo;
                }
                var dWhoamI = _entities.USP_WE_GET_PROFILE_WHOAMI_BY_ID(ProfileId).ToList();
                if (dWhoamI.Count > 0)
                {
                    DPM.WhoAmI.ThankkfulFor = dWhoamI[0].ThankfulFor;
                    DPM.WhoAmI.LifeSkills = dWhoamI[0].LifeSkills;
                    DPM.WhoAmI.ThingsCantLiveWithout = dWhoamI[0].ThingsCantLiveWithout;
                    DPM.WhoAmI.FriendsDescribedAs = dWhoamI[0].FriendsDescribedAs;
                }
            }
            return DPM;
        }
        public string InsertProfilePhotos(int ProfileId, string PhotoNameExt)
        {
            var optPhotoname = new ObjectParameter("Optimizedphotourl", typeof(string));
            _entities.usp_WE_Insert_Profile_Photo(ProfileId, PhotoNameExt, optPhotoname).ToList();
            _entities.SaveChanges();
            return Convert.ToString(optPhotoname.Value);
        }
        public int SetDefaultPhoto(int ProfileId, int PhotoId)
        {
           var rProfileId =_entities.USP_WE_SET_USER_DEFAULT_PHOTO(ProfileId, PhotoId).FirstOrDefault();
           return Convert.ToInt16(rProfileId);
        }
        public string InsertCoverPhoto(int ProfileId, string PhotoNameExt)
        {
            var optPhotoname = new ObjectParameter("Optimizedphotourl", typeof(string));
            _entities.USP_WE_INSERT_COVER_PHOTO(ProfileId, PhotoNameExt, optPhotoname).ToList();
            _entities.SaveChanges();
            return Convert.ToString(optPhotoname.Value);
        }

        public int InsertProfilePic(int ProfileId, string PhotoUrl)
        {
            var optPhotoname = new ObjectParameter("sResult", typeof(string));
            _entities.USP_WE_INSERT_PROFILE_OPTIMIZED_PIC(ProfileId, PhotoUrl, optPhotoname).ToList();
            _entities.SaveChanges();
            return Convert.ToInt16(optPhotoname.Value);
        }
        public List<ProfilePhoto> GetProfilePhotos(int ProfileId)
        {
            List<ProfilePhoto> PP = new List<ProfilePhoto>();
            var dList = _entities.USP_WE_GET_PROFILE_PHOTOS_BY_ID(ProfileId).ToList();

            foreach (var li in dList)
            {
                ProfilePhoto pl = new ProfilePhoto();
                pl.PhotoId = li.PhotoId;
                pl.ProfileId = (int)li.ProfileId;
                pl.PhotoUrl = li.PhotoURL;
                pl.ThumbnailUrl = li.ThumbnailUrl;
                PP.Add(pl);
            }
            return PP;
        }
        public Coverphoto GetCoverPhoto(int ProfileId)
        {
            var dList = _entities.USP_WE_GET_COVER_PHOTO_BY_ID(ProfileId).ToList();
            Coverphoto cp = new Coverphoto();
            if (dList.Count > 0)
            {
                cp.CoverPhotoId = dList[0].CoverPhotoId;
                cp.ProfileId = (int)dList[0].ProfileId;
                cp.CoverPhotoURL = dList[0].CoverPhotoURL;
            }
            return cp;
        }
        public UserProfilePic GetProfilePic(int ProfileId)
        {
            var dList = _entities.USP_WE_GET_PROFILE_PIC_BY_ID(ProfileId).ToList();
            UserProfilePic cp = new UserProfilePic();
            if (dList.Count > 0)
            {
                cp.UserProfilePicId = dList[0].ProfilePicId;
                cp.UserProfilePicUrl = dList[0].ProfilePicURL;
            }
            return cp;
        }

        public List<UserProfile> GetWhoViewedMe(int ProfileId)
        {
            List<UserProfile> _masterdata = new List<UserProfile>();
            var data = _entities.USP_WE_DASHBOARD_GET_WHO_VIEWED_ME(ProfileId,0).ToList();
            foreach (var li in data)
            {
                UserProfile up = new UserProfile();
                up.ProfileId =(int)li.ProfileId;
                up.ProfileName = li.ProfileName;
                // up.Email = li.EmailId;
                up.GenderName = li.Gender;
                up.SeekingGenderName = li.SeekingGender;
                // up.DateofBirth =(DateTime)li.DateofBirth;
                up.Age = Convert.ToInt16(li.Age);
                up.Caste = li.Caste;
                up.SubCaste = li.SubCaste;
                //up.Mobile = li.;
                up.City = li.City;
                up.CountryName = li.Country;
                up.State = li.StateName;
                up.Religion = li.Religion;
                //up.Zipcode = li.Zipcode;
                //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                //up.ProfilePhotoUrl = li.ph
                up.Height = li.Height;
                up.MaritialStatus = li.RelationshipStatus;
                //up.LanguagesKnown = li.LanguagesKnown;
                up.Occupation = li.Occupation;
                up.Introduction = li.Introduction;
                up.ProfilePhotoUrl = li.DefaultPhotoUrl;
                up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                _masterdata.Add(up);
            }
            return _masterdata;

        }

        public List<UserProfile> GetUserfavourites(int ProfileId)
        {
            List<UserProfile> _masterdata = new List<UserProfile>();
            var data = _entities.USP_WE_GET_USER_FAVOURITES(ProfileId).ToList();
            foreach (var li in data)
            {
                UserProfile up = new UserProfile();
                up.ProfileId = li.ProfileId;
                up.ProfileName = li.ProfileName;
                // up.Email = li.EmailId;
                up.GenderName = li.Gender;
                up.SeekingGenderName = li.SeekingGender;
                // up.DateofBirth =(DateTime)li.DateofBirth;
                up.Age = Convert.ToInt16(li.Age);
                up.Caste = li.Caste;
                up.SubCaste = li.SubCaste;
                //up.Mobile = li.;
                up.City = li.City;
                up.CountryName = li.Country;
                up.State = li.StateName;
                up.Religion = li.Religion;
                //up.Zipcode = li.Zipcode;
                //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                //up.ProfilePhotoUrl = li.ph
                up.Height = li.Height;
                up.MaritialStatus = li.RelationshipStatus;
                //up.LanguagesKnown = li.LanguagesKnown;
                up.Occupation = li.Occupation;
                up.Introduction = li.Introduction;
                up.ProfilePhotoUrl = li.PhotoURL;
                up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                _masterdata.Add(up);
            }
            return _masterdata;

        }
        public List<UserProfile> GetUserCompatableMatches(int ProfileId)
        {
            List<UserProfile> _masterdata = new List<UserProfile>();
            var data = _entities.USP_WE_GET_USER_COMPATABLE_MATCHES(ProfileId).ToList();
            foreach (var li in data)
            {
                UserProfile up = new UserProfile();
                up.ProfileId = li.ProfileId;
                up.ProfileName = li.ProfileName;
                // up.Email = li.EmailId;
                up.GenderName = li.Gender;
                up.SeekingGenderName = li.SeekingGender;
                // up.DateofBirth =(DateTime)li.DateofBirth;
                up.Age = Convert.ToInt16(li.Age);
                up.Caste = li.Caste;
                up.SubCaste = li.SubCaste;
                //up.Mobile = li.;
                up.City = li.City;
                up.CountryName = li.Country;
                up.State = li.StateName;
                up.Religion = li.Religion;
                //up.Zipcode = li.Zipcode;
                //up.HowDidYouHearAboutUs = li.HowDidYouHearAboutUs;
                //up.ProfilePhotoUrl = li.ph
                up.Height = li.Height;
                up.MaritialStatus = li.RelationshipStatus;
                //up.LanguagesKnown = li.LanguagesKnown;
                up.Occupation = li.Occupation;
                up.Introduction = li.Introduction;
                up.ProfilePhotoUrl = li.PhotoURL;
                up.IsFavorate = Convert.ToInt16(li.IsFavorate);
                _masterdata.Add(up);
            }
            return _masterdata;
        }
        public bool SetFavourite(int pId, int fId)
        {
            bool result;
            try
            {
                _entities.USP_WE_SET_USER_FAVOURITES(pId, fId);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
        public bool RemFavourite(int pId, int fId)
        {
            bool result;
            try
            {
                _entities.USP_WE_REM_USER_FAVOURITES(pId, fId);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }

        public string ViewMobileNumber(int vProfileId, int ProfileId)
        {
            var sResult = new ObjectParameter("sResult", typeof(string));
            _entities.USP_WE_GET_PROFILE_MOBILE_NUMBER_BY_ID(vProfileId, ProfileId, sResult).ToList();
            _entities.SaveChanges();
            return Convert.ToString(sResult.Value);
        }
        public UserProfile GetProfiledetails(int ProfileId)
        {
            var data = _entities.USP_WE_GET_PROFILE_BY_ID_FOR_UPDATE(ProfileId).ToList();
            UserProfile up = new UserProfile();
            up.ProfileId = ProfileId;
            up.ProfileName = data[0].ProfileName;
            up.Email = data[0].Email;
            up.Mobile = data[0].Mobile; 
            return up;
        }
        public UserPackageDetails GetUserPackageDetails(int ProfileId)
        {
            var data = _entities.USP_WE_GET_USER_PACKAGE_DETAILS(ProfileId).ToList();
            UserPackageDetails upd = new UserPackageDetails();
            if (data.Count > 0) {
            upd.ProfileId = Convert.ToInt16(data[0].ProfileID);
            upd.PackageId = Convert.ToInt16(data[0].PackageId);
            upd.PackageName = data[0].PackageName;
            upd.TrasactionId = data[0].Transaction_Id;
            upd.ByCountry = Convert.ToInt16(data[0].ByCountry);
            upd.PackageDuration = Convert.ToInt16(data[0].PackageDuration);
            upd.PackageStartDate = Convert.ToDateTime(data[0].PackageStartDate);
            upd.PackageEndDate = Convert.ToDateTime(data[0].PackageEndDate);
            upd.PackagePrice = string.Format("{0:0.00}", data[0].PackagePrice); 
            upd.PayMode = data[0].Payment_From;
            upd.Status = data[0].Status;
            }
            return upd;
        }

        public string InsertUserPackage(int ProfileId, int PackageId, string Transaction_Id, string Payment_From)
        {
            
            var sResult =_entities.USP_WE_INSERT_USER_PACKAGE(ProfileId,PackageId,Transaction_Id,Payment_From).FirstOrDefault();           
            return Convert.ToString(sResult);
        }


        public List<CompatibleMatches> GetCompatibleMatches(int ProfileId)
        {
            List<CompatibleMatches> LCM = new List<CompatibleMatches>();
            
             var data = _entities.USP_WE_GET_COMPATIBLE_MATCHES(ProfileId).ToList();
            foreach (var li in data)
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
                LCM.Add(cm);
            }
            return LCM;

        }
        public List<RecentVisitors> GetRecentVisitors(int ProfileId)
        {
            List<RecentVisitors> rvs = new List<RecentVisitors>();
            var data = _entities.USP_WE_GET_RECENT_VISITORS(ProfileId).ToList();
            foreach (var lis in data)
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
                rvs.Add(rv);
            }
            return rvs;
        }
        public List<RecentVisitors> GettotRecentVisitors(int ProfileId)
        {
            List<RecentVisitors> rvs = new List<RecentVisitors>();
            var data = _entities.USP_WE_GET_TOTAL__RECENT_VISITORS(ProfileId).ToList();
            foreach (var lis in data)
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
                rvs.Add(rv);
            }
            return rvs;
        }

       
        public List<WhoViewedMe> GetDashBoardWhoViewedMe(int ProfileId,int rows)
        {
            List<WhoViewedMe> wvm = new List<WhoViewedMe>();
            var data = _entities.USP_WE_DASHBOARD_GET_WHO_VIEWED_ME(ProfileId, rows).ToList();
            foreach (var lis in data)
            {
                WhoViewedMe wv = new WhoViewedMe();
                wv.ProfileId = Convert.ToInt16(lis.ProfileId);
                wv.ViewedProfileId = Convert.ToInt16(lis.ViewedProfileId);
                wv.ProfileName = lis.ProfileName;
                wv.ProfileLastName = lis.ProfileLastName;
                wv.Age = lis.Age;
                wv.Height = lis.Height;
                wv.City = lis.City;
                wv.StateName = lis.StateName;
                wv.Country = lis.Country;
                wv.MotherTongue = lis.MotherTongue;
                wv.DefaultPhotoUrl = lis.DefaultPhotoUrl;
                wv.Gender = lis.Gender;
                wvm.Add(wv);
            }
            return wvm;

        }

        public List<SelectListItem> States(int CountryId)
        {
            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_STATE_BY_COUNTRY(CountryId).ToList();

            foreach (var li in data)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.StateId),
                    Text = li.StateName
                });

            }
            return _masterdata;
        }
        public List<SelectListItem> Cities(int StateId)
        {
            List<SelectListItem> _masterdata = new List<SelectListItem>();

            var data = _entities.USP_WE_GET_CITY_BY_STATE(StateId).ToList();

            foreach (var li in data)
            {
                _masterdata.Add(new SelectListItem
                {
                    Value = Convert.ToString(li.CityId),
                    Text = li.CityName
                });

            }
            return _masterdata;
        }
        public int SaveNewPassword(int user, string pwdEncryptred)
        {
            var sResult = new ObjectParameter("Result", typeof(int));
            _entities.WEvaha_Update_NewPassword(sResult, user, pwdEncryptred);
            _entities.SaveChanges();
            return Convert.ToInt16(sResult.Value);
        }

    }
}
