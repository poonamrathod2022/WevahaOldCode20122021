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
    
    public partial class USP_WE_GET_USER_PROFILE_BY_ID_Result
    {
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileLastName { get; set; }
        public string EmailId { get; set; }
        public Nullable<int> Gender { get; set; }
        public string DateofBirth { get; set; }
        public Nullable<int> SeekingGender { get; set; }
        public string Zipcode { get; set; }
        public Nullable<int> Country { get; set; }
        public string HowDidYouHearAboutUs { get; set; }
    }
}
