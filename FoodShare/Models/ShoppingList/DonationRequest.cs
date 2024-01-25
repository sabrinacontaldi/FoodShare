using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FoodShare;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoodShare.Models
{
    public class DonationRequest
    {
        public int Id { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        // [RegularExpression("^(?!-).*$", ErrorMessage = "Please select a valid value.")]
        public string QuantityType { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public int Quantity { get; set; }

        // [JsonPropertyName("RequestDate")]
        // public Date RequestDate { get; set; }

        public string FeederId { get; set;}

        public string? DonorId { get; set;} = null;

        public bool selected {get; set;} = false;

        public DonationRequest(){}

        // public DonationRequest(int Id, AddItem item)
        // {
        //     Id = Id;
        //     ItemName = item.ItemName;
        //     QuantityType = item.QuantityType;
        //     Quantity = item.Quantity;
        // }

        // public DonationRequest(AddItem item)
        // {
        //     // id = ID;
        //     ItemName = item.Name;
        //     QuantityType = item.QuantityType;
        //     Quantity = item.Quantity;
        //}
    }
}