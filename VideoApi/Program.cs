using Microsoft.EntityFrameworkCore;
using VideoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Controllers
builder.Services.AddControllers();

// connect to database
var videoAppConnectionString = builder.Configuration.GetConnectionString("VideoAppPostgreSql");
builder.Services.AddDbContext<VideoAppDBContext>(options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseNpgsql(videoAppConnectionString);
});

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
