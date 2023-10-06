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

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(SBUserDTO user)
        {
            var session = await _client.Auth.SignUp(user.Email, user.Password);
            return session.AccessToken;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(SBUserDTO user)
        {
            var session = await _client.Auth.SignIn(user.Email, user.Password);
            return session.AccessToken;
        }
    }
}