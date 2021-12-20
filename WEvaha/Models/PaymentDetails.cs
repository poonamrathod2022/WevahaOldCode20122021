using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEvaha.Models
{
    public class PaymentDetails
    {
        
        public string FirstName { get; set; }
       
        public string PhoneNo { get; set; }
       
        public double Amount { get; set; }
      
        public string Email { get; set; }
        public string SuccessUrl { get; set; }
        public string FailUrl { get; set; }
       
        public string ProductInfo { get; set; }

        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string TxId { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string key { get; set; }
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string Hash { get; set; }

    }
}