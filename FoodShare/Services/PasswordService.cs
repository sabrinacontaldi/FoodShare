using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FoodShare.Models.Account;
using Newtonsoft.Json;

namespace FoodShare.Services
{
    public interface IPasswordService
    {
        Task Register(AddPassword password);
        Task<bool> CheckPassword(AddPassword password);
    }
    public class PasswordService : IPasswordService
    {
        // private readonly string URLbase = "https://localhost:5076/api/Feeder";
        private readonly HttpClient _httpClient;

        private readonly JsonSerializerOptions _options;

        public PasswordService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task Register(AddPassword password)
        {
            await _httpClient.PostAsJsonAsync($"api/Password/InsertPassword", password);
        }

        //not working always
        // public async Task<bool> CheckPassword(AddPassword password)
        // {
        //     var json = JsonConvert.SerializeObject(password);
        //     var content = new StringContent(json, Encoding.UTF8, "application/json");

        //     var response = await _httpClient.PostAsync($"api/Password/CheckPassword", content);

        //     // var response = await _httpClient.GetAsync($"api/Password/CheckPassword");
        //     var result = await response.Content.ReadAsStringAsync();
        //     if (response.IsSuccessStatusCode)
        //     {
                
        //         return System.Text.Json.JsonSerializer.Deserialize<bool>(result, _options);
        //     }
        //     else
        //     {
        //         throw new ApplicationException(result);
        //     }
           
        // }

        public async Task<bool> CheckPassword(AddPassword password)
        {

            var response = await _httpClient.PostAsJsonAsync($"api/Password/CheckPassword", password);

            var content = await response.Content.ReadAsStringAsync();

            // if (!response.IsSuccessStatusCode)
            // {
            //     throw new ApplicationException(content);
            // }
            // var verify = System.Text.Json.JsonSerializer.Deserialize<bool>(content, _options);
            return System.Text.Json.JsonSerializer.Deserialize<bool>(content, _options);
        }

        //  public async Task<bool> CheckPassword(AddPassword password)
        // {
        //     var response = await _httpClient.GetAsync($"api/Password/CheckPassword");
        //     var content = await response.Content.ReadAsStringAsync();
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         throw new ApplicationException(content);
        //     }
        //     // var verify = System.Text.Json.JsonSerializer.Deserialize<bool>(content, _options);
        //     return System.Text.Json.JsonSerializer.Deserialize<bool>(content, _options);
        // }
    }
}