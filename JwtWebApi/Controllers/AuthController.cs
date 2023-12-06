// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Security.Cryptography;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using System.Security.Cryptography;
// using Microsoft.IdentityModel.Tokens;
// using System.Security.Claims;
// using System.IdentityModel.Tokens.Jwt;
// using Microsoft.AspNetCore.Authorization;
// using Supabase.Gotrue;

// namespace JwtWebApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class AuthController : ControllerBase
//     {
//         public static User currUser = new User();
//         private readonly IConfiguration _configuration;
//         private readonly IUserService _userService;

//         private readonly Supabase.Client _client;

//         public AuthController(IConfiguration configuration)
//         {
//             _configuration = configuration;
//         }

//         [HttpGet, Authorize]
//         public ActionResult<string> GetMe()
//         {
//             var userName = _userService.GetMyName();
//             return Ok(userName);
//         }

//         [HttpPost("register")]
//         public async Task<ActionResult<string>> Register(UserDTO request)
//         {
//             CreatePasswordHash(request.Password, out byte[] hashed_password, out byte[]password_salt);

//             var user = new User
//             {
//                 Username = request.Username,
//                 PasswordHash = hashed_password,
//                 PasswordSalt = password_salt
//             };

//             var response = await _client.From<User>().Insert(user);

//             var newUser = response.Models.First();

//             return Ok(newUser.Id);
//         }

//         [HttpPost("login")]
//         public async Task<ActionResult<string>> Login(UserDTO request)
//         {
//             if(currUser.Username != request.Username)
//             {
//                 return BadRequest("User not found.");
//             }

//             if(!VerifyPasswordHash(request.Password, currUser.PasswordHash, currUser.PasswordSalt))
//             {
//                 return BadRequest("Wrong Password.");
//             }

//             string token = CreateToken(currUser);

//             // refresh token
//             var refreshToken = GenerateRefreshToken();
//             SetRefreshToken(refreshToken);

//             return Ok(token);
//         }

//         [HttpPost("refresh-token")]
//         public async Task<ActionResult<string>> RefreshToken()
//         {
//             var refreshToken = Request.Cookies["refreshToken"];

//             if(!currUser.RefreshToken.Equals(refreshToken))
//             {
//                 return Unauthorized("Invalid Refresh Token.");
//             }
//             else if(currUser.TokenExpires < DateTime.Now)
//             {
//                 return Unauthorized("Token expired.");
//             }

//             string token = CreateToken(currUser);
//             var newRefreshToken = GenerateRefreshToken();
//             SetRefreshToken(newRefreshToken);

//             return Ok(token);
//         }

//         private RefreshToken GenerateRefreshToken()
//         {
//             var refreshToken = new RefreshToken
//             {
//                 Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
//                 Expires = DateTime.Now.AddDays(7),
//                 Created = DateTime.Now
//             };

//             return refreshToken;
//         }

//         private void SetRefreshToken(RefreshToken newRefreshToken)
//         {
//             var cookieOptions = new CookieOptions
//             {
//                 HttpOnly = true,
//                 Expires = newRefreshToken.Expires
//             };
//             Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

//             currUser.RefreshToken = newRefreshToken.Token;
//             currUser.TokenCreated = newRefreshToken.Created;
//             currUser.TokenExpires = newRefreshToken.Expires;

//         }

//         private string CreateToken(User user)
//         {
//             List<Claim> claims = new List<Claim>
//             {
//                 new Claim(ClaimTypes.Name, user.Username)
//             };

//             var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
//                 _configuration.GetSection("AppSettings:Token").Value));
            
//             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            
//             var token = new JwtSecurityToken(
//                 claims: claims,
//                 // Token valid for one day
//                 expires: DateTime.Now.AddDays(1),
//                 signingCredentials: creds);

//             var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            
//             return jwt;
//         }

//         private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
//         {
//             using(var hmac = new HMACSHA512())
//             {
//                 passwordSalt = hmac.Key;
//                 passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//             }
//         }

//         private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
//         {
//             using(var hmac = new HMACSHA512(passwordSalt))
//             {
//                 // hash the password that the user entered using the salt associated with the user
//                 var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
//                 // compare new hashed password with the hashed password saved in the database
//                 return computedHash.SequenceEqual(passwordHash);
//             }
//         }
//     }
// }