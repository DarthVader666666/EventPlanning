using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Api.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        ValidIssuer = builder.Configuration["Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecurityKey"])),
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddControllers();
builder.Services.ConfigureAutomapper();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IRepository<UserEvent>, UserEventRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Theme>, ThemeRepository>();
builder.Services.AddScoped<EmailSender>();

var connectionString = builder.Configuration.GetConnectionString("EventDb");

if (builder.Environment.IsDevelopment() && connectionString != null)
{
    builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseSqlServer(connectionString));
}
else
{
    builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseInMemoryDatabase("EventDb"));
}

using var scope = builder.Services?.BuildServiceProvider()?.CreateScope();
MigrateSeedDatabase(scope?.ServiceProvider.GetRequiredService<EventPlanningDbContext>());

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

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