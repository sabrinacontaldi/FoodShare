using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class NewSBUser
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public NewSBUser(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}