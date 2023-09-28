using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JwtWebApi.Contracts;
using JwtWebApi.Contracts.Newsletter;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Supabase.Client _client;
       
        public NewsletterController(Supabase.Client client, IConfiguration configuration)
        {
            this._client = client;
            _configuration = configuration;
        }

        [HttpPost("new")]
        public async Task<ActionResult<string>> NewNewsletter(CreateNewsletterRequest request)
        {
            var newsletter = new Newsletter
            {
                Name = request.Name,
                Description = request.Description,
                ReadTime = request.ReadTime
            };

            var response = await _client.From<Newsletter>().Insert(newsletter);

            var newNewsletter = response.Models.First();

            return Ok(newNewsletter.Id);

        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<NewsletterResponse>> GetById(long id)
        {
            var response = await _client
                .From<Newsletter>()
                .Where(n => n.Id == id)
                .Get();
            
            var newsletter = response.Models.FirstOrDefault();

            if (newsletter is null)
            {
                return NotFound();
            }

            var newsletterResponse = new NewsletterResponse
            {
                id = newsletter.Id,
                Name = newsletter.Name,
                Description = newsletter.Description,
                ReadTime = newsletter.ReadTime,
                CreatedAt = newsletter.CreatedAt
            };

            return Ok(newsletterResponse);

        }
        
        // Even if the id doesn't exist, the same code is being returned
        [HttpDelete("delete/{id}")]
        public async Task<HttpStatusCode> DeleteItem(long id)
        {
            await _client
                .From<Newsletter>()
                .Where(n => n.Id == id)
                .Delete();
            
            return HttpStatusCode.OK;
        }
    }
}