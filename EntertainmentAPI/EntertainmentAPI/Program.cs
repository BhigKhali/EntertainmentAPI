using EntertainmentAPI.Data;
using EntertainmentAPI.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Load JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddSingleton(new JwtTokenHelper(
    jwtSettings["Key"],
    jwtSettings["Issuer"],
    jwtSettings["Audience"],
    int.Parse(jwtSettings["ExpiryMinutes"])  // Pass expiry time
));


// Configure DbContext
builder.Services.AddDbContext<EntertainmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Use custom JWT authentication middleware (if any other middleware is present, ensure it's correctly ordered)
app.UseMiddleware<JwtAuthenticationMiddleware>();

// Enable middleware for authentication and authorization
app.UseAuthentication();  // This should be called before app.UseAuthorization()
app.UseAuthorization();

// Swagger and other middlewares (development specific)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();