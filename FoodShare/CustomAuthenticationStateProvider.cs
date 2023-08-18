using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using FoodShare.Models;
using Microsoft.AspNetCore.Components.Authorization;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;

    private readonly ILocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        User currentUser = await GetUSerByJWTAsync(); //_httpClient.GetFromJsonAsync<User>("user/getcurrentuser");

        if(currentUser != null && currentUser.username != null)
        {
            var claimsUsername = new Claim(ClaimTypes.Name, currentUser.username);
            //might want to change to be the feeder/donor id
            var claimsNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Id));
            
            //assign roles to the user
            string role = "";
            if(currentUser.feeder_id != null)
                role = "Feeder";
            else if(currentUser.donor_id != null)
                role = "Donor";
            
            var claimsRole = new Claim(ClaimTypes.Role, role);

            var claimsIdentity = new ClaimsIdentity(new[] {claimsUsername, claimsNameIdentifier});
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new AuthenticationState(claimsPrincipal);
        }
        else
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<User> GetUSerByJWTAsync()
    {
        //pulling the token from localStorage
        var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
        if(jwtToken == null) return null;

        //preparing the http request
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "user/getuserbyjwt");
        requestMessage.Content = new StringContent(jwtToken);

        requestMessage.Content.Headers.ContentType
            = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        //making the http request
        var response = await _httpClient.SendAsync(requestMessage);

        var responsesStatusCode = response.StatusCode;
        var returnedUser = await response.Content.ReadFromJsonAsync<User>();

        //returning the user if found
        if(returnedUser != null) return await Task.FromResult(returnedUser);
        else return null;
    }
}
