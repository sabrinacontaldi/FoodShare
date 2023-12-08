using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts
{
    // Used to expose information from Api
    public class DonationRequestResponse
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string QuantityType { get; set; }
        public string FeederId { get; set; }
        public string DonorId { get; set; }
    }
}