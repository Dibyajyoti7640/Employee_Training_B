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
    public class TrainingAssessmentsController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public TrainingAssessmentsController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/TrainingAssessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingAssessment>>> GetTrainingAssessments()
        {
            return await _context.TrainingAssessments.ToListAsync();
        }

        // GET: api/TrainingAssessments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingAssessment>> GetTrainingAssessment(int id)
        {
            var trainingAssessment = await _context.TrainingAssessments.FindAsync(id);

            if (trainingAssessment == null)
            {
                return NotFound();
            }

            return trainingAssessment;
        }

        // PUT: api/TrainingAssessments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingAssessment(int id, TrainingAssessment trainingAssessment)
        {
            if (id != trainingAssessment.AssessmentId)
            {
                return BadRequest();
            }

            _context.Entry(trainingAssessment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingAssessmentExists(id))
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

        // POST: api/TrainingAssessments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingAssessment>> PostTrainingAssessment(TrainingAssessment trainingAssessment)
        {
            _context.TrainingAssessments.Add(trainingAssessment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainingAssessment", new { id = trainingAssessment.AssessmentId }, trainingAssessment);
        }

        // DELETE: api/TrainingAssessments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingAssessment(int id)
        {
            var trainingAssessment = await _context.TrainingAssessments.FindAsync(id);
            if (trainingAssessment == null)
            {
                return NotFound();
            }

            _context.TrainingAssessments.Remove(trainingAssessment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingAssessmentExists(int id)
        {
            return _context.TrainingAssessments.Any(e => e.AssessmentId == id);
        }
    }
}
