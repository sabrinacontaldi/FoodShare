using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models
{
    public class Item
    {
        public string Name { get; set; }
        public string QuantityType { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public bool selected {get; set;} = false;

        public Item()
        {
            
        }
        public Item(string name, string quantityType, int quantity){
            Name = name;
            QuantityType = quantityType;
            Quantity = quantity;
        }

    }
}