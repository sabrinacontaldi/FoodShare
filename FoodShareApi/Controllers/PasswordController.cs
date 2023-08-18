using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using BCrypt.Net;
using FoodShareApi.DTO;
using System.Security.Claims;

namespace FoodShareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : ControllerBase
    {
        private readonly FoodshareContext DBContext;

        private static readonly string SALT = "$2a$10$elWzkojiPOfIgB30YVleNO";
        //private static readonly int SLENGTH = SALT.Length;
        private static readonly int MAX = 45 - SALT.Length;


        public PasswordController( FoodshareContext DBContext)
        {
            this.DBContext = DBContext;
        }

        // [HttpGet("GetPasswordById")]
        // public async Task<ActionResult<PasswordDTO>> GetPasswordById(int Id)
        // {
        //     PasswordDTO Password = await DBContext.Passwords.Select(
        //             s => new PasswordDTO
        //             {
        //                 id = s.id,
        //                 password = s.password
        //             })
        //         .FirstOrDefaultAsync(s => s.id == Id);

        //     if (User == null)
        //     {
        //         return NotFound();
        //     }
        //     else
        //     {
        //         return Password;
        //     }
        // }

        [HttpPost("GetPasswordById")]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetPasswordById(int Id)
        {
            PasswordDTO Password = await DBContext.Passwords.Select(
                    s => new PasswordDTO
                    {
                        id = s.id,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.id == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return Password.password;
            }
        }

        [HttpPost("InsertPassword")]
        public async Task<HttpStatusCode> InsertPassword(PasswordDTO Password)
        {
            var entity = new Password()
            {
                id = Password.id,
                //encrypts the password
                // password = HashPassword(Password.password)
               password = BCrypt.Net.BCrypt.HashPassword(Password.password, SALT)

            };

            DBContext.Passwords.Add(entity);
            await DBContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        // [HttpGet("CheckPassword/{id:int}/{password}")]
        [HttpPost("CheckPassword")]
        // [Produces("application/json")]
        public async Task<bool> CheckPasswordCorrect(PasswordDTO p)
        {

            //gets the hashed password from the database that has the same id as the parameter p
            PasswordDTO Password = await DBContext.Passwords.Select(
                    s => new PasswordDTO
                    {
                        id = s.id,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.id == p.id);

            // string check = BCrypt.Net.BCrypt.HashPassword(p.password, SALT).Substring(0, MAX);
            string check = BCrypt.Net.BCrypt.HashPassword(p.password, SALT);

            //Checks to see if the passwords match
            // return CheckPassword(p.password, Password.password);

            if(check == Password.password)
                return true;
            else
                return false;
            // return BCrypt.Net.BCrypt.Verify(p.password, Password.password);
        }


        [HttpPost("VerifyPassword")]
        // [Produces("application/json")]
        public async Task<ActionResult<string>> CheckPassword(PasswordDTO p)
        {

            //gets the hashed password from the database that has the same id as the parameter p
            PasswordDTO Password = await DBContext.Passwords.Select(
                    s => new PasswordDTO
                    {
                        id = s.id,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.id == p.id);

            // string check = BCrypt.Net.BCrypt.HashPassword(p.password, SALT).Substring(0, MAX);
            string check = BCrypt.Net.BCrypt.HashPassword(p.password, SALT);

            //Checks to see if the passwords match
            // return CheckPassword(p.password, Password.password);

            // string token = CreateToken(p);
            if(check == Password.password)
                return BadRequest("Wrong Password");
            else
                return Ok("");
            // return BCrypt.Net.BCrypt.Verify(p.password, Password.password);
        }

        //Claims - properties describing user that is authenticated
        //can be read with a client app
        //store id
        private string CreateToken(Password p)
        {
            List<Claim> claims = new List<Claim>
            {
                // new Claim(ClaimTypes.
            };

            return string.Empty;
        }
        // public static Password password = new Password();

        // [HttpPost("register")]
        // public async Task<ActionResult<bool>> Register2(PasswordDTO p)
        // {
        //     CreatePasswordHash(p.password, out byte[] passwordHash, out byte[] passwordSalt);

        //     password.id = p.id;
        //     //need to change to be byte[]
        //     p.password = passwordHash;
        //     //need to add password salt to the database 
        //     p.passwordSalt = passwordSalt;

        //     return true;

        // }
        // [HttpPost("login")]
        // public async Task<ActionResult<string>> Login(PasswordDTO p)
        // {
            
        // }


        // private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        // {
        //     using (var hmac = new HMACSHA512())
        //     {
        //         passwordSalt = hmac.Key;
        //         passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //     }
        // }

            //if true = password is correct
        // private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        // {
        //     using (var hmac = new HMACSHA512(passwordSalt))
        //     {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        return computedHash.SequenceEqual(passwordHash);
        //     }
        // }

        
    }
}