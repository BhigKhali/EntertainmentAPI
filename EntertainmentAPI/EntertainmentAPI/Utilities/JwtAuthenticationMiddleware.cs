using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using EntertainmentAPI.Utilities;

namespace EntertainmentAPI.Utilities
{
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtTokenHelper _jwtHelper;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;

        public JwtAuthenticationMiddleware(RequestDelegate next, JwtTokenHelper jwtHelper, ILogger<JwtAuthenticationMiddleware> logger)
        {
            _next = next;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Retrieve the Authorization header
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    // Validate the JWT token
                    var principal = _jwtHelper.ValidateToken(token);

                    if (principal != null)
                    {
                        // If the token is valid, set the user context
                        context.User = principal;
                    }
                    else
                    {
                        // If the token is invalid, set status code as Unauthorized
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Unauthorized: Invalid token");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Token validation failed: {Message}", ex.Message);
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token");
                    return;
                }
            }

            // Continue with the request processing
            await _next(context);
        }
    }
}
