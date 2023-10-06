using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class SBLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        // public NewSBUser(string email, string password)
        // {
        //     Email = email;
        //     Password = password;
        // }
    }
}