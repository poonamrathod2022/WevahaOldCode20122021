using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WE.BusinessEntities;

namespace WEvaha.Models
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
        public List<UserProfile> UserProfilelistdetails { get; set; }
        public List<UserProfile> UserProfileCompatablelistdetails { get; set; }
        public UserProfile userProfile { get; set; }
        public List<ProfilePhoto> profilePhotos { get; set; }
        public List<RecentVisitors> recentVisitors { get; set; }
        public Coverphoto coverPhoto { get; set; }
        public UserProfilePic userprofilePic { get; set; }
        public List<Packages> LPackages { get; set; }
        public List<CompatibleMatches> compatibleMatches { get; set; }
        public List<WhoViewedMe> whoViewedMe { get; set; }
        public ResetPassword resetPassword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RecordCount { get; set; }
        public int PageCount { get; set; }
        public int UserPackageStatus { get; set; }
        public List<MessageUserList> MessageCountList { get; set; }
        public int IsFullyRegistered { get; set; }
    }
}