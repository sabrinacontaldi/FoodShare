using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FoodShare.DatabaseObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodShare.Models
{
    public class DonationRequest
    {
        [JsonPropertyName("Id")]
        public int id { get; set; }

        [JsonPropertyName("Name")]
        [Required]
        public string Name { get; set; }

        [JsonPropertyName("QuantityType")]
        [Required]
        [RegularExpression("^(?!-).*$", ErrorMessage = "Please select a valid value.")]
        public string QuantityType { get; set; }

        [JsonPropertyName("Quantity")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int Quantity { get; set; }

        // [JsonPropertyName("RequestDate")]
        // public Date RequestDate { get; set; }

        [JsonPropertyName("FeederId")]
        [Required]
        public string FeederId { get; set;}

        // [JsonPropertyName("donor_id")]
        // public int? DonorId { get; set;} = null;

        public bool selected {get; set;} = false;

        public DonationRequest(){}

        public DonationRequest(int Id, AddItem item)
        {
            id = Id;
            Name = item.Name;
            QuantityType = item.QuantityType;
            Quantity = item.Quantity;
        }

        public DonationRequest(AddItem item)
        {
            // id = ID;
            Name = item.Name;
            QuantityType = item.QuantityType;
            Quantity = item.Quantity;
        }
    }
}