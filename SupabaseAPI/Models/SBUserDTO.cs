namespace JwtWebApi.Models
{
    // User Data Transfer Object
    public class SBUserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}