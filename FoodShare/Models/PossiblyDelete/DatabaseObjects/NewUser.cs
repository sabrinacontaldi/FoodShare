using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.DatabaseObjects
{
    public class NewUser
    {
        // [Required]
        // public int id { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string contact_person { get; set; }
        [Required]
        [EmailAddress]
        public string email_address { get; set; }
        public int donor_id { get; set; }
        public int feeder_id { get; set; }
    }
}   