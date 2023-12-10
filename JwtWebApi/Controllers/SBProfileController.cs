// I dont think this is used at all 
// It was moved to the 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JwtWebApi.Contracts.Profile;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Supabase.Client _client;
       
        public ProfileController(Supabase.Client client, IConfiguration configuration)
        {
            this._client = client;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> NewProfille(CreateProfileRequest request)
        {
            var profile = new Profile
            {
                Id = request.Id,
                Role = request.Role,

                Name = request.Name,
                Email = request.Email,
                Number = request.Number,

                StreetAddress = request.StreetAddress,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode
            };

            var response = await _client.From<Profile>().Insert(profile);

            var newProfile = response.Models.First();

            return Ok(newProfile.Id);

        }

        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProfileResponse>> GetById(string id)
        {
            var response = await _client
                .From<Profile>()
                .Where(n => n.Id == id)
                .Get();
            
            var profile = response.Models.FirstOrDefault();

            if (profile is null)
            {
                return NotFound();
            }

            var profileResponse = new ProfileResponse
            {
                Id = profile.Id,
                Role = profile.Role,

                Name = profile.Name,
                Email = profile.Email,
                Number = profile.Number,

                StreetAddress = profile.StreetAddress,
                City = profile.City,
                State = profile.State,
                ZipCode = profile.ZipCode,

                CreatedAt = profile.CreatedAt
            };

            return Ok(profileResponse);

        }
        
        [HttpGet("getFeeders")]
        public async Task<ActionResult<ProfileResponse>> GetFeeders()
        {
            List <ProfileResponse> response = new List <ProfileResponse>();
            
            var dataList = await _client
                .From<Profile>()
                .Where(n => n.Role == "Feeder")
                .Get();
            
            dataList.Models.ForEach(s => response.Add(new ProfileResponse{
                Id = s.Id,
                Role = s.Role,

                Name = s.Name,
                Email = s.Email,
                Number = s.Number,

                StreetAddress = s.StreetAddress,
                City = s.City,
                State = s.State,
                ZipCode = s.ZipCode,

                CreatedAt = s.CreatedAt
            }));

           if (!response.Any())
            {
                Console.WriteLine("No data found.");
                return NotFound();
            }
            else
            {
                Console.WriteLine("Data found.");
                return Ok(response);
            }
        
        }

        // [HttpGet("getFeedersCloseToFar")]
        // public async Task<ActionResult<ProfileResponse>> GetFeedersCloseToFar(string zip)
        // {
        //     List <ProfileResponse> response = new List <ProfileResponse>();
            
        //     var dataList = await _client
        //         .From<Profile>()
        //         .Where(n => n.Role == "Feeder")
        //         .Get();
            
        //     dataList.Models.ForEach(s => response.Add(new ProfileResponse{
        //         Id = s.Id,
        //         Role = s.Role,

        //         Name = s.Name,
        //         Email = s.Email,
        //         Number = s.Number,

        //         StreetAddress = s.StreetAddress,
        //         City = s.City,
        //         State = s.State,
        //         ZipCode = s.ZipCode,

        //         CreatedAt = s.CreatedAt
        //     }));

        //    if (!response.Any())
        //     {
        //         Console.WriteLine("No data found.");
        //         return NotFound();
        //     }
        //     else
        //     {
        //         Console.WriteLine("Data found.");
        //         return Ok(response);
        //     }
        
        // }
    }
}