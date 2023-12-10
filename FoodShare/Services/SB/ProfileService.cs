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
        Task<List<Profile>> GetFeeders();

        Task<Profile> GetProfileById(string id);
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

        public async Task<List<Profile>> GetFeeders()
        {
            var response = await _httpClient.GetAsync($"api/Profile/GetFeeders");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<Profile> feeders  = System.Text.Json.JsonSerializer.Deserialize<List<Profile>>(content, _options);
            Console.WriteLine(feeders.Count);
            return feeders;

        }

        public async Task<Profile> GetProfileById(string id)
        {
            var response = await _httpClient.GetAsync($"api/Profile/get/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            Profile profile  = System.Text.Json.JsonSerializer.Deserialize<Profile>(content, _options);

            return profile;
        }

    }
}