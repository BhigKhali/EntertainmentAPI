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
    public class FeedbacksController : ControllerBase
    {
        private readonly EntertainmentDbContext _context;

        public FeedbacksController(EntertainmentDbContext context)
        {
            _context = context;
        }

        // Create Feedback (Using DTO)
        [HttpPost]
        public async Task<ActionResult<Feedback>> CreateFeedback(FeedbackCreateDTO feedbackCreateDTO)
        {
            var feedback = new Feedback
            {
                EventId = feedbackCreateDTO.EventId,
                UserId = feedbackCreateDTO.UserId,
                Comment = feedbackCreateDTO.Comment,
                Rating = feedbackCreateDTO.Rating
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.FeedbackId }, feedback);
        }

        // Get all Feedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        // Get a single Feedback
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return feedback;
        }

        // Update Feedback (Using DTO)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, FeedbackUpdateDTO feedbackUpdateDTO)
        {
            if (id != feedbackUpdateDTO.FeedbackId)
            {
                return BadRequest();
            }

            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Comment = feedbackUpdateDTO.Comment;
            feedback.Rating = feedbackUpdateDTO.Rating;

            _context.Entry(feedback).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Feedback
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
