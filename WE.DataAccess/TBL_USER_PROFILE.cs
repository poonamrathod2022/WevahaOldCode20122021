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
    using System.Collections.Generic;
    
    public partial class TBL_USER_PROFILE
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> SeekingGender { get; set; }
        public string Zipcode { get; set; }
        public Nullable<int> Country { get; set; }
        public string HowDidYouHearAboutUs { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
        public string ResetPasswordCode { get; set; }
        public string Geolocation { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> LastSeenDate { get; set; }
    }
}
