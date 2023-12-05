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
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("ItemName")]
        [Required]
        public string ItemName { get; set; }

        [JsonPropertyName("ItemQuantityType")]
        [Required]
        [RegularExpression("^(?!-).*$", ErrorMessage = "Please select a valid value.")]
        public string ItemQuantityType { get; set; }

        [JsonPropertyName("ItemQuantity")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int ItemQuantity { get; set; }

        // [JsonPropertyName("RequestDate")]
        // public Date RequestDate { get; set; }

        [JsonPropertyName("feeder_id")]
        [Required]
        public int FeederId { get; set;}

        [JsonPropertyName("donor_id")]
        public int? DonorId { get; set;} = null;

        public bool selected {get; set;} = false;

        public DonationRequest(){}

        public DonationRequest(int ID, AddItem item)
        {
            id = ID;
            ItemName = item.ItemName;
            ItemQuantityType = item.ItemQuantityType;
            ItemQuantity = item.ItemQuantity;
        }

        public DonationRequest(AddItem item)
        {
            // id = ID;
            ItemName = item.ItemName;
            ItemQuantityType = item.ItemQuantityType;
            ItemQuantity = item.ItemQuantity;
        }
    }
}