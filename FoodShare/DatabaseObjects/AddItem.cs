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
        public string ItemName { get; set; }

        [Required]
        public string ItemQuantityType { get; set; }
        
        [Required]
        public int ItemQuantity { get; set; }

        [Required]
        public int feeder_id {get; set; }
        // public string Status { get; set; }
        // [Required]
        // public DateTime RequestDate { get; set; }

        public AddItem(){}
        public AddItem(ShoppingList sl, Item item)
        {
            ItemName = item.ItemName;
            ItemQuantity = item.ItemQuantity;
            ItemQuantityType = item.ItemQuantityType;
            // RequestDate = sl.Date;
        }

    }

    
}