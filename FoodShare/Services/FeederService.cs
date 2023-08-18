using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using FoodShare.DatabaseObjects;
using FoodShare.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Authentication;
using System.Net.Security;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FoodShare.Services
{
    public interface IFeederService
    {
        List<Feeder> Feeders {get;set;}
        Task InsertItemAsync(NewFeeder feeder);
        Task<List<Feeder>> GetFeeders();
        Task<Feeder> GetById(int id);
        Task<int> Register(NewFeeder feeder);

        Task<List<Feeder>> GetFeedersCloseToFar();
        Task<List<Feeder>> GetFeedersMostToLeast();
        Task<List<Feeder>> GetFeedersLeastToMost();

        // Task Register(NewFeeder feeder);
    }
    public class FeederService : IFeederService
    {
        private readonly string URLbase = "https://localhost:5076/api/Feeder";
        private readonly HttpClient _httpClient;

        public List<Feeder> Feeders { get; set; } = new List<Feeder>();

        private readonly JsonSerializerOptions _options;

        public FeederService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // public async Task Register(NewFeeder feeder)
        // {
        //     await _httpClient.PostAsJsonAsync($"api/Feeder/InsertFeeder", feeder);
        // }
        public async Task<int> Register(NewFeeder feeder)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Feeder/NewFeeder", feeder);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            int id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            return id;
        }

        public async Task InsertItemAsync(NewFeeder feeder)
        {
            string URL = URLbase + "/InsertFeeder";

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(feeder), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(URL, content);
                string apiResponse = await response.Content.ReadAsStringAsync();       
            }
        }

        public async Task<List<Feeder>> GetFeeders()
        {
            var response = await _httpClient.GetAsync($"api/Feeder/GetFeeders");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var feeders = System.Text.Json.JsonSerializer.Deserialize<List<Feeder>>(content, _options);
            return feeders;
        }
        
        //FILTER OPTIONS
        //CLOSE TO FAR
            //edit so that zipcode is the parameter
        public async Task<List<Feeder>> GetFeedersCloseToFar()
        {
            var response = await _httpClient.GetAsync($"api/Feeder/GetFeedersCloseToFar");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var feeders = System.Text.Json.JsonSerializer.Deserialize<List<Feeder>>(content, _options);
            return feeders;
        }

        //MOST ITEMS TO LEAST
        public async Task<List<Feeder>> GetFeedersMostToLeast()
        {
            var response = await _httpClient.GetAsync($"api/Feeder/GetFeedersMostToLeast");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var feeders = System.Text.Json.JsonSerializer.Deserialize<List<Feeder>>(content, _options);
            return feeders;
        }
        
        //LEAST ITEMS TO MOST
        public async Task<List<Feeder>> GetFeedersLeastToMost()
        {
            var response = await _httpClient.GetAsync($"api/Feeder/GetFeedersLeastToMost");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var feeders = System.Text.Json.JsonSerializer.Deserialize<List<Feeder>>(content, _options);
            return feeders;
        }

        public async Task<Feeder> GetById(int id)
        {
            // return await _httpService.Get<User>($"/users/{id}");
            var response = await _httpClient.GetAsync($"api/Feeder/GetFeederById/{id}");
            // var content = response.Content.ReadAsStringAsync();
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            Feeder feeder = System.Text.Json.JsonSerializer.Deserialize<Feeder>(content, _options);
            return feeder;
        }
    }
}
