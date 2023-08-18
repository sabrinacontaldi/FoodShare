using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Cors;

namespace FoodShareApi.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly FoodshareContext DBContext;

        public DonorController( FoodshareContext DBContext)
        {
            this.DBContext = DBContext;
        }
        
        [HttpGet("GetDonors")]
        public async Task<ActionResult<IEnumerable<DonorDTO>>> GetDonor()
        {
            // return await DBContext.Feeders.ToListAsync();
            var List = await DBContext.Donors.Select(
                s => new DonorDTO
                {
                    id = s.id,
                    organization= s.organization,
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

        [HttpGet("GetDonorById/{id:int}")]
        public async Task<ActionResult<DonorDTO>> GetDonorById(int id)
        {
            DonorDTO donor = await DBContext.Donors
                .Where(d => d.id == id)
                .Select(d => new DonorDTO
                {
                    id = d.id,
                    organization = d.organization,
                    branch = d.branch,
                    zipCode = d.zipCode,
                })
                .FirstOrDefaultAsync();

            if (donor == null)
            {
                return NotFound();
            }

            return donor;
        }

        [HttpPost("NewDonor")]
        public async Task<ActionResult<int>> NewDonor(DonorDTO Donor)
        {
            Donor d = new Donor()
            {
                organization = Donor.organization,
                branch = Donor.branch,
                zipCode = Donor.zipCode,
            };
            
            DBContext.Donors.Add(d);
            await DBContext.SaveChangesAsync();

            return d.id;
        }
       

        [HttpPut("UpdateDonor")]
        public async Task<HttpStatusCode> UpdateDonor(DonorDTO Donor)
        {
            var entity = await DBContext.Donors.FirstOrDefaultAsync(s => s.id == Donor.id);

                entity.id = Donor.id;
                entity.organization = Donor.organization;
                entity.branch = Donor.branch;
                entity.zipCode = Donor.zipCode;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteDonor/{Id}")]
        public async Task<HttpStatusCode> DeleteFeeder(int Id)
        {
            var entity = new Donor()
            {
                id = Id
            };
            DBContext.Donors.Attach(entity);
            DBContext.Donors.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}