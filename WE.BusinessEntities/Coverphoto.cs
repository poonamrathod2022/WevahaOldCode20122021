using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WE.BusinessEntities
{
    public class Coverphoto
    {
        public int CoverPhotoId { get; set; }
        public string PhotoName { get; set; }
        public string CoverPhotoURL { get; set; }      
        public int CoverPhotoType { get; set; }
        public int ProfileId { get; set; }
        public int Status { get; set; }
    }
}
