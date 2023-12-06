using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;

namespace JwtWebApi.Models
{
    [Table("donation_request")]
    public class DonationRequest : BaseModel
    {
        // set should insert to false - should auto generate id
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("item_name")]
        public string ItemName { get; set; }
        
        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("quantity_type")]
        public string QuantityType { get; set; }

        [Column("feeder_id")]
        public string FeederId { get; set; }
        
        // [Column("donor_id")]
        // public string DonorId { get; set; }

    }
}