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
    
    public partial class TBL_MESSAGELABELS
    {
        public long MessageLabelId { get; set; }
        public int UserId { get; set; }
        public string LabelName { get; set; }
        public bool IsDelete { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<bool> CanDelete { get; set; }
    }
}
