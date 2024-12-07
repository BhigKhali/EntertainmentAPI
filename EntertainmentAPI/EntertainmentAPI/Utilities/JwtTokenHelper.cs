using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EntertainmentAPI.Utilities
{
    public class JwtTokenHelper
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        // Constructor to inject JWT settings
        public JwtTokenHelper(string key, string issuer, string audience, int expiryMinutes)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
            _expiryMinutes = expiryMinutes;
        }

        // Method to generate the JWT token
        public string GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, email) // Use email as the name claim
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Method to validate the JWT token
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key); // Use the same key to validate the token

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return principal; // Return the principal if token is valid
            }
            catch (Exception)
            {
                return null; // Return null if token is invalid or expired
            }
        }
    }
}
