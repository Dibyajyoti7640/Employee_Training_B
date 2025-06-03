using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employee_Training.Models;
using Microsoft.AspNetCore.Authorization;
using Employee_Training_B.Models;

namespace Employee_Training.Controllers
{
    [Authorize ]
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingProgramsController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public TrainingProgramsController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/TrainingPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetTrainingPrograms()
        {
            return await _context.TrainingPrograms.ToListAsync();
        }

        // GET: api/TrainingPrograms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgram>> GetTrainingProgram(int id)
        {
            var trainingProgram = await _context.TrainingPrograms.FindAsync(id);

            if (trainingProgram == null)
            {
                return NotFound();
            }

            return trainingProgram;
        }

        // PUT: api/TrainingPrograms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingProgram(int id, TrainingProgram trainingProgram)
        {
            if (id != trainingProgram.ProgramId)
            {
                return BadRequest();
            }

            _context.Entry(trainingProgram).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingProgramExists(id))
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

        // POST: api/TrainingPrograms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingProgram>> PostTrainingProgram(TrainingProgram trainingProgram)
        {
            _context.TrainingPrograms.Add(trainingProgram);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainingProgram", new { id = trainingProgram.ProgramId }, trainingProgram);
        }

        // DELETE: api/TrainingPrograms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingProgram(int id)
        {
            var trainingProgram = await _context.TrainingPrograms.FindAsync(id);
            if (trainingProgram == null)
            {
                return NotFound();
            }

            _context.TrainingPrograms.Remove(trainingProgram);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingProgramExists(int id)
        {
            return _context.TrainingPrograms.Any(e => e.ProgramId == id);
        }
    }
}
