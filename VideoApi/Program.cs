using Microsoft.EntityFrameworkCore;
using VideoApi.Models;
using VideoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

app.Run();
