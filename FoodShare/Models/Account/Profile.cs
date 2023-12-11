using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Account
{
    public class Profile
    {
        public string Id { get; set; }
        public string Role { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public long Number { get; set; }
        
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}