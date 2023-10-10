using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts.SBUser
{
    public class CreateSBUserRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public string Role { get; set; }
        public string OrganizationName { get; set; }
        
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        
    }
}
