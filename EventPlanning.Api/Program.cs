using EventPlanning.Api.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "https://event-planning-server.azurewebsites.net/",
        ValidateAudience = true,
        ValidAudience = "https://event-planning-server.azurewebsites.net/",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysupersecret_secretsecretsecretkey!123")),
        ValidateIssuerSigningKey = true,
    };
});

var app = builder.Build();

//app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Hello World!");

app.Run();
