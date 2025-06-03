using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employee_Training.Models;
using Employee_Training_B.Models;

namespace Employee_Training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingFeedbacksController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public TrainingFeedbacksController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/TrainingFeedbacks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingFeedback>>> GetTrainingFeedbacks()
        {
            return await _context.TrainingFeedbacks.ToListAsync();
        }

        // GET: api/TrainingFeedbacks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingFeedback>> GetTrainingFeedback(int id)
        {
            var trainingFeedback = await _context.TrainingFeedbacks.FindAsync(id);

            if (trainingFeedback == null)
            {
                return NotFound();
            }

            return trainingFeedback;
        }

        // PUT: api/TrainingFeedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingFeedback(int id, TrainingFeedback trainingFeedback)
        {
            if (id != trainingFeedback.FeedbackId)
            {
                return BadRequest();
            }

            _context.Entry(trainingFeedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingFeedbackExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TrainingFeedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingFeedback>> PostTrainingFeedback(TrainingFeedback trainingFeedback)
        {
            _context.TrainingFeedbacks.Add(trainingFeedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainingFeedback", new { id = trainingFeedback.FeedbackId }, trainingFeedback);
        }

        // DELETE: api/TrainingFeedbacks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingFeedback(int id)
        {
            var trainingFeedback = await _context.TrainingFeedbacks.FindAsync(id);
            if (trainingFeedback == null)
            {
                return NotFound();
            }

            _context.TrainingFeedbacks.Remove(trainingFeedback);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingFeedbackExists(int id)
        {
            return _context.TrainingFeedbacks.Any(e => e.FeedbackId == id);
        }
    }
}
