using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEvaha.Models
{
    public class Profile_AboutMySelf
    {
        public int Height { get; set; }
        public string dHeight { get; set; }
        public int RelationshipStatus { get; set; }
        
        public string dRelationshipStatus { get; set; }
        public int Kids { get; set; }
        public string dKids { get; set; }
        public int Pets { get; set; }
        public string dPets { get; set; }

        public int WantChildren { get; set; }
        public string dWantChildren { get; set; }
        public int Education { get; set; }       
        public string dEducation { get; set; }
        public int Smoke { get; set; }
        public string dSmoke { get; set; }
        public int Drink { get; set; }
        public string dDrink { get; set; }
        public int Ethnicity { get; set; }
        public string dEthnicity { get; set; }
        public int Religion { get; set; }
        public int sReligion { get; set; }
        public string dReligion { get; set; }        
        public int Language { get; set; } // MotherTongue is replaced with Language

        public int FoodPreference { get; set; }
        public int sLanguage { get; set; }
        public int Caste { get; set; }
        public int sCaste { get; set; }
        public int SubCaste { get; set; }
        public int Job { get; set; }  
        public string sJob { get; set; }
        public int Profession { get; set; }
        public string CityofJob { get; set; }
        public string CountryofJob { get; set; }
        public string StateofJob { get; set; }
        public string IntroductionAboutMyself { get; set; }
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
        public string JobDetails { get; set; }
        public string EducationDetails { get; set; }
        public string RelationshipDetails { get; set; }
        //
        public List<SelectListItem> FoodPreferences{ get; set; }
        public List<SelectListItem> HeightOpti { get; set; }
        public List<SelectListItem> RelationshipStatusOpti { get; set; }
        public List<SelectListItem> KidsOpti { get; set; }
        public List<SelectListItem> PetsOpti { get; set; }
        public List<SelectListItem> GenderOpti { get; set; }

        public List<SelectListItem> WantChildrenOpti { get; set; }
        public List<SelectListItem> EducationOpti { get; set; }
        public List<SelectListItem> ProfessionOpti { get; set; }
        public List<SelectListItem> SmokeOpti { get; set; }
        public List<SelectListItem> DrinkOpti { get; set; }
        public List<SelectListItem> EthnicityOpti { get; set; }
        public List<SelectListItem> ReligionOpti { get; set; }
        public List<SelectListItem> MotherTongueOpti { get; set; }
        public List<SelectListItem> LanguageOpti { get; set; }
        public List<SelectListItem> CasteOpti { get; set; }
        public List<SelectListItem> sCasteOpti { get; set; }
        public List<SelectListItem> SubCasteOpti { get; set; }
        public List<SelectListItem> JobOpti { get; set; }

        public List<SelectListItem> CityofJobOpti { get; set; }
        public List<SelectListItem> StateofJobOpti { get; set; }
        public List<SelectListItem> CountryofJobOpti { get; set; }
        public List<SelectListItem> AgeFromOption { get; set; }
        public List<SelectListItem> AgeToOption { get; set; }
       
    }
}