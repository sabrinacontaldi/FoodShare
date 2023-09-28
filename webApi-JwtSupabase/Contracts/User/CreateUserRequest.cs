using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts.User
{
    // Used to expose information from Api
    public class CreateUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}