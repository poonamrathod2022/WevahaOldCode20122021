using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WE.BusinessEntities
{
    public class RecentVisitors
    {
        public int ProfileId { get; set; }
        public int ViewedProfileId { get; set; }
        public string ProfileName { get; set; }
        public string ProfileLastName { get; set; }
        public string Age { get; set; }
        public string Height { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string Country { get; set; }
        public string MotherTongue { get; set; }
        public string DefaultPhotoUrl { get; set; }
    }
}
