using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEvaha.Models
{
    public class WevahaViewModel
    {
        public LoginEntity login { get; set; }
        public UserProfile userProfile { get; set; }       
        public FactFile factFile { get; set; }
        public PartnerSpecifications partnerSpecifications { get; set; }
        public Profile_PersonalDetails profile_PersonalDetails { get; set; }
        public Profile_WhoAmI profile_WhoAmI { get; set; }
        public Profile_SelfDescription profile_SelfDescription { get; set; }      
        public Profile_AboutMySelf profile_AboutMySelf { get; set; }
        public Profile_PersonalChoices profile_PersonalChoices { get; set; }
        public Profile_Hobbies profile_Hobbies { get; set; }
        public Profile_PersonalViews profile_PersonalViews { get; set; }
        
       
    }
}