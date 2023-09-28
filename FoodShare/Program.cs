// PG Auth + RBAuth
global using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FoodShare.Services;
using System.Net.Security;
using Foodshare.Handlers;
using Blazored.LocalStorage;
using FoodShare.Handlers;


namespace FoodShare
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
               .AddScoped<IShoppingListService, ShoppingListService>()
               .AddScoped<IFeederService, FeederService>()
               .AddScoped<IDonorService, DonorService>()
               .AddScoped<IUserService, UserService>()
               .AddScoped<IPasswordService, PasswordService>()
               .AddScoped<CurrentUserService>()
               .AddScoped<JwtTokenHandler>();
            
            // PG Auth + RBAuth
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddTransient<CustomAuthorizationHandler>();
            
            //Adds local storage so that the current logged in user can be stored
            builder.Services.AddBlazoredLocalStorage();
            
            // Add authorization policies
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("FeederPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Feeder"); 

                });
                options.AddPolicy("DonorPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Donor"); 

                });
            });
           
            //add API    
            builder.Services.AddScoped(x => {
               var apiUrl = new Uri(builder.Configuration["apiUrl"]);
                return new HttpClient() { BaseAddress = apiUrl };
           });

            //IGNORE SSL ERRORS FROM A SPECIFIC DOMAIN
            builder.Services
            .AddHttpClient("myclient", client => client.BaseAddress = new Uri("apiUrl"))
            .ConfigurePrimaryHttpMessageHandler(handler =>
            {
                var httpClientHandler = new HttpClientHandler();
                if (handler is HttpClientHandler baseHandler)
                {
                    httpClientHandler = baseHandler;
                }
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    if (message.RequestUri.Host == "https://127.0.0.1:5076")
                    {
                        return true;
                    }
                    return errors == SslPolicyErrors.None;
                };
                return httpClientHandler;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                            policy =>
                            {
                                policy.WithOrigins("https://127.0.0.1:5706")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .SetIsOriginAllowed(origin => true)
                                .AllowCredentials();
                            });
            });

            await builder.Build().RunAsync();
        }
    }
}