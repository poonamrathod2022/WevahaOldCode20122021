using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEvaha.Models
{
    public class UserProfile
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileLastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Gender { get; set; }
        public string GenderName { get; set; }
        public int SeekingGender { get; set; }
        public string SeekingGenderName { get; set; }
        public string DateofBirth { get; set; }
        public int Age { get; set; }
        public string Caste { get; set; }
        public string SubCaste { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public int Country { get; set; }
        public string CountryName { get; set; }
        public string State { get; set; }
        public string StateName { get; set; }
        public string Zipcode { get; set; }
        public string HowDidYouHearAboutUs { get; set; }
        public string ActivationCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public int Status { get; set; }
        public string Religion { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string Height { get; set; }
        public string MaritialStatus { get; set; }
        public string LanguagesKnown { get; set; }
        public string Occupation { get; set; }
        public string Introduction { get; set; }
        public string ProfilePhoto { get; set; }
        public List<SelectListItem> GenderOpti { get; set; }
        public int IsFavorate { get; set; }

    }
}