using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models
{
    public class Item
    {
        public string ItemName { get; set; }
        public string ItemQuantityType { get; set; }
        public int ItemQuantity { get; set; }
        public string Status { get; set; }
        public bool selected {get; set;} = false;

        public Item()
        {
            
        }
        public Item(string name, string quantityType, int quantity){
            ItemName = name;
            ItemQuantityType = quantityType;
            ItemQuantity = quantity;
        }

    }
}