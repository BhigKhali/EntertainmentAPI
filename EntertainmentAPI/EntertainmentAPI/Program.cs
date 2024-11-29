using EntertainmentAPI.Data;
using EntertainmentAPI.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Loading JWT settings from configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddSingleton(new JwtTokenHelper(
    jwtSettings["Key"],
    jwtSettings["Issuer"],
    jwtSettings["Audience"],
    int.Parse(jwtSettings["ExpiryMinutes"])  // Pass expiry time for token during
));


// Configure DbContext
builder.Services.AddDbContext<EntertainmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Use custom JWT authentication middleware (jwtBearer package not compatible with asp.net 8)
app.UseMiddleware<JwtAuthenticationMiddleware>();

// Enable middleware for authentication and authorization
app.UseAuthentication();  
app.UseAuthorization();

// Swagger and other middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();