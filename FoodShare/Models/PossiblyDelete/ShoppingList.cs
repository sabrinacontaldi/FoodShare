using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models
{
    public class ShoppingList
    {
        // public string ItemName { get; set; }
        // public string ItemQuantityType { get; set; }

        // public int ItemQuantity { get; set; }

        public List<Item> Items { get; set; }
        public DateTime Date { get; set; }

        public int People { get; set; }

        public ShoppingList()
        {
            Items = new List<Item>
            {
                new Item {}
            };

            Date = DateTime.Today;

            People = 0;
        }

        public ShoppingList( List<Item> items)
        {
            Items = items;

            Date = DateTime.Today;

            People = 0;
        }


    }
}