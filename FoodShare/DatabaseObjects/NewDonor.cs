using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.DatabaseObjects
{
    // When a new Donor creates an account, their information is saved into this NewDonor database object so that it can be sent to the database
    public class NewDonor
    {
        [Required]
        public string Organization { get; set; }
        
        public string Branch { get; set; }
        [Required]
        public int ZipCode { get; set; }
    }
}