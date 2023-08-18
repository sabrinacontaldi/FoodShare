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
    public interface IDonorService
    {
        List<Donor> Donors {get;set;}
        Task InsertItemAsync(NewDonor donor);
        Task<List<Donor>> GetDonors();
        Task<Donor> GetById(int id);
        Task<int> Register(NewDonor donor);
        // Task Register(NewFeeder feeder);
    }
    public class DonorService : IDonorService
    {
        private readonly string URLbase = "https://localhost:5076/api/Donor";
        private readonly HttpClient _httpClient;

        public List<Donor> Donors { get; set; } = new List<Donor>();

        private readonly JsonSerializerOptions _options;

        public DonorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        
        public async Task<int> Register(NewDonor donor)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Donor/NewDonor", donor);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            int id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            return id;
        }

        public async Task InsertItemAsync(NewDonor donor)
        {
            string URL = URLbase + "/InsertDonor";

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(URL, content);
                string apiResponse = await response.Content.ReadAsStringAsync();       
            }
        }

        public async Task<List<Donor>> GetDonors()
        {
            var response = await _httpClient.GetAsync($"api/Donor/GetDonors");
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var donors = System.Text.Json.JsonSerializer.Deserialize<List<Donor>>(content, _options);
            return donors;
        }

        public async Task<Donor> GetById(int id)
        {
            // return await _httpService.Get<User>($"/users/{id}");
            var response = await _httpClient.GetAsync($"api/Donor/GetDonorById/{id}");
            // var content = response.Content.ReadAsStringAsync();
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            Donor donor = System.Text.Json.JsonSerializer.Deserialize<Donor>(content, _options);
            return donor;
        }
    }
}
