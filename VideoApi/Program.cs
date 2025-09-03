using Microsoft.EntityFrameworkCore;
using VideoApi.Models;
using VideoApi.Extensions;
using VideoApi.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Controllers
builder.Services.AddControllers();

// Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// connect to database
var videoAppConnectionString = builder.Configuration.GetConnectionString("VideoAppPostgreSql");
builder.Services.AddDbContext<VideoAppDBContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseNpgsql(videoAppConnectionString);
});

// Email
builder.Services.Configure<EmailSettingsModel>(builder.Configuration.GetSection("EmailSettings"));

// JWT
builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JwtSettings"));

// Register repositories
builder.Services.RegisterRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Controllers
app.MapControllers();


// Authentication
app.UseAuthentication();
app.UseAuthorization();

// For Files
app.UseStaticFiles();

app.Run();
