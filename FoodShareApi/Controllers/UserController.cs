using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;
using FoodShareApi.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace FoodShareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly FoodshareContext DBContext;
        private readonly IConfiguration _configuration;
        private static readonly string SALT = "$2a$10$elWzkojiPOfIgB30YVleNO";

        
        public UserController(FoodshareContext DBContext, IConfiguration configuration)
        {
            this.DBContext = DBContext;
            _configuration = configuration;
        }

        //login method
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDTO login)
        {
            User User = await DBContext.Users.Select(
                    s => new User
                    {
                        id = s.id,
                        contact_person = s.contact_person,
                        username = s.username,
                        email_address = s.email_address,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    })
                .FirstOrDefaultAsync(s => s.username == login.username);

            if (User == null)
            {
                return BadRequest("Incorrect username.");
            }

            Password Password = await DBContext.Passwords.Select(
                    s => new Password
                    {
                        id = s.id,
                        password = s.password
                    })
                .FirstOrDefaultAsync(s => s.id == User.id);
            
            string check = BCrypt.Net.BCrypt.HashPassword(login.password, SALT);

            if(check != Password.password)
            {
                return BadRequest("Incorrect password.");
            }
            
            string token = CreateToken(User);
            return token;
        }

        //Method to create a jwt token for login
        private string CreateToken(User user)
        {
            string role = null;
            int? id = null;
            
            if(user.feeder_id != null)
            {
                role = "Feeder";
                id = user.feeder_id;
            }
            else if(user.donor_id != null)
            {
                role = "Donor";
                id = user.donor_id;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),

                //The user authorization role (Feeder/Donor)
                new Claim(ClaimTypes.Role, role),

                //This is the feeder/donor id
                new Claim(ClaimTypes.NameIdentifier, id.ToString())
            };


            //When using this it says that the key needs to be atleast 512 bytes but its 136 bytes
            // var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            //     _configuration.GetSection("AppSettings:Token").Value));

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                "my super super great top secret keymy super super great top secret keymy super super great top secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        
        //CreateNewAccount
        // [HttpPost("CreateNewAccount")]
        // public async Task<ActionResult> CreateNewAccount(NewAccountDTO newAcc)
        // {
            
        //     var entity = new User()
        //     {
        //         // id = User.id,
        //         contact_person = User.contact_person,
        //         username = User.username,
        //         email_address = User.email_address,
        //         feeder_id = User.feeder_id,
        //         donor_id = User.donor_id
        //     };

        //     DBContext.Users.Add(entity);
        //     await DBContext.SaveChangesAsync();

        //     return HttpStatusCode.Created;
        // }
        //  newF.Organization = model.Organization;
        //         newF.Description = model.Description;
        //         newF.Branch = model.Branch;
        //         newF.ZipCode = model.ZipCode;

        //         //new user
        //         //id of new feeder is set to the user's feeder_id
        //         newUser.feeder_id = await FeederService.Register(newF);

        //         newUser.contact_person = model.Name;
        //         newUser.email_address = model.EmailAddress;
        //         newUser.username = model.Username;

        //         //new password
        //         //id of new user is set to the password's id
        //         newPassword.Id = await UserService.Register(newUser);

        //         newPassword.Password = model.Password;
                
        //         await PasswordService.Register(newPassword);
                
        //         loading = false;
                
        //         NavigationManager.NavigateTo("/account/login");


        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            var List = await DBContext.Users.Select(
                s => new UserDTO
                {
                    id = s.id,
                    contact_person = s.contact_person,
                    username = s.username,
                    email_address = s.email_address,
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

        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int Id)
        {
            UserDTO User = await DBContext.Users.Select(
                    s => new UserDTO
                    {
                        id = s.id,
                        contact_person = s.contact_person,
                        username = s.username,
                        email_address = s.email_address,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    })
                .FirstOrDefaultAsync(s => s.id == Id);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }

        [HttpGet("GetIdByUsername/{Username}")]
        public async Task<ActionResult<int>> GetIdByUsername(string Username)
        {
            UserDTO User = await DBContext.Users.Select(
                    s => new UserDTO
                    {
                        id = s.id,
                        contact_person = s.contact_person,
                        username = s.username,
                        email_address = s.email_address,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    })
                .FirstOrDefaultAsync(s => s.username == Username);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User.id;
            }
        }

        [HttpGet("GetUserByUsername/{Username}")]
        public async Task<ActionResult<UserDTO>> GetUserByUsername(string Username)
        {
            UserDTO User = await DBContext.Users.Select(
                    s => new UserDTO
                    {
                        id = s.id,
                        contact_person = s.contact_person,
                        username = s.username,
                        email_address = s.email_address,
                        feeder_id = s.feeder_id,
                        donor_id = s.donor_id
                    })
                .FirstOrDefaultAsync(s => s.username == Username);

            if (User == null)
            {
                return NotFound();
            }
            else
            {
                return User;
            }
        }
        // [HttpPost("InsertUser")]
        // public async Task<HttpStatusCode> InsertUser(UserDTO User)
        // {
        //     var entity = new User()
        //     {
        //         // id = User.id,
        //         contact_person = User.contact_person,
        //         username = User.username,
        //         email_address = User.email_address,
        //         feeder_id = User.feeder_id,
        //         donor_id = User.donor_id
        //     };

        //     DBContext.Users.Add(entity);
        //     await DBContext.SaveChangesAsync();

        //     return HttpStatusCode.Created;
        // }

        // [HttpPost("InsertUser")]
        // public async Task<HttpStatusCode> InsertUser(UserDTO User)
        // {
        //     var entity = new User()
        //     {
        //         // id = User.id,
        //         contact_person = User.contact_person,
        //         username = User.username,
        //         email_address = User.email_address,
        //         feeder_id = User.feeder_id,
        //         donor_id = User.donor_id
        //     };

        //     DBContext.Users.Add(entity);
        //     await DBContext.SaveChangesAsync();

        //     return HttpStatusCode.Created;
        // }


        [HttpPost("InsertUser")]
        public async Task<ActionResult<int>> InsertUser(UserDTO User)
        {
            var entity = new User()
            {
                // id = User.id,
                contact_person = User.contact_person,
                username = User.username,
                email_address = User.email_address,
                feeder_id = User.feeder_id,
                donor_id = User.donor_id
            };

            DBContext.Users.Add(entity);
            await DBContext.SaveChangesAsync();

            return entity.id;
        }
        // if(User.feeder_id != null)
            //     await FoodshareContext.AddToRoleAsync(User, "Feeder");
             
            // else if(User.donor_id != null)
            //     await _userManager.AddToRoleAsync(newUser, "Donor");

        [HttpPut("UpdateUser")]
        public async Task<HttpStatusCode> UpdateUser(UserDTO User)
        {
            var entity = await DBContext.Users.FirstOrDefaultAsync(s => s.id == User.id);

                entity.contact_person = User.contact_person;
                entity.username = User.username;
                entity.email_address = User.email_address;
                entity.feeder_id = User.feeder_id;
                entity.donor_id = User.donor_id;

            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteUser/{Id}")]
        public async Task<HttpStatusCode> DeleteUser(int Id)
        {
            var entity = new User()
            {
                id = Id
            };
            DBContext.Users.Attach(entity);
            DBContext.Users.Remove(entity);
            await DBContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

    }
}