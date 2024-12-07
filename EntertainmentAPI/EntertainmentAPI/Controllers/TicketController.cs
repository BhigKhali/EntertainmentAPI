using EntertainmentAPI.Data;
using EntertainmentAPI.Models;
using EntertainmentAPI.Models.DTOs;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly EntertainmentDbContext _context;

        public TicketsController(EntertainmentDbContext context)
        {
            _context = context;
        }

        // Create Ticket (Using DTO)
        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket(TicketCreateDTO ticketCreateDTO)
        {
            var ticket = new Ticket
            {
                EventId = ticketCreateDTO.EventId,
                UserId = ticketCreateDTO.UserId,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticket);
        }

        // Get all Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // Get a single Ticket
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // Update Ticket (Using DTO)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, TicketUpdateDTO ticketUpdateDTO)
        {
            if (id != ticketUpdateDTO.TicketId)
            {
                return BadRequest();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.EventId = ticketUpdateDTO.EventId;
            ticket.UserId = ticketUpdateDTO.UserId;
            ticket.PurchaseDate = ticketUpdateDTO.PurchaseDate;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Ticket
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
