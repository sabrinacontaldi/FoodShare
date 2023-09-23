using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Cors;

using Newtonsoft.Json;

namespace FoodShareApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly FoodshareContext DBContext;

        public object JsonConvert { get; private set; }

        public ItemController( FoodshareContext DBContext)
        {
            this.DBContext = DBContext;
        }

        //returns all donation requests (oldest to newest)
        [HttpGet("GetItems")]
        public async Task<ActionResult<List<DonationRequestDTO>>> GetItem()
        {
            // HttpContext.Response.Headers.Add("");
            var List = await DBContext.DonationRequests
            .Where(s => s.donor_id == null) 
            .Select(
                s => new DonationRequestDTO
                {
                    Id = s.Id,
                    ItemName = s.ItemName,
                    ItemQuantity = s.ItemQuantity,
                    ItemQuantityType = s.ItemQuantityType,
                    // RequestDate = s.RequestDate,
                    feeder_id = s.feeder_id,
                    donor_id = s.donor_id
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        //returns all donation requests ordered from newest to oldest
        [HttpGet("GetItemsNewToOld")]
        public async Task<ActionResult<List<DonationRequestDTO>>> GetItemNewToOld()
        {
            // HttpContext.Response.Headers.Add("");
            var List = await DBContext.DonationRequests
            .Where(s => s.donor_id == null) 
            .Select(
                s => new DonationRequestDTO
                {
                    Id = s.Id,
                    ItemName = s.ItemName,
                    ItemQuantity = s.ItemQuantity,
                    ItemQuantityType = s.ItemQuantityType,
                    // RequestDate = s.RequestDate,
                    feeder_id = s.feeder_id,
                    donor_id = s.donor_id
                }
            ).OrderByDescending(d => d.Id)
            .ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        //returns all donation requests linked to a specified feeder id
        [HttpGet("GetItemsByFeeder/{id}")]
        public async Task<ActionResult<List<DonationRequestDTO>>> GetItemByFeeder(int Id)
        {
            var List = await DBContext.DonationRequests.Where(s => s.feeder_id == Id)
            .Select(
                    s => new DonationRequestDTO
                    {
                        Id = s.Id,
                        ItemName = s.ItemName,
                        ItemQuantity = s.ItemQuantity,
                        ItemQuantityType = s.ItemQuantityType,
                        // RequestDate = s.RequestDate,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    }).ToListAsync();

           if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }
        
        //returns a donation request linked to a specified id
        [HttpGet("GetItemById")]
        public async Task<ActionResult<DonationRequestDTO>> GetItemById(int Id)
        {
            DonationRequestDTO Item = await DBContext.DonationRequests.Select(
                    s => new DonationRequestDTO
                    {
                        Id = s.Id,
                        ItemName = s.ItemName,
                        ItemQuantity = s.ItemQuantity,
                        ItemQuantityType = s.ItemQuantityType,
                        // RequestDate = s.RequestDate,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    })
                .FirstOrDefaultAsync(s => s.Id == Id);

            if (Item == null)
            {
                return NotFound();
            }
            else
            {
                return Item;
            }
        }
        
        
        [HttpPost("InsertItem")]
        // [HttpHead("Access-Control-Allow-Origin: *")]
        public async Task<HttpStatusCode> InsertItem(DonationRequestDTO Item)
        {
            var entity = new DonationRequest()
            {
                // Id = Item.Id,
                ItemName = Item.ItemName,
                ItemQuantity = Item.ItemQuantity,
                ItemQuantityType = Item.ItemQuantityType,
                // RequestDate = Item.RequestDate,
                feeder_id = Item.feeder_id,
                donor_id = Item.donor_id
            };

            DBContext.DonationRequests.Add(entity);
            await DBContext.SaveChangesAsync();
            // var status = HttpStatusCode.Created + HttpRequestHeader
            return HttpStatusCode.Created;
        }

        [HttpPost("InsertItemRID")]
        // [HttpHead("Access-Control-Allow-Origin: *")]
        public async Task<int> InsertItemRID(DonationRequestDTO Item)
        {
            var entity = new DonationRequest()
            {
                // Id = Item.Id,
                ItemName = Item.ItemName,
                ItemQuantity = Item.ItemQuantity,
                ItemQuantityType = Item.ItemQuantityType,
                // RequestDate = Item.RequestDate,
                feeder_id = Item.feeder_id,
                donor_id = Item.donor_id
            };

            DBContext.DonationRequests.Add(entity);
            await DBContext.SaveChangesAsync();
            // var status = HttpStatusCode.Created + HttpRequestHeader
            return entity.Id;
        }

        
        [HttpPut("UpdateItem")]
        public async Task<HttpStatusCode> UpdateItem(DonationRequestDTO Item)
        {
            var entity = await DBContext.DonationRequests.FirstOrDefaultAsync(s => s.Id == Item.Id);

                entity.ItemName = Item.ItemName;
                entity.ItemQuantity = Item.ItemQuantity;
                entity.ItemQuantityType = Item.ItemQuantityType;
                // entity.RequestDate = Item.RequestDate;
                entity.feeder_id = Item.feeder_id;
                entity.donor_id = Item.donor_id;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpPut("UpdateDonorId")]
        public async Task<HttpStatusCode> UpdateDonorId(DonationRequestDTO Item)
        {
            var entity = await DBContext.DonationRequests.FirstOrDefaultAsync(s => s.Id == Item.Id);

                entity.ItemName = Item.ItemName;
                entity.ItemQuantity = Item.ItemQuantity;
                entity.ItemQuantityType = Item.ItemQuantityType;
                // entity.RequestDate = Item.RequestDate;
                entity.feeder_id = Item.feeder_id;
                entity.donor_id = Item.donor_id;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        //Delete a donation request given a specific id
        [HttpDelete("DeleteItem/{Id}")]
        public async Task<HttpStatusCode> DeleteItem(int Id)
        {
            var entity = new DonationRequest()
            {
                Id = Id
            };
            DBContext.DonationRequests.Attach(entity);
            DBContext.DonationRequests.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}