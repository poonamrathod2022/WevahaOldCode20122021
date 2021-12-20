using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEvaha.Models
{
    public class ProfilePhoto
    {
        public int PhotoId { get; set; }
        public string PhotoName { get; set; }
        public string PhotoUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public int PhotoType { get; set; }
        public int ProfileId { get; set; }
    }
}