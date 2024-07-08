using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data;
using EventPlanning.Data.Entities;
using EventPlanning.Server.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static EventPlanning.Server.Controllers.AuthorizationController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opts => opts.AddPolicy("AllowClient", policy =>
policy.WithOrigins($"http://localhost:3000/*", $"http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()
    ));

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // ���������, ����� �� �������������� �������� ��� ��������� ������
        ValidateIssuer = true,
        // ������, �������������� ��������
        ValidIssuer = AuthOptions.ISSUER,
        // ����� �� �������������� ����������� ������
        ValidateAudience = true,
        // ��������� ����������� ������
        ValidAudience = AuthOptions.AUDIENCE,
        // ����� �� �������������� ����� �������������
        ValidateLifetime = true,
        // ��������� ����� ������������
        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
        // ��������� ����� ������������
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddControllers();
builder.Services.AddDbContext<EventPlanningDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDb")));
builder.Services.ConfigureAutomapper();
builder.Services.AddScoped<IRepository<Event>, EventRepository>();
builder.Services.AddScoped<IRepository<Theme>, ThemeRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<UserEvent>, UserEventRepository>();
builder.Services.AddScoped<EmailSender>();

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
