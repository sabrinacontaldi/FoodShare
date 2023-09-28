using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts
{
    public class NewsletterResponse
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReadTime { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}