// Patrick God Tutorial -> Authentication and Role Based


using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.JSInterop;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;

    public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    private string? token;

    public async Task<string> GetJwtAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
    }
    // possibly parse token as a parameter
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //Random token so that the rest of the authentication can be completed before looking at JWT
        // token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVG9ueSBTdGFyayIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Iklyb24gTWFuIiwiZXhwIjozMTY4NTQwMDAwfQ.IbVQa1lNYYOzwso69xYfsMOHnQfO3VLvVqV2SOXS7sTtyyZ8DEf5jmmwz2FGLJJvZnQKZuieHnmHkg7CGkDbvA";
        // token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
        token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
        // empty identity means User is not authorized
        // var identity = new ClaimsIdentity();

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        // with the identity we create a new user with a ClaimsPrincipal
        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        //components will get a notification that the authentication state has changed
        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    // Code written by Steve Sanders
    // Methods needed to parse the jwt and gets the claims at key value pairs
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    // End of code by Steve Sanders
}








// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net.Http.Json;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Blazored.LocalStorage;
// using FoodShare.Models;
// using Microsoft.AspNetCore.Components.Authorization;

// public class CustomAuthenticationStateProvider : AuthenticationStateProvider
// {
//     private readonly HttpClient _httpClient;

//     private readonly ILocalStorageService _localStorageService;

//     public CustomAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
//     {
//         _httpClient = httpClient;
//         _localStorageService = localStorageService;
//     }

//     public async override Task<AuthenticationState> GetAuthenticationStateAsync()
//     {
//         User currentUser = await GetUSerByJWTAsync(); //_httpClient.GetFromJsonAsync<User>("user/getcurrentuser");

//         if(currentUser != null && currentUser.username != null)
//         {
//             var claimsUsername = new Claim(ClaimTypes.Name, currentUser.username);
//             //might want to change to be the feeder/donor id
//             var claimsNameIdentifier = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(currentUser.Id));
            
//             //assign roles to the user
//             string role = "";
//             if(currentUser.feeder_id != null)
//                 role = "Feeder";
//             else if(currentUser.donor_id != null)
//                 role = "Donor";
            
//             var claimsRole = new Claim(ClaimTypes.Role, role);

//             var claimsIdentity = new ClaimsIdentity(new[] {claimsUsername, claimsNameIdentifier});
//             var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

//             return new AuthenticationState(claimsPrincipal);
//         }
//         else
//             return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//     }

//     public async Task<User> GetUSerByJWTAsync()
//     {
//         //pulling the token from localStorage
//         var jwtToken = await _localStorageService.GetItemAsStringAsync("jwt_token");
//         if(jwtToken == null) return null;

//         //preparing the http request
//         var requestMessage = new HttpRequestMessage(HttpMethod.Post, "user/getuserbyjwt");
//         requestMessage.Content = new StringContent(jwtToken);

//         requestMessage.Content.Headers.ContentType
//             = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

//         //making the http request
//         var response = await _httpClient.SendAsync(requestMessage);

//         var responsesStatusCode = response.StatusCode;
//         var returnedUser = await response.Content.ReadFromJsonAsync<User>();

//         //returning the user if found
//         if(returnedUser != null) return await Task.FromResult(returnedUser);
//         else return null;
//     }
// }
