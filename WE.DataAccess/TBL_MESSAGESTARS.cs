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
    
    public partial class TBL_MESSAGESTARS
    {
        public long MessageStarId { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public bool IsStar { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
