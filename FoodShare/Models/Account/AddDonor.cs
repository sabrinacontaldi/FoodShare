using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class AddDonor
    {
        //General Info
        [Required]
        public string Organization { get; set; }

        //Should branch be required? Maybe if organization exists, branch is required
        [Required]
        public string Branch { get; set; }

        //Address info
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int ZipCode { get; set; }

        //Contact person + Account info
         [Required]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        [EmailAddress]
        [Compare("EmailAddress",
            ErrorMessage = "The email address does not match")]
        public string ConfirmEmail { get; set; }

        //Login will be done using Username and password - ensures multiple people can access the same account
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "The Password must be a minimum of 8 characters")]
        public string Password { get; set; }
        [Required]
        [Compare("Password",
            ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }
    }
}