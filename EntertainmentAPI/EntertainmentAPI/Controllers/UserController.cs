using EntertainmentAPI.Data;
using EntertainmentAPI.Models;
using EntertainmentAPI.Models.DTOs;  // Importinf the DTOs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly EntertainmentDbContext _context;

        public UsersController(EntertainmentDbContext context)
        {
            _context = context;
        }

        // Create User (Using DTO for creat usrr)
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(UserCreateDTO userCreateDTO)
        {
            var user = new User
            {
                Name = userCreateDTO.Name,
                Email = userCreateDTO.Email,
                PasswordHash = userCreateDTO.PasswordHash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // Get all Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Get a single User
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // Update User (Using DTO for update user)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDTO userUpdateDTO)
        {
            if (id != userUpdateDTO.UserId)
            {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Name = userUpdateDTO.Name;
            user.Email = userUpdateDTO.Email;
            user.PasswordHash = userUpdateDTO.PasswordHash;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete User
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
