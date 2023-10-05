using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using FoodShare.DatabaseObjects;
using FoodShare.Models;
using FoodShare.Models.Account;

namespace FoodShare.Services
{
    public interface IUserService
    {
        Task<int> Register(NewUser User);
        Task<int> GetID(string Username);

        Task<int> GetIDbyU(string Username);
        Task<User> GetUserByUsername(string username);

        Task<string> Login(Login login);
    }
    public class UserService : IUserService
    {
        // private readonly string URLbase = "https://localhost:5076/api/Feeder";
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<int> Register(NewUser user)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/SBAuth/Register", user);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            
            return id;
        }

        public async Task<string> Login(Login login)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/User/Login", login);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            // var token = System.Text.Json.JsonSerializer.Deserialize<string>(content, _options);
            
            return content;
        }

        public async Task<int> GetID(string Username)
        {
            var response = await _httpClient.GetAsync($"api/User/GetIdByUsername/{Username}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            
            return id;
        }

        public async Task<int> GetIDbyU(string Username)
        {
            var response = await _httpClient.GetAsync($"api/User/GetIdByUsername/{Username}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            int id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            
            return id;
        }

         public async Task<User> GetUserByUsername(string username)
        {
            var response = await _httpClient.GetAsync($"api/User/GetUserByUsername/{username}");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var u = System.Text.Json.JsonSerializer.Deserialize<User>(content, _options);
            
            return u;
        }

        

    }
}