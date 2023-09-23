using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.DatabaseObjects
{
    public class NewDonor
    {
        [Required]
        public string Organization { get; set; }
        
        public string Branch { get; set; }
        [Required]
        public int ZipCode { get; set; }
    }
}