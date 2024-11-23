using EntertainmentAPI.Data;
using EntertainmentAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EntertainmentDbContext _context;

        public EventController(EntertainmentDbContext context)
        {
            _context = context;
        }

        // GET: api/Event
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        // GET: api/Event/{eventId}
        [HttpGet("{eventId}")]
        public async Task<ActionResult<Event>> GetEvent(int eventId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null) return NotFound();

            return evnt;
        }

        // POST: api/Event
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event evnt)
        {
            _context.Events.Add(evnt);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { eventId = evnt.EventId }, evnt);
        }

        // PUT: api/Event/{eventId}
        [HttpPut("{eventId}")]
        public async Task<IActionResult> PutEvent(int eventId, Event evnt)
        {
            if (eventId != evnt.EventId) return BadRequest();

            _context.Entry(evnt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(eventId)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/Event/{eventId}
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var evnt = await _context.Events.FindAsync(eventId);
            if (evnt == null) return NotFound();

            _context.Events.Remove(evnt);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventExists(int eventId)
        {
            return _context.Events.Any(e => e.EventId == eventId);
        }
    }
}
