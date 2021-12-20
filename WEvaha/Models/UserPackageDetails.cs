using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEvaha.Models
{
    public class UserPackageDetails
    {
        public int ProfileId { get; set; }
        public int PackageId { get; set; }
        public int ByCountry { get; set; }
        public string TrasactionId { get; set; }
        public string PackageName { get; set; }
        public int PackageDuration { get; set; }
        public string PackageStartDate { get; set; }
        public string PackageEndDate { get; set; }
        public string PackagePrice { get; set; }
        public string PayMode { get; set; }
        public string PerMonth { get; set; }
        public string Status { get; set; }
    }
}