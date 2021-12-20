using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WE.BusinessEntities
{
    public class Profile_AboutMySelf
    {
        public string Height { get; set; }
        public string RelationshipStatus { get; set; }
        public string Kids { get; set; }
        public string Pets { get; set; }

        public string WantChildren { get; set; }
        public string Education { get; set; }
        public string Smoke { get; set; }
        public string Drink { get; set; }
        public string Ethnicity { get; set; }
        public string Religion { get; set; }
        public string Language { get; set; }//MotherToungue is replaced by language
        public string Caste { get; set; }
        public string SubCaste { get; set; }
        public string Job { get; set; }
       
        public string CityofJob { get; set; }
        public string StateofJob { get; set; }
        public string CountryofJob { get; set; }
        public string FoodPreference { get; set; }
        public string IntroductionAboutMyself { get; set; }

        public string JobDetails { get; set; }
        public string EducationDetails { get; set; }
        public string RelationshipDetails { get; set; }
    }
}