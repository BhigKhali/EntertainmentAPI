using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EntertainmentAPI.Utilities
{
    public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly JwtTokenHelper _jwtHelper;

        public JwtAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            JwtTokenHelper jwtHelper)
            : base(options, logger, encoder, clock)
        {
            _jwtHelper = jwtHelper;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Get Authorization header
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            // Check if the header is present and starts with 'Bearer'
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer ", System.StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Missing or invalid Authorization header");
            }

            // Extract token from the Authorization header
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            // Validate the token using JwtTokenHelper
            var principal = _jwtHelper.ValidateToken(token);
            if (principal == null)
            {
                return AuthenticateResult.Fail("Invalid token");
            }

            // Create authentication ticket with claims and the validated user
            var identity = principal.Identity as ClaimsIdentity;
            var claims = new List<Claim>(identity?.Claims ?? new List<Claim>());
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), "Bearer");

            // Return successful authentication result
            return AuthenticateResult.Success(ticket);
        }
    }
}
