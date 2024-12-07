using EntertainmentAPI.Data;
using EntertainmentAPI.Models.DTOs;
using EntertainmentAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EntertainmentAPI.Utilities;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly EntertainmentDbContext _context;

    public UsersController(EntertainmentDbContext context)
    {
        _context = context;
    }

    // Create User (Accessible without authentication)
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(UserCreateDTO userCreateDTO)
    {
        // Hash the password before saving to the database
        var hashedPassword = PasswordHelper.HashPassword(userCreateDTO.PasswordHash);

        var user = new User
        {
            Name = userCreateDTO.Name,
            Email = userCreateDTO.Email,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
    }
    // Get all Users (Accessible by Admins only)
    [HttpGet]
   // [Authorize(Roles = "Admin")] will do that subsequently just did to test
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // Get a single User (Accessible to Admins or the specific user)
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        // Allow Admins or the specific user to access their details
        if (!User.IsInRole("Admin") && User.Identity?.Name != user.Email)
        {
            return Forbid();
        }

        return user;
    }

    // Update User (Accessible to Admins or the specific user)
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

        // Allow Admins or the specific user to update their details
      //  if (!User.IsInRole("Admin") && User.Identity?.Name != user.Email)
       // {
         //   return Forbid();
       // }

        user.Name = userUpdateDTO.Name;
        user.Email = userUpdateDTO.Email;
        user.PasswordHash = userUpdateDTO.PasswordHash;

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // Delete User (Accessible by Admins only)
    [HttpDelete("{id}")]
   // [Authorize(Roles = "Admin")]
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
