using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FoodShare.Models.Account.Profile;

namespace FoodShare.Services
{
    public interface IProfileService
    {
        Task<string> Register(Profile user);
        // Task<string> Login(SBLogin user);
    }
    public class ProfileService : IProfileService
    {

        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options;

        public ProfileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // register user
        public async Task<string> Register(Profile profile)
        {
            
            var response = await _httpClient.PostAsJsonAsync($"api/Profile/register", profile);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            
            return content;
        }

        // public async Task<string> Login(SBLogin user)
        // {
        //     var response = await _httpClient.PostAsJsonAsync($"api/SBAuth/login", user);
        //     var content = await response.Content.ReadAsStringAsync();
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         throw new ApplicationException(content);
        //     }
        //     // var token = System.Text.Json.JsonSerializer.Deserialize<string>(content, _options);
            
        //     return content;
        // }

    }
}