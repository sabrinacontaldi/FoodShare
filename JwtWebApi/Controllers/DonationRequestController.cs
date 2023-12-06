using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JwtWebApi.Contracts;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // help secure the webApi
    // [Authorize]

    public class DonationRequestController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Supabase.Client _client;

        public DonationRequestController(Supabase.Client client, IConfiguration configuration)
        {
            this._client = client;
            _configuration = configuration;
        }

    // Edited for Supabase
    //------------------------------------------------------
    // Get Requests
        //returns a donation request linked to a specified id
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<DonationRequestResponse>> GetById(long Id)
        {

            var response = await _client
                .From<DonationRequest>()
                .Where(n => n.Id == Id)
                .Get();
            
            var donationRequest = response.Models.FirstOrDefault();

            if (donationRequest is null)
            {
                return NotFound();
            }

            var donationRequestResponse = new DonationRequestResponse
            {
                Id = donationRequest.Id,
                CreatedAt = donationRequest.CreatedAt,
                ItemName = donationRequest.ItemName,
                Quantity = donationRequest.Quantity,
                QuantityType = donationRequest.QuantityType,
                FeederId = donationRequest.FeederId
            };

            return Ok(donationRequestResponse);
        }

        //returns all donation requests with a specified FeederId
        [HttpGet("GetItemsByFeeder/{id}")]
        public async Task<ActionResult<List<DonationRequestResponse>>> GetItemByFeeder(string id)
        {
            List <DonationRequestResponse> response = new List <DonationRequestResponse>();
            
            var dataList = await _client
                .From<DonationRequest>()
                .Where(s => s.FeederId == id)
                .Get();

            // if (dataList.Error != null)
            // {
            //     // Log the error
            //     Console.WriteLine($"Error fetching data: {dataList.Error.Message}");
            //     return BadRequest(dataList.Error.Message);
            // }


            dataList.Models.ForEach(s => response.Add(new DonationRequestResponse{
                Id = s.Id,
                ItemName = s.ItemName,
                Quantity = s.Quantity,
                QuantityType = s.QuantityType,
                FeederId = s.FeederId,
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


        //add a new donation request to the table
        [HttpPost("new")]
        public async Task<ActionResult<string>> InsertItem(CreateDonationRequest request)
        {
            var donationRequest = new DonationRequest
            {
                ItemName = request.ItemName,
                Quantity = request.Quantity,
                QuantityType = request.QuantityType,
                FeederId = request.FeederId,
                // donor_id = Item.donor_id
            };

            var response = await _client.From<DonationRequest>().Insert(donationRequest);

            var newDonationRequest = response.Models.First();

            return Ok(newDonationRequest.Id);
        }

        // Update an item
        [HttpPut("update")]
        public async Task<HttpStatusCode> UpdateItem(DonationRequest Item)
        {
            var entity = await _client
                .From<DonationRequest>()
                .Where(x => x.Id == Item.Id)
                .Single();

            entity.ItemName = Item.ItemName;
            entity.Quantity = Item.Quantity;
            entity.QuantityType = Item.QuantityType;
            
            await entity.Update<DonationRequest>();
            return HttpStatusCode.OK;
        }
    
        //Delete a donation request given a specific id
        [HttpDelete("delete/{id}")]
        public async Task<HttpStatusCode> DeleteItem(long id)
        {
            await _client
                .From<DonationRequest>()
                .Where(n => n.Id == id)
                .Delete();
            
            return HttpStatusCode.OK;
        }


    }
}
    // ----------------------------------------------------
    // For Donor
    // returns all donation requests (oldest to newest)
    // [HttpGet("GetItems")]
    // public async Task<ActionResult<List<DonationRequestDTO>>> GetItem()
    // {
    //     // HttpContext.Response.Headers.Add("");
    //     var List = await _client
    //         .From<DonationRequest>()
    //         .Where(s => s.donor_id == null) 
    //         .Select(
    //             s => new DonationRequestDTO
    //             {
    //                 Id = s.Id,
    //                 ItemName = s.ItemName,
    //                 ItemQuantity = s.ItemQuantity,
    //                 ItemQuantityType = s.ItemQuantityType,
    //                 // RequestDate = s.RequestDate,
    //                 feeder_id = s.feeder_id,
    //                 donor_id = s.donor_id
    //             }
    //         ).ToListAsync();

    //     if (List.Count < 0)
    //     {
    //         return NotFound();
    //     }
    //     else
    //     {
    //         return List;
    //     }
    // }
    
    
    //     //returns all donation requests ordered from newest to oldest
    //     [HttpGet("GetItemsNewToOld")]
    //     public async Task<ActionResult<List<DonationRequestDTO>>> GetItemNewToOld()
    //     {
    //         // HttpContext.Response.Headers.Add("");
    //         var List = await DBContext.DonationRequests
    //         .Where(s => s.donor_id == null) 
    //         .Select(
    //             s => new DonationRequestDTO
    //             {
    //                 Id = s.Id,
    //                 ItemName = s.ItemName,
    //                 ItemQuantity = s.ItemQuantity,
    //                 ItemQuantityType = s.ItemQuantityType,
    //                 // RequestDate = s.RequestDate,
    //                 feeder_id = s.feeder_id,
    //                 donor_id = s.donor_id
    //             }
    //         ).OrderByDescending(d => d.Id)
    //         .ToListAsync();

    //         if (List.Count < 0)
    //         {
    //             return NotFound();
    //         }
    //         else
    //         {
    //             return List;
    //         }
    //     }

    
    //     [HttpPost("InsertItemRID")]
    //     // [HttpHead("Access-Control-Allow-Origin: *")]
    //     public async Task<int> InsertItemRID(DonationRequestDTO Item)
    //     {
    //         var entity = new DonationRequest()
    //         {
    //             // Id = Item.Id,
    //             ItemName = Item.ItemName,
    //             ItemQuantity = Item.ItemQuantity,
    //             ItemQuantityType = Item.ItemQuantityType,
    //             // RequestDate = Item.RequestDate,
    //             feeder_id = Item.feeder_id,
    //             donor_id = Item.donor_id
    //         };

    //         DBContext.DonationRequests.Add(entity);
    //         await DBContext.SaveChangesAsync();
    //         // var status = HttpStatusCode.Created + HttpRequestHeader
    //         return entity.Id;
    //     }

    //     [HttpPut("UpdateDonorId")]
    //     public async Task<HttpStatusCode> UpdateDonorId(DonationRequestDTO Item)
    //     {
    //         var entity = await DBContext.DonationRequests.FirstOrDefaultAsync(s => s.Id == Item.Id);

    //             entity.ItemName = Item.ItemName;
    //             entity.ItemQuantity = Item.ItemQuantity;
    //             entity.ItemQuantityType = Item.ItemQuantityType;
    //             // entity.RequestDate = Item.RequestDate;
    //             entity.feeder_id = Item.feeder_id;
    //             entity.donor_id = Item.donor_id;

    //         await DBContext.SaveChangesAsync();
    //         return HttpStatusCode.OK;
    //     }
    // }