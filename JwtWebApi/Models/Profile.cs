using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace JwtWebApi.Models
{
    [Table("profile")]
    public class Profile : BaseModel
    {
        // set should insert to true OR IT WON"T WORK
        [PrimaryKey("id", true)]
        public string Id { get; set; }

        [Column("role")]
        public string Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("organization_name")]
        public string Name { get; set; }
        
        [Column("email_address")]
        public string Email { get; set; }
        
        [Column("contact_number")]
        public long Number { get; set; }

        [Column("street_address")]
        public string StreetAddress { get; set; }

        [Column("city")]
        public string City { get; set; }
        
        [Column("state")]
        public string State { get; set; }

        [Column("zip_code")]
        public int ZipCode { get; set; }



        // Refresh token stuff -> Not sure if this is the place for it
        // public string RefreshToken { get; set; } = string.Empty;
        // public DateTime TokenCreated { get; set; }
        // public DateTime TokenExpires { get; set; }
    }
}