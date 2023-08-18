using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FoodShare.Models
{
    public class Feeder
    {
        [JsonPropertyName("id")]
        public int id { get; set; }
        [JsonPropertyName("organization")]
        public string organization { get; set; }
        [JsonPropertyName("description")]
        public string description { get; set; }
        [JsonPropertyName("branch")]
        public string branch { get; set; }
        [JsonPropertyName("zipCode")]
        public int zipCode { get; set; }
        // public DateTimeOffset StartDate { get; set; }

        //street address
        //city
        //state

        public Feeder() {}

        public Feeder(int Id,string org, string Description, string Branch, int zip)
        {
            id = Id;
            organization = org;
            description = Description;
            branch = Branch;
            zipCode = zip;
            // StartDate = DateTime.Today;
        }

    }

}