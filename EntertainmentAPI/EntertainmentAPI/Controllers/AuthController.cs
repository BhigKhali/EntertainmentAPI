using EntertainmentAPI.Utilities; // For JwtTokenHelper fie
using Microsoft.AspNetCore.Mvc;

namespace EntertainmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenHelper _jwtHelper;

        public AuthController(JwtTokenHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            // Validate user credentials (for testing i will have them hard coded but will change overtime)
            if (loginRequest.Username == "admin" && loginRequest.Password == "password")
            {
                var token = _jwtHelper.GenerateToken(loginRequest.Username);

                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }
    }

    // Request model for login
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
