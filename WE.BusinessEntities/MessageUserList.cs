using System;
using System.Collections.Generic;

namespace WE.BusinessEntities
{
    public class MessageUserList
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public int? UnreadMsg { get; set; }
        public UserProfilePic UserProfilePic { get; set; }
        public DateTime LastMessageDate { get; set; }
        public string LastMessage { get; set; }
        public UserProfilePic CurrentUserProfileURL { get; set; }
        public string CurrentUserProfileName { get; set; }
        public string LastSeenStringDate { get; set; }
        public bool IsStar { get; set; }
        public bool IsArchive { get; set; }
        public bool IsSpam { get; set; }
        public bool IsReported { get; set; }
        public List<LabelUserList> CurrentUserLabel { get; set; }
        public List<LabelUserList> ToUserLabel { get; set; }
    }

    public class LabelUserList
    {
        public long LabelId { get; set; }
        public string LabelName { get; set; }
        public bool CanDelete { get; set; }
    }

    public class ReportUserModel
    {
        public string ProfileId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string ToUserProfileId { get; set; }
        public string ReportMessage { get; set; }
    }
}
