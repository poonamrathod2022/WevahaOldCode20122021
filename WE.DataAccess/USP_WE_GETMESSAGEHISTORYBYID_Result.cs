//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WE.DataAccess
{
    using System;
    
    public partial class USP_WE_GETMESSAGEHISTORYBYID_Result
    {
        public int TotalRowCount { get; set; }
        public long MessageHistoryId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromUserName { get; set; }
        public string ProfilePicURL { get; set; }
        public string MessageDetail { get; set; }
        public string MessageDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string FileName { get; set; }
        public string FileUniqueId { get; set; }
        public bool IsReported { get; set; }
        public bool IsSpam { get; set; }
        public bool IsStar { get; set; }
    }
}
