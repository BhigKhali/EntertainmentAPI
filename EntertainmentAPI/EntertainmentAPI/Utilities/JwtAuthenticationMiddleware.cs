namespace EntertainmentAPI.Utilities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class JwtAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtTokenHelper _jwtHelper;

    public JwtAuthenticationMiddleware(RequestDelegate next, JwtTokenHelper jwtHelper)
    {
        _next = next;
        _jwtHelper = jwtHelper;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Validate the token
            var principal = _jwtHelper.ValidateToken(token);
            if (principal != null)
            {
                context.User = principal;  // Set the User context to the validated claims
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;  // Unauthorized access if token is invalid
            }
        }

        await _next(context);
    }
}
