using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class AddPassword
    {
        [Required]
        public int Id {get; set;}
        
        [Required]
        public string Password {get; set;}

        public AddPassword()
        {}

        public AddPassword(int i, string p)
        {
            Id = i;
            Password = p;
        }
    }
}