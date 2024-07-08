using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Server.Configurations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts => opts.AddPolicy("AllowClient", policy =>
policy.WithOrigins($"http://localhost:3000/*", $"http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    ));

builder.Services.AddControllers();
builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));
builder.Services.ConfigureAutomapper();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IRepository<Theme>, ThemeRepository>();

using var scope = builder.Services.BuildServiceProvider().CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<EventPlanningDbContext>();
dbContext.Database.Migrate();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
