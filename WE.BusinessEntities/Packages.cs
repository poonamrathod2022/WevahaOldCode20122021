using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WE.BusinessEntities
{
    public class Packages
    {
        public int PackageId { get; set; }
        public int ByCountry { get; set; }
        public string PackageName { get; set; }
        public int PackageDuration { get; set; }
        public string PackagePrice { get; set; }
        public string PerMonth { get; set; }
        public int Status { get; set; }
    }
}
