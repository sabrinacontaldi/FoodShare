using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts.User
{
    public class UserResponse
    {
        public long id { get; set; }
        public string Username { get; set; } 
        public string PasswordHash { get; set; }
        public int PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}