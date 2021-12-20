using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WE.BusinessEntities
{
    public class UserPackageDetails
    {
        public int ProfileId { get; set; }
        public int PackageId { get; set; }
        public int ByCountry { get; set; }
        public string TrasactionId { get; set; }
        public string PackageName { get; set; }
        public int PackageDuration { get; set; }
        public DateTime PackageStartDate { get; set; }
        public DateTime PackageEndDate { get; set; }
        public string PackagePrice { get; set; }
        public string PayMode { get; set; }
        public string PerMonth { get; set; }
        public string Status { get; set; }
    }
}
