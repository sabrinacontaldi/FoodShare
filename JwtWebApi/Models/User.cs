using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace JwtWebApi
{
    [Table("user")]
    public class User : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("username")]
        public string Username { get; set; } = string.Empty;
        [Column("hashed_password")]
        public byte[] PasswordHash { get; set; }
        [Column("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Refresh token stuff
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

    }
}