using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class AddUser
    {
        [Required]
        public string Organization { get; set; }
        [Required]
        public long Number { get; set; }
    
        //Address info
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public int ZipCode { get; set; }
        
        //Login will be done using Email and password 

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required(ErrorMessage = "Confirm Email is required.")]
        [EmailAddress]
        [Compare("EmailAddress",
            ErrorMessage = "The email address does not match")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "The Password must be a minimum of 8 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password",
            ErrorMessage = "The password does not match")]
        public string ConfirmPassword { get; set; }
    }
}