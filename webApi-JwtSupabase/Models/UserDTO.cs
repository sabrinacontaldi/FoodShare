namespace JwtWebApi
{
    // User Data Transfer Object
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}