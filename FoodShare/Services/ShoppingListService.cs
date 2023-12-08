using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

using FoodShare.Models;
using FoodShare.Models.Account;
using FoodShare.DatabaseObjects;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;

namespace FoodShare.Services
{
    //Interface that connects to the donation request table in the MySQL database through the FoodShareApi
    public interface IShoppingListService
    {
        // Task Register(AddItem item);
        Task<string> Register(DonationRequest donation);
        // Task<int> InsertItem(AddItem item);
        Task<int> InsertItem(DonationRequest donation);
        Task<List<DonationRequest>> GetItemsByFeeder(string id);
        // Task<List<DonationRequest>> GetAllItems();
        // Task<List<DonationRequest>> GetItemsNewToOld();
        Task DeleteItem(int id);
        Task DeleteItems(List<int> ids);
        // Task UpdateDonorId(DonationRequest d);
        // Task UpdateDonorIds(List<DonationRequest> donations);
    }

    public class ShoppingListService : IShoppingListService
    {   
        private readonly HttpClient _httpClient;

        public List<DonationRequest> Items {get; set;} = new List<DonationRequest>();

        private readonly JsonSerializerOptions _options;

        public ShoppingListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        
         public async Task<string> Register(DonationRequest item)
        { 

            // await _httpClient.PostAsJsonAsync($"api/DonationRequest/new", item);
            var response = await _httpClient.PostAsJsonAsync($"api/DonationRequest/new", item);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            
            return content;
            // var response = await _httpClient.PostAsJsonAsync($"api/Profile/register", profile);
            // var content = await response.Content.ReadAsStringAsync();
            // if (!response.IsSuccessStatusCode)
            // {
            //     throw new ApplicationException(content);
            // }
            
            // return content;
    
        }

        //Add a donation to the donation request table in the database
        //return the auto-incremented id of the donation request
         public async Task<int> InsertItem(DonationRequest donation)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/DonationRequest/new", donation);

            // var response = await _httpClient.PostAsJsonAsync($"api/Item/InsertItemR", donation);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            var id = System.Text.Json.JsonSerializer.Deserialize<int>(content, _options);
            
            return id;
        }

        //Get a list of donations that are linked to a specific feeder id
        public async Task<List<DonationRequest>> GetItemsByFeeder(string id)
        {
            // return await _httpService.Get<User>($"/users/{id}");
            var response = await _httpClient.GetAsync($"api/DonationRequest/GetItemsByFeeder/{id}");
            // var content = response.Content.ReadAsStringAsync();
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            List<DonationRequest> items = System.Text.Json.JsonSerializer.Deserialize<List<DonationRequest>>(content, _options);
            Console.WriteLine(items.Count);
            return items;

        }
        
        //FILTER METHODS
        //Get a list of all the donation requests ordered from old to new
        // public async Task<List<DonationRequest>> GetAllItems()
        // {
        //     var response = await _httpClient.GetAsync($"api/Item/GetItems");
  
        //     var content = await response.Content.ReadAsStringAsync();
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         throw new ApplicationException(content);
        //     }
        //     List<DonationRequest> items = System.Text.Json.JsonSerializer.Deserialize<List<DonationRequest>>(content, _options);

        //     return items;
        // }

        //Get a list of all donation requests ordered from new to old
        // public async Task<List<DonationRequest>> GetItemsNewToOld()
        // {
        //     var response = await _httpClient.GetAsync($"api/Item/GetItemsNewToOld");
        //     var content = await response.Content.ReadAsStringAsync();
        //     if (!response.IsSuccessStatusCode)
        //     {
        //         throw new ApplicationException(content);
        //     }
        //     List<DonationRequest> items = System.Text.Json.JsonSerializer.Deserialize<List<DonationRequest>>(content, _options);

        //     return items;
        // }

        //Delete donation request
        public async Task DeleteItem(int id)
        {
            await _httpClient.DeleteAsync($"api/DonationRequest/delete/{id}");
        }

         public async Task DeleteItems(List<int> ids)
        {
            foreach(int id in ids)
            {
                 await _httpClient.DeleteAsync($"api/DonationRequest/delete/{id}");
            }
        }

        // public async Task UpdateDonorId(DonationRequest d)
        // {
        //     await _httpClient.PutAsJsonAsync($"api/Item/UpdateDonorId", d);
        // }

        // public async Task UpdateDonorIds(List<DonationRequest> donations)
        // {
        //     foreach(DonationRequest donation in donations)
        //         await _httpClient.PutAsJsonAsync($"api/DonationRequest/UpdateDonorId", donation);
        // }
    }
}