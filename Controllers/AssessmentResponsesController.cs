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
    public class AssessmentResponsesController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public AssessmentResponsesController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/AssessmentResponses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssessmentResponse>>> GetAssessmentResponses()
        {
            return await _context.AssessmentResponses.ToListAsync();
        }

        // GET: api/AssessmentResponses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssessmentResponse>> GetAssessmentResponse(int id)
        {
            var assessmentResponse = await _context.AssessmentResponses.FindAsync(id);

            if (assessmentResponse == null)
            {
                return NotFound();
            }

            return assessmentResponse;
        }

        // PUT: api/AssessmentResponses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessmentResponse(int id, AssessmentResponse assessmentResponse)
        {
            if (id != assessmentResponse.ResponseId)
            {
                return BadRequest();
            }

            _context.Entry(assessmentResponse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessmentResponseExists(id))
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

        // POST: api/AssessmentResponses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssessmentResponse>> PostAssessmentResponse(AssessmentResponse assessmentResponse)
        {
            _context.AssessmentResponses.Add(assessmentResponse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssessmentResponse", new { id = assessmentResponse.ResponseId }, assessmentResponse);
        }

        // DELETE: api/AssessmentResponses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessmentResponse(int id)
        {
            var assessmentResponse = await _context.AssessmentResponses.FindAsync(id);
            if (assessmentResponse == null)
            {
                return NotFound();
            }

            _context.AssessmentResponses.Remove(assessmentResponse);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssessmentResponseExists(int id)
        {
            return _context.AssessmentResponses.Any(e => e.ResponseId == id);
        }
    }
}
