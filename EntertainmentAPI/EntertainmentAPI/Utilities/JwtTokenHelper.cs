namespace EntertainmentAPI.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenHelper
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    public JwtTokenHelper(string secretKey, string issuer, string audience, int expiryMinutes)
    {
        _secretKey = secretKey;
        _issuer = issuer;
        _audience = audience;
        _expiryMinutes = expiryMinutes;
    }

    // Method to generate a JWT token
    public string GenerateToken(string username)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, username),
            // Add more claims as necessary
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.Now.AddMinutes(_expiryMinutes),  // Use ExpiryMinutes from config
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Method to validate a JWT token
    public ClaimsPrincipal ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var tokenHandler = new JwtSecurityTokenHandler();
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = key
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, parameters, out _);
            return principal;  // Returns ClaimsPrincipal if the token is valid
        }
        catch
        {
            return null;  // Token is invalid
        }
    }
}
