using MySql.EntityFrameworkCore.Extensions;
using FoodShareApi.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins("https://127.0.0.1:5076")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
});

builder.Services.AddDbContext<FoodshareContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton(sp =>
{
    var httpClientHandler = new HttpClientHandler();
    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
    {
        if (message.RequestUri.Host == "http://localhost:5209")
        {
            return true;
        }
        return errors == SslPolicyErrors.None;
    };
    return new HttpClient(httpClientHandler);
});

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


// Add authentication and authorization middleware here
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//There has to be a better way to do this, but this works for now
app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    // .WithOrigins("https://localhost:5209") // Allow only this origin can also have multiple origins separated with comma
                    .AllowCredentials()); // allow credentials

app.Run();

