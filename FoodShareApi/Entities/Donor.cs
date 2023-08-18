using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShareApi.Entities
{
    public partial class Donor
    {
        public int id { get; set; }
        public string organization { get; set; }
        public string branch { get; set; }
        public int zipCode { get; set; }
        // public DateTimeOffset StartDate { get; set; }

    }
}