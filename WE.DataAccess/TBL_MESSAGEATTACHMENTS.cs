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
    
    public partial class TBL_MESSAGEATTACHMENTS
    {
        public long MessageAttachmentId { get; set; }
        public long MessageHistoryId { get; set; }
        public string FileName { get; set; }
        public string FileUniqueId { get; set; }
        public System.DateTime CreatedDate { get; set; }
    
        public virtual TBL_MESSAGEHISTORY TBL_MESSAGEHISTORY { get; set; }
    }
}
