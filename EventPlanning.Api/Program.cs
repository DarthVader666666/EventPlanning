using EventPlanning.Api.Configurations;
using EventPlanning.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://event-planning-server.azurewebsites.net/",
        ValidateAudience = true,
        ValidAudience = "https://yellow-sand-066b7f603.5.azurestaticapps.net",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddDbContext<EventPlanningDbContext>(opt => opt.UseInMemoryDatabase("EventDb"));

builder.Services.AddControllers();
builder.Services.AddRouting();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

//app.UseStatusCodePages();

//app.UseAuthorization();
//app.UseAuthentication();

//app.MapGet("/", () => "Hello World!");

app.Run();
