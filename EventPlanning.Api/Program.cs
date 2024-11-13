using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EventPlanning.Bll.Services.SqlRepositories;
using JsonFlatFileDataStore;
using EventPlanning.Bll.Services.JsonRepositories;

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

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IRepository<Event>, EventRepository>();
    builder.Services.AddScoped<IRepository<UserEvent>, UserEventRepository>();
    builder.Services.AddScoped<IRepository<User>, UserRepository>();
    builder.Services.AddScoped<IRepository<Role>, RoleRepository>();
    builder.Services.AddScoped<IRepository<Theme>, ThemeRepository>();

    var connectionString = builder.Configuration.GetConnectionString("EventDb");
    builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    var path = $"{Directory.GetCurrentDirectory()}\\eventDb.json";

    builder.Services.AddScoped<IRepository<Event>, EventJsonRepository>();
    builder.Services.AddScoped<IRepository<UserEvent>, UserEventJsonRepository>();
    builder.Services.AddScoped<IRepository<User>, UserJsonRepository>();
    builder.Services.AddScoped<IRepository<Role>, RoleJsonRepository>();
    builder.Services.AddScoped<IRepository<Theme>, ThemeJsonRepository>();
    builder.Services.AddScoped(serviceProvider => new DataStore(path, useLowerCamelCase: false));
}


builder.Services.AddScoped<EmailSender>();

using var scope = builder.Services?.BuildServiceProvider()?.CreateScope();
await MigrateSeedDatabase(scope);

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

async Task MigrateSeedDatabase(IServiceScope? scope)
{
    if (builder.Environment.IsDevelopment())
    {
        var dbContext = scope?.ServiceProvider.GetRequiredService<EventPlanningDbContext>();
        dbContext?.Database.Migrate();
    }
    else
    {
        var dataStore = scope?.ServiceProvider.GetRequiredService<DataStore>();
        var userCollection = dataStore.GetCollection<User>();
        var roleCollection = dataStore.GetCollection<Role>();
        var userRoleCollection = dataStore.GetCollection<UserRole>();

        if (!userCollection.AsQueryable().Any(user => user.Email == "rumyancer@gmail.com"))
        {
            await userCollection.InsertOneAsync(new User { UserId = 1, Email = "rumyancer@gmail.com", Password = "Haemorr_8421" });
            await roleCollection.InsertOneAsync(new Role { RoleId = 1, RoleName = "Admin" });
            await userRoleCollection.InsertOneAsync(new UserRole {  RoleId = 1, UserId = 1 });
        }
    }
}