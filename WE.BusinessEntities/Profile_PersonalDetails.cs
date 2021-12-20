using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WE.BusinessEntities
{
    public class Profile_PersonalDetails
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileLastName { get; set; }
        public string Email { get; set; }
        public DateTime DateofBirth { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public bool IsSameasAbove { get; set; }
        public string JobCity { get; set; }
        public string JobCountry { get; set; }
        public string JobState { get; set; }
        public string JobZipcode { get; set; }
        public string FBLink { get; set; }
        public string InstaLink { get; set; }
        public string TwitterLink { get; set; }
        public string SocialMediaLinks { get; set; }
        public string Introduction { get; set; }
       

    }
}