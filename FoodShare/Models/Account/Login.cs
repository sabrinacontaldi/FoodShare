using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodShare.Models.Account
{
    public class Login
    {
        [JsonPropertyName("username")]
        [Required]
        public string Username {get; set;}
        
        [JsonPropertyName("password")]
        [Required]
        public string Password {get; set;}
    }
}