using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using FoodShare.Models;

namespace FoodShare.DatabaseObjects
{
    public class AddItem
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string QuantityType { get; set; }
        
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string FeederId {get; set; }
        // public string Status { get; set; }
        // [Required]
        // public DateTime RequestDate { get; set; }

        public AddItem(){}
        public AddItem(ShoppingList sl, Item item)
        {
            Name = item.Name;
            Quantity = item.Quantity;
            QuantityType = item.QuantityType;
            // RequestDate = sl.Date;
        }

    }

    
}