using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtWebApi.Contracts.Newsletter
{
    // Used to expose information from Api
    public class CreateNewsletterRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ReadTime { get; set; }
    }
}