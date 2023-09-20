using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Cors;
using System.Data.SqlClient;
// Auth0
using Microsoft.AspNetCore.Authorization;


namespace FoodShareApi.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    // Auth0
    // help secure the webApi
    [Authorize]
    public class FeederController : ControllerBase
    {
        private readonly FoodshareContext DBContext;

        public FeederController( FoodshareContext DBContext)
        {
            this.DBContext = DBContext;
        }
        
        //GET ALL FEEDERS (not ordered in any way)
        [HttpGet("GetFeeders")]
        public async Task<ActionResult<IEnumerable<FeederDTO>>> GetFeeder()
        {
            // return await DBContext.Feeders.ToListAsync();
            var List = await DBContext.Feeders.Select(
                s => new FeederDTO
                {
                    id = s.id,
                    organization= s.organization,
                    description = s.description,
                    branch = s.branch,
                    zipCode = s.zipCode
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

    //FILTER OPTIONS:
        //GET FEEDERS CLOSEST TO FURTHEST
        [HttpGet("GetFeedersCloseToFar")]
        public async Task<ActionResult<IEnumerable<FeederDTO>>> GetFeedersCloseToFar()
        {
             var List = await DBContext.Feeders.Select(
                s => new FeederDTO
                {
                    id = s.id,
                    organization= s.organization,
                    description = s.description,
                    branch = s.branch,
                    zipCode = s.zipCode
                })
                .OrderBy(f => Math.Abs(f.zipCode - 46135))
                .ToListAsync();
            
            return List;
        }

            //GET FEEDERS MOST ITEMS TO LEAST ITEMS
            [HttpGet("GetFeedersMostToLeast")]
            public async Task<ActionResult<IEnumerable<FeederDTO>>> GetFeedersMostToLeast()
            {
                    var List = await DBContext.Feeders
                    .OrderByDescending(f => DBContext.DonationRequests.Count(d => d.feeder_id == f.id))
                    .Select(s => new FeederDTO{
                        id = s.id,
                        organization= s.organization,
                        description = s.description,
                        branch = s.branch,
                        zipCode = s.zipCode,

                    })
                    .ToListAsync();

                return List;
            }
        
            //GET FEEDERS LEAST ITEMS TO MOST ITEMS 
            [HttpGet("GetFeedersLeastToMost")]
            public async Task<ActionResult<IEnumerable<FeederDTO>>> GetFeedersLeastToMost()
            {
                    var List = await DBContext.Feeders
                    .OrderBy(f => DBContext.DonationRequests.Count(d => d.feeder_id == f.id))
                    .Select(s => new FeederDTO{
                        id = s.id,
                        organization= s.organization,
                        description = s.description,
                        branch = s.branch,
                        zipCode = s.zipCode,

                    })
                    .ToListAsync();
                return List;
            }

        //GET FEEDER BY ID 
        [HttpGet("GetFeederById/{id:int}")]
        public async Task<ActionResult<FeederDTO>> GetFeederById(int id)
        {
            FeederDTO feeder = await DBContext.Feeders
                .Where(f => f.id == id)
                .Select(f => new FeederDTO
                {
                    id = f.id,
                    organization = f.organization,
                    description = f.description,
                    branch = f.branch,
                    zipCode = f.zipCode,
                    // StartDate = f.StartDate
                })
                .FirstOrDefaultAsync();

            if (feeder == null)
            {
                return NotFound();
            }

            return feeder;
        }

        //ADD NEW FEEDER TO TABLE
        [HttpPost("NewFeeder")]
        public async Task<ActionResult<int>> NewFeeder(FeederDTO Feeder)
        {
            Feeder f = new Feeder()
            {
                organization = Feeder.organization,
                description = Feeder.description,
                branch = Feeder.branch,
                zipCode = Feeder.zipCode,
                // StartDate = Feeder.StartDate
            };

            DBContext.Feeders.Add(f);
            await DBContext.SaveChangesAsync();

            return f.id;
        }
       

        [HttpPost("InsertFeeder")]
        // [HttpHead("Access-Control-Allow-Origin: *")]
        public async Task<HttpStatusCode> InsertFeeder(FeederDTO Feeder)
        {
            var entity = new Feeder()
            {
                id = Feeder.id,
                organization = Feeder.organization,
                description = Feeder.description,
                branch = Feeder.branch,
                zipCode = Feeder.zipCode,
                // StartDate = Feeder.StartDate
            };

            DBContext.Feeders.Add(entity);
            await DBContext.SaveChangesAsync();
            // var status = HttpStatusCode.Created + HttpRequestHeader
            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateFeeder")]
        public async Task<HttpStatusCode> UpdateFeeder(FeederDTO Feeder)
        {
            var entity = await DBContext.Feeders.FirstOrDefaultAsync(s => s.id == Feeder.id);

                entity.id = Feeder.id;
                entity.organization = Feeder.organization;
                entity.description = Feeder.description;
                entity.branch = Feeder.branch;
                entity.zipCode = Feeder.zipCode;
                // entity.StartDate = Feeder.StartDate;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteFeeder/{Id}")]
        public async Task<HttpStatusCode> DeleteFeeder(int Id)
        {
            var entity = new Feeder()
            {
                id = Id
            };
            DBContext.Feeders.Attach(entity);
            DBContext.Feeders.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}