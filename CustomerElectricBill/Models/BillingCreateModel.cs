using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Models
{
    public class BillingCreateModel
    {
        [Required]
        public int UserId { get; set; }
        public double readings { get; set; }
        public IFormFile Document { get; set; }
    }
}
