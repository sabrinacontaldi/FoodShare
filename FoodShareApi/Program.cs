using MySql.EntityFrameworkCore.Extensions;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Security;
// Auth0 authentication + previous authentication
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Previous code
builder.Services.AddAuthentication(options => 
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8
            // .GetBytes(builder.Configuration.GetSection("AppSettings: Token").Value))
                .GetBytes("my super super great top secret keymy super super great top secret keymy super super great top secret key")),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
        
    });

// Auth0 Authentication code
// help secure the webApi
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
//     {
//         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
//         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//         {
//             ValidAudience = builder.Configuration["Auth0:Audience"],
//             ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}"
//         };
//     });

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy(name: MyAllowSpecificOrigins,
//                 policy =>
//                 {
//                     policy.WithOrigins("https://127.0.0.1:5209")
//                     .AllowAnyHeader()
//                     .AllowAnyMethod();
//                 });
// });
builder.Services.AddCors();

builder.Services.AddDbContext<FoodshareContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Might need to uncomment
// builder.Services.AddSingleton(sp =>
// {
//     var httpClientHandler = new HttpClientHandler();
//     httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
//     {
//         if (message.RequestUri.Host == "http://localhost:5209")
//         {
//             return true;
//         }
//         return errors == SslPolicyErrors.None;
//     };
//     return new HttpClient(httpClientHandler);
// });

// await builder.Build().RunAsync();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
//added line
app.UseRouting();

// Auth0 and previous authentication
// Add authentication and authorization middleware here
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Auth0
// not sure if this line is needed
// Broke the program
// app.MapRazorPages();

//There has to be a better way to do this, but this works for now
app.UseCors(x => 
{
   x.WithOrigins("https://localhost:5209/");
});
                    // .AllowAnyMethod()
                    // .AllowAnyHeader()
                    // .SetIsOriginAllowed(origin => true) // allow any origin
                    // .WithOrigins("https://localhost:5209") // Allow only this origin can also have multiple origins separated with comma
                    // .AllowCredentials() // allow credentials


app.Run();

