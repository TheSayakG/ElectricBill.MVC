using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerElectricBill.Models
{
    public class FeedbackModel
    {
        [Key]
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string Review { get; set; }

        [Range(1, 5, ErrorMessage = "Stars must be within 1-5")]
        public int Stars { get; set; }
    }
}
