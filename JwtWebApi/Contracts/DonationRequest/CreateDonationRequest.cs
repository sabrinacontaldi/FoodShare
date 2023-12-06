using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts
{
    // Used to expose information from Api
    public class CreateDonationRequest
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string QuantityType { get; set; }
        public string FeederId { get; set; }
    }
}