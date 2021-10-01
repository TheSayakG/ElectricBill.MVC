using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Models
{
    public class UploadModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
    }
}
