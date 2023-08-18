public class UserDTO
{
    public int id { get; set; }
    public string username { get; set; }
    public string contact_person { get; set; }
    public string email_address { get; set; }
    public int? feeder_id { get; set; } = null;
    public int? donor_id { get; set; } = null;
    // public string Password { get; set; }
    // public DateTime EnrollmentDate { get; set; }
}