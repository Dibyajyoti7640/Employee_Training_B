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
    public class TrainingCertificatesController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public TrainingCertificatesController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/TrainingCertificates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingCertificate>>> GetTrainingCertificates()
        {
            return await _context.TrainingCertificates.ToListAsync();
        }

        // GET: api/TrainingCertificates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingCertificate>> GetTrainingCertificate(int id)
        {
            var trainingCertificate = await _context.TrainingCertificates.FindAsync(id);

            if (trainingCertificate == null)
            {
                return NotFound();
            }

            return trainingCertificate;
        }

        // PUT: api/TrainingCertificates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainingCertificate(int id, TrainingCertificate trainingCertificate)
        {
            if (id != trainingCertificate.CertificateId)
            {
                return BadRequest();
            }

            _context.Entry(trainingCertificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingCertificateExists(id))
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

        // POST: api/TrainingCertificates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TrainingCertificate>> PostTrainingCertificate(TrainingCertificate trainingCertificate)
        {
            _context.TrainingCertificates.Add(trainingCertificate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrainingCertificate", new { id = trainingCertificate.CertificateId }, trainingCertificate);
        }

        // DELETE: api/TrainingCertificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainingCertificate(int id)
        {
            var trainingCertificate = await _context.TrainingCertificates.FindAsync(id);
            if (trainingCertificate == null)
            {
                return NotFound();
            }

            _context.TrainingCertificates.Remove(trainingCertificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrainingCertificateExists(int id)
        {
            return _context.TrainingCertificates.Any(e => e.CertificateId == id);
        }
    }
}
