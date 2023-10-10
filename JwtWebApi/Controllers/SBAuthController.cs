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

namespace JwtWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SBAuthController : ControllerBase
    {
        public static User currUser = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

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
            var session = await _client.Auth.SignIn(user.Email, user.Password);

            string token = session.AccessToken;
            return Ok(token);
        }

        // [HttpPost("login")]
        // public async Task<ActionResult<string>> Login(UserDTO request)
        // {
            // DONE BY SUPABASE
            // if(currUser.Username != request.Username)
            // {
            //     return BadRequest("User not found.");
            // }

            // if(!VerifyPasswordHash(request.Password, currUser.PasswordHash, currUser.PasswordSalt))
            // {
            //     return BadRequest("Wrong Password.");
            // }

            // THIS SECTION MIGHT BE NECESSARY
        //     string token = CreateToken(currUser);

        //     // refresh token
        //     var refreshToken = GenerateRefreshToken();
        //     SetRefreshToken(refreshToken);

        //     return Ok(token);
        // }

        // I think this would be used if token is stored in database? Should it be?
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(!currUser.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if(currUser.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(currUser);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
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

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
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

       
    }
}