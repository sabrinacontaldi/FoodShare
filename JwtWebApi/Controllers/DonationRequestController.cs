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
    //add a new donation request to the table
        [HttpPost("new")]
        public async Task<ActionResult<string>> InsertItem(CreateDonationRequest request)
        {
            var donationRequest = new DonationRequest
            {
                ItemName = request.ItemName,
                Quantity = request.Quantity,
                QuantityType = request.QuantityType,
                FeederId = request.FeederId
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
            // Do we need the option to update donor id?
            
            await entity.Update<DonationRequest>();
            return HttpStatusCode.OK;
        }

        //Update donor id used when a donor agrees to donate an item
        [HttpPut("UpdateDonorId")]
        public async Task<HttpStatusCode> UpdateDonorId(DonationRequestResponse Item)
        {
            var entity = await _client
                .From<DonationRequest>()
                .Where(x => x.Id == Item.Id)
                .Single();

            entity.DonorId = Item.DonorId;
            // Do we need the option to update donor id?
            
            await entity.Update<DonationRequest>();
            return HttpStatusCode.OK;

            // var entity = await DBContext.DonationRequests.FirstOrDefaultAsync(s => s.Id == Item.Id);

            //     entity.ItemName = Item.ItemName;
            //     entity.ItemQuantity = Item.ItemQuantity;
            //     entity.ItemQuantityType = Item.ItemQuantityType;
            //     // entity.RequestDate = Item.RequestDate;
            //     entity.feeder_id = Item.feeder_id;
            //     entity.donor_id = Item.donor_id;

            // await DBContext.SaveChangesAsync();
            // return HttpStatusCode.OK;
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
                FeederId = donationRequest.FeederId,
                DonorId = donationRequest.DonorId
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

            dataList.Models.ForEach(s => response.Add(new DonationRequestResponse{
                Id = s.Id,
                ItemName = s.ItemName,
                Quantity = s.Quantity,
                QuantityType = s.QuantityType,
                FeederId = s.FeederId,
                DonorId = s.DonorId,
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

        // For Donor views:
         // returns all donation requests (oldest to newest)
        [HttpGet("GetItems")]
        public async Task<ActionResult<List<DonationRequestResponse>>> GetItem()
        {
            
            List <DonationRequestResponse> response = new List <DonationRequestResponse>();
            
            var dataList = await _client
                .From<DonationRequest>()
                // Only items that still need to be donated
                .Where(s => s.DonorId == null)
                .Get();

            dataList.Models.ForEach(s => response.Add(new DonationRequestResponse{
                Id = s.Id,
                ItemName = s.ItemName,
                Quantity = s.Quantity,
                QuantityType = s.QuantityType,
                FeederId = s.FeederId,
                DonorId = s.DonorId,
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

        //returns all donation requests ordered from newest to oldest
        // [HttpGet("GetItemsNewToOld")]
        // public async Task<ActionResult<List<DonationRequestResponse>>> GetItemNewToOld()
        // {
        //     List <DonationRequestResponse> response = new List <DonationRequestResponse>();
            
        //     var dataList = await _client
        //         .From<DonationRequest>()
        //         // Only items that still need to be donated
        //         .Where(s => s.DonorId == null)
        //         // This is what the supabase c# client says you should do but it gives an error
        //         .Order(d => d.Id, Ordering.Descending)
        //         .Get();

        //     dataList.Models.ForEach(s => response.Add(new DonationRequestResponse{
        //         Id = s.Id,
        //         ItemName = s.ItemName,
        //         Quantity = s.Quantity,
        //         QuantityType = s.QuantityType,
        //         FeederId = s.FeederId,
        //         DonorId = s.DonorId,
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

    //     
    // }