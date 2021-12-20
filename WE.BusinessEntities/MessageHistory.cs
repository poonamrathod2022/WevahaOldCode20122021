using System;

namespace WE.BusinessEntities
{
    public class MessageHistory
    {
        public long MessageId { get; set; }
        public int? TotalRowCount { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromUserName { get; set; }
        public string MessageDetail { get; set; }
        public string MessageDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserProfilePic UserProfilePic { get; set; }
        public bool IsReported { get; set; }
        public bool IsBlock { get; set; }
        public bool IsStar { get; set; }
        public string FileName { get; set; }
        public string FileUniqueId { get; set; }
    }

    public class MessageListModel
    {
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 1000;
    }
}