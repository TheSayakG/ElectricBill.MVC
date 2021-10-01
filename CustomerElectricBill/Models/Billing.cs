using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Models
{
    public class Billing
    {
        [Key]
        public int txid { get; set; }
        [Required]
        public int UserId { get; set; }
        public double readings { get; set; }
        public string DocumentUrl { get; set; }
    }
}
