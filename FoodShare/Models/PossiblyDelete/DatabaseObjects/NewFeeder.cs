using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FoodShare.DatabaseObjects
{
    public class NewFeeder
    {
        [Required]
        public string Organization { get; set; }
        [Required]
        public string Description { get; set; }

        public string Branch { get; set; }
        [Required]
        public int ZipCode { get; set; }
        // [Required]
        // public DateTime StartDate { get; set; }

        // public NewFeeder(string org, string description, string branch, int zip)
        // {
        //     Organization = org;
        //     Description = description;
        //     Branch = branch;
        //     ZipCode = zip;
        //     // StartDate = DateTime.Today;
        // }
    }
}