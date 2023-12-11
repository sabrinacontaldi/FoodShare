// global using JwtWebApi.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using System.ComponentModel;
using JwtWebApi;
using Supabase;
using JwtWebApi.Models;
using JwtWebApi.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpContextAccessor();

// Supabase
builder.Services.AddScoped<Supabase.Client>(_ => 
    new Supabase.Client(
        builder.Configuration.GetSection("Supabase:SupabaseUrl").Value,
        builder.Configuration.GetSection("Supabase:SupabaseKey").Value,
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token\"})",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
// builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
//     policy => 
//     {
//         policy.WithOrigins("https//localhost:5209").AllowAnyMethod().AllowAnyHeader();
//     }));


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5209") // Allow requests from this origin
               .AllowAnyMethod() // Allow any HTTP method
               .AllowAnyHeader(); // Allow any HTTP headers
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors("NgOrigins");
app.UseCors("MyCorsPolicy");

// app.MapPost("/newsletters", async (
//     CreateNewsletterRequest request,
//     Supabase.Client client) =>
//     {
//         var newsletter = new Newsletter
//         {
//             Name = request.Name,
//             Description = request.Description,
//             ReadTime = request.ReadTime
//         };

//         var response = await client.From<Newsletter>().Insert(newsletter);

//         var newNewsletter = response.Models.First();

//         return Results.Ok(newNewsletter.Id);
//     });

// app.MapGet("/newsletters/{id}", async (long id, Supabase.Client client) =>
//     {
//         var response = await client
//             .From<Newsletter>()
//             .Where(n => n.Id == id)
//             .Get();
        
//         var newsletter = response.Models.FirstOrDefault();

//         if (newsletter is null)
//         {
//             return Results.NotFound();
//         }

//         var newsletterResponse = new NewsletterResponse
//         {
//             id = newsletter.Id,
//             Name = newsletter.Name,
//             Description = newsletter.Description,
//             ReadTime = newsletter.ReadTime,
//             CreatedAt = newsletter.CreatedAt
//         };

//         return Results.Ok(newsletterResponse);
//     });

// app.MapDelete("/newsletters/{id}", async (long id, Supabase.Client client) =>
// {
//     await client
//         .From<Newsletter>()
//         .Where(n => n.Id == id)
//         .Delete();
    
//     return Results.NoContent();
// });

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
