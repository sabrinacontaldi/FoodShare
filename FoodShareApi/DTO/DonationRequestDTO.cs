public class DonationRequestDTO
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string ItemQuantityType { get; set; }
    public int ItemQuantity { get; set; }
    // public string Status { get; set; }
    // public DateTime RequestDate { get; set; }
    public int feeder_id { get; set; }
    public int? donor_id { get; set; } = null;
}
