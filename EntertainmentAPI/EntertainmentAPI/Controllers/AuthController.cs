using EntertainmentAPI.Data; // For DbContext
using EntertainmentAPI.Models; // For User model
using EntertainmentAPI.Utilities; // For JwtTokenHelper
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For async DB operations
using System.Linq;

namespace EntertainmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtHelper;
        private readonly EntertainmentDbContext _context;

        public AuthController(JwtTokenHelper jwtHelper, EntertainmentDbContext context)
        {
            _jwtHelper = jwtHelper;
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDTO.Email);

            if (user == null || !PasswordHelper.VerifyPassword(loginDTO.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            // Generate token if credentials are valid
            var token = _jwtHelper.GenerateToken(user.Email);
            return Ok(new { Token = token });
        }
    }

    // Request model for login
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
