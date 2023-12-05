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
        token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
        Console.WriteLine("This is the token being taken from local storage: " + token);
        return token;
    }
    // possibly parse token as a parameter
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //Random token so that the rest of the authentication can be completed before looking at JWT
        // token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiVG9ueSBTdGFyayIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6Iklyb24gTWFuIiwiZXhwIjozMTY4NTQwMDAwfQ.IbVQa1lNYYOzwso69xYfsMOHnQfO3VLvVqV2SOXS7sTtyyZ8DEf5jmmwz2FGLJJvZnQKZuieHnmHkg7CGkDbvA";
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