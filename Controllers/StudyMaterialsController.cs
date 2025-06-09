using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Employee_Training_B.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudyMaterialsController : ControllerBase
    {
        private readonly EmpTdsContext _context;

        public StudyMaterialsController(EmpTdsContext context)
        {
            _context = context;
        }

        // GET: api/StudyMaterials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudyMaterial>>> GetStudyMaterials()
        {
            return await _context.StudyMaterials.ToListAsync();
        }

        // GET: api/StudyMaterials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudyMaterial>> GetStudyMaterial(int id)
        {
            var studyMaterial = await _context.StudyMaterials.FindAsync(id);

            if (studyMaterial == null)
            {
                return NotFound();
            }

            return studyMaterial;
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        public async Task<ActionResult> UploadFiles(IFormFile file, [FromForm] int courseID)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded or file is empty");
            }

            try
            {
                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var studyMaterial = new StudyMaterial
                {
                    DocumentName = Path.GetFileName(file.FileName), // Sanitized filename
                    Content = memoryStream.ToArray(),
                    CourseId = courseID
                };

                _context.StudyMaterials.Add(studyMaterial);
                await _context.SaveChangesAsync();

                return Ok(new { message = "File uploaded successfully", id = studyMaterial.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("download/{id}")]
       
        public async Task<ActionResult> DownloadMaterial(int id)
        {
            var material = await _context.StudyMaterials.FindAsync(id);
            if (material == null || material.Content == null)
            {
                return NotFound("File not found");
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(material.DocumentName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(material.Content, contentType, material.DocumentName);
        }
        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }
        // PUT: api/StudyMaterials/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudyMaterial(int id, StudyMaterial studyMaterial)
        {
            if (id != studyMaterial.Id)
            {
                return BadRequest();
            }

            _context.Entry(studyMaterial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudyMaterialExists(id))
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

        // POST: api/StudyMaterials
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudyMaterial>> PostStudyMaterial(StudyMaterial studyMaterial)
        {
            _context.StudyMaterials.Add(studyMaterial);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudyMaterialExists(studyMaterial.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStudyMaterial", new { id = studyMaterial.Id }, studyMaterial);
        }

        // DELETE: api/StudyMaterials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudyMaterial(int id)
        {
            var studyMaterial = await _context.StudyMaterials.FindAsync(id);
            if (studyMaterial == null)
            {
                return NotFound();
            }

            _context.StudyMaterials.Remove(studyMaterial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudyMaterialExists(int id)
        {
            return _context.StudyMaterials.Any(e => e.Id == id);
        }
    }
}
