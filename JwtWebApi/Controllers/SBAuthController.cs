using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Supabase.Gotrue;
using JwtWebApi.Models;
using JwtWebApi.Contracts.Profile;
using System.Text;

namespace JwtWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SBAuthController : ControllerBase
    {
        public static User currUser = new User();
        private readonly IConfiguration _configuration;
        private readonly Supabase.Client _client;

        public SBAuthController(Supabase.Client client, IConfiguration configuration)
        {
            this._client = client;
            _configuration = configuration;
        }

        // Not sure if this is valid for username
        // [HttpGet, Authorize]
        // public ActionResult<string> GetMe()
        // {
        //     var userName = _userService.GetMyName();
        //     return Ok(userName);
        // }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(SBUserDTO user)
        {
            var response = await _client.Auth.SignUp(user.Email, user.Password);
            
            // returns the user id of the newly registered user
            return response.User.Id;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(SBUserDTO user)
        {
            // The user logins in 
            // when a successful login message is returned token is created
            // user id grabs user info form the porfile table
            // token created using:
                // user id + user/org name + role + zip?
            
            // Login for supabase auth only 
            // var session = await _client.Auth.SignIn(user.Email, user.Password);
            // string token = session.AccessToken;
            
            // return Ok(token);

            var session = await _client.Auth.SignIn(user.Email, user.Password);

            if (session != null)
            {
                // Retrieve user information from the profile table
                var profileResponse = await GetProfileByUserId(session.User.Id.ToString());

                if (profileResponse != null)
                {
                    // Create a custom JWT with user information
                    // var token = GenerateCustomToken(profileResponse);
                    var token = CreateToken(profileResponse);

                    return Ok(token);
                }
            }

            return Unauthorized("Login failed");
        }
        
        // Get the Profile information using the user id
        private async Task<ProfileResponse> GetProfileByUserId(string userId)
        {
            var response = await _client
                .From<Profile>()
                .Where(n => n.Id == userId)
                .Get();
            
            var profile = response.Models.FirstOrDefault();

            // This shouldn't be possible
            // if (profile is null)
            // {
            //     return NotFound();
            // }

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

            return profileResponse;
        }
        
        
        // Creates a JWT token with Name and Role
        private string CreateToken(ProfileResponse profile)
        {
            List<Claim> claims = new List<Claim>
            {
                // Add id?
                new Claim(ClaimTypes.Name, profile.Name),
                new Claim(ClaimTypes.Role, profile.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));
            
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
            var token = new JwtSecurityToken(
                claims: claims,
                // Token valid for one day
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            currUser.RefreshToken = newRefreshToken.Token;
            currUser.TokenCreated = newRefreshToken.Created;
            currUser.TokenExpires = newRefreshToken.Expires;

        }

      // Old method for creating token that didnt work
        // private string GenerateCustomToken(ProfileResponse profile)
        // {
        //     // You can use the System.IdentityModel.Tokens.Jwt library to create a JWT.
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value); // Replace with your secret key
        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(new[]
        //         {
        //             new Claim(ClaimTypes.Name, profile.Name), // Customize this based on your needs
        //             new Claim(ClaimTypes.Role, profile.Role),
        //             // Add more claims as needed
        //         }),
        //         Expires = DateTime.UtcNow.AddHours(1), // Set token expiration time
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        //     };
        //     var token = tokenHandler.CreateToken(tokenDescriptor);

        //     return tokenHandler.WriteToken(token);
        // }  
       
    }
}