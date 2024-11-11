using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts => opts.AddPolicy("AllowClient", policy =>
policy.WithOrigins($"{builder.Configuration["ClientUrl"]}")
    .AllowAnyHeader()
    .AllowAnyMethod()
    ));

builder.Services.AddAuthentication(x =>
{ 
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Issuer"],
        ValidAudience = builder.Configuration["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKey"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.ConfigureAutomapper();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IRepository<UserEvent>, UserEventRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Theme>, ThemeRepository>();
builder.Services.AddScoped<EmailSender>();

var connectionString = builder.Configuration.GetConnectionString("EventDb");

Action<DbContextOptionsBuilder> action = builder.Environment.IsDevelopment() && connectionString != null 
    ? options => options.UseSqlServer(connectionString)
    : options => options.UseInMemoryDatabase("EventDb");

builder.Services.AddDbContext<EventPlanningDbContext>(action);

using var scope = builder.Services?.BuildServiceProvider()?.CreateScope();
MigrateSeedDatabase(scope?.ServiceProvider.GetRequiredService<EventPlanningDbContext>());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapPost("events", () => Results.Ok())
//    .RequireAuthorization("Admin");

app.Run();

void MigrateSeedDatabase(EventPlanningDbContext? dbContext)
{
    if (builder.Environment.IsDevelopment())
    {
        dbContext?.Database.Migrate();
    }
    else
    {
        dbContext?.Seed();
    }
}