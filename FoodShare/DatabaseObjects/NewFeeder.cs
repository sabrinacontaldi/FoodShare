using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FoodShare.DatabaseObjects
{
    // When a new Feeder creates an account, their information is saved into this NewFeeder database object so that it can be sent to the database
    public class NewFeeder
    {
        [Required]
        public string Organization { get; set; }
        [Required]
        public string Description { get; set; }

        public string Branch { get; set; }
        [Required]
        public int ZipCode { get; set; }

        // More information that could be collected
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