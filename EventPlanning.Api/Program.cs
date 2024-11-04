using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Server.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EventPlanning.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var url = builder.Configuration["ClientUrl"];
builder.Services.AddCors(opts => opts.AddPolicy("AllowClient", policy =>
policy.WithOrigins($"{builder.Configuration["ClientUrl"]}")
    .AllowAnyHeader()
    .AllowAnyMethod()
    ));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Azure AD").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = AuthOptions.ISSUER,
        ValidateAudience = true,
        ValidAudience = AuthOptions.AUDIENCE,
        ValidateLifetime = true,
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddControllers();
builder.Services.ConfigureAutomapper();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IRepository<UserEvent>, UserEventRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<EmailSender>();

var connectionString = builder.Configuration.GetConnectionString("EventDb");

if (builder.Environment.IsDevelopment() && connectionString != null)
{
    builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseSqlServer(connectionString));
    MigrateDatabase();
}
else
{
    builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseInMemoryDatabase("EventDb"));
    SeedDatabase();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedDatabase()
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EventPlanningDbContext>();
    dbContext.Seed();
}

void MigrateDatabase()
{
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EventPlanningDbContext>();
    dbContext.Database.Migrate();
}
