using System;
using System.Collections.Generic;

namespace FoodShareApi.Entities;

public partial class User
{
    public int id { get; set; }
    public string username { get; set; }
    public string contact_person { get; set; }
    public string email_address { get; set; }
    public int? feeder_id { get; set; } = null;
    public int? donor_id { get; set; } = null;
    // public int Id { get; set; }

    // public string FirstName { get; set; } = null!;

    // public string LastName { get; set; } = null!;

    // public string Username { get; set; } = null!;

    // public string Password { get; set; } = null!;

    // public DateTime EnrollmentDate { get; set; }
}
