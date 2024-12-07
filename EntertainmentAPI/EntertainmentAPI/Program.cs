using EntertainmentAPI.Data;
using EntertainmentAPI.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add JWT settings t container
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddSingleton(new JwtTokenHelper(
    jwtSettings["Key"],
    jwtSettings["Issuer"],
    jwtSettings["Audience"],
    int.Parse(jwtSettings["ExpiryMinutes"])  // Pass expiry time for token during
));

// Add DbContext for SQL Server
builder.Services.AddDbContext<EntertainmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers and other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register custom JWT authentication handler
builder.Services.AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>("Bearer", options => { });

var app = builder.Build();

// Automatically apply database migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<EntertainmentDbContext>();
    dbContext.Database.Migrate();  // This will apply any pending migrations after i deployed to azure
}

// Use custom JWT authentication middleware
app.UseMiddleware<JwtAuthenticationMiddleware>(); // Handle JWT token validation

// Enable middleware for authentication and authorization
app.UseAuthentication();  // <-- Make sure authentication is called before authorization
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
