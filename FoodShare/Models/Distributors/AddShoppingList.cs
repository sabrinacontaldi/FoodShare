using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShare.Models.Distributors
{
    public class AddShoppingList
    {
        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int ItemQuantity { get; set; }

        [Required]
        public string ItemQuantityType { get; set; }

    }
}