using System.Text.Json.Serialization;

namespace FoodShare.Models
{
    // public class User{
    //     public string Id { get; set; }
    //     public string FirstName { get; set; }
    //     public string LastName { get; set; }
    //     public string UserName { get; set; }
    //     public string Token { get; set; }

    //     public bool IsDeleting { get; set; }
    // }

    //I don't know if this class is necessary - how often do you need to get a users information

    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("username")]
        public string username { get; set; }
        [JsonPropertyName("contact_person")]
        public string contact_person { get; set; }
        [JsonPropertyName("email_address")]
        public string email_address { get; set; }
        [JsonPropertyName("donor_id")]
        public int? donor_id { get; set; }
        [JsonPropertyName("feeder_id")]
        public int? feeder_id { get; set; }
    }
}