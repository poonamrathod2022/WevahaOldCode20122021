using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WE.BusinessEntities
{
    public class WevahaViewModel
    {
        public LoginEntity login { get; set; }
        public UserProfile userProfile { get; set; }
        public FactFile factFile { get; set; }
        public PartnerSpecifications partnerSpecifications { get; set; }
    }
}