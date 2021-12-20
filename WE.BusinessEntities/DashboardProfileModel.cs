using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WE.BusinessEntities
{
    public class DashboardProfileModel
    {
        public Profile_PersonalDetails PersonalDetails { get; set; }
        public Profile_WhoAmI WhoAmI { get; set; }
        public Profile_SelfDescription SelfDescription { get; set; }
        public Profile_AboutMySelf AboutMySelf { get; set; }
        public Profile_PersonalChoices PersonalChoices { get; set; }
        public Profile_Hobbies Hobbies { get; set; }
        public Profile_PersonalViews PersonalViews { get; set; }
        public UserProfile userProfile { get; set; }
    }
}