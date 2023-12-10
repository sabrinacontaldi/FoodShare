using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FoodShare.Models;
using FoodShare.Models.Account;

namespace FoodShare.Services
{
    public interface ISBUserService
    {
        Task<string> Register(NewSBUser user);
        Task<string> Login(SBLogin user);
    }
    public class SBUserService : ISBUserService
    {
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options;

        public SBUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<string> Register(NewSBUser user)
        {
            
            var response = await _httpClient.PostAsJsonAsync($"api/SBAuth/register", user);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }         
            // user id
            return content;
        }

        public async Task<string> Login(SBLogin user)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/SBAuth/login", user);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            // var token = System.Text.Json.JsonSerializer.Deserialize<string>(content, _options);

            return content;
        }

    }
}