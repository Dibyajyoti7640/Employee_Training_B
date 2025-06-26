using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using Employee_Training_B.Models;
using Employee_Training_B.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using PostmarkDotNet;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly EmpTdsContext _context;
        private readonly EmailService _emailService;
        public CertificatesController(EmpTdsContext context, EmailService service)
        {
            _context = context;
            _emailService = service;
        }
        [HttpPost("upload")]
        public async Task<ActionResult> Upload(IFormFile file, [FromForm] string title, [FromForm] int traineeId)
        {
            var trainee = await _context.Users.FindAsync(traineeId);
            if (trainee == null)
            {
                return BadRequest("No Employee found");
            }
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var cert = new Certificate
            {
                TraineeId = traineeId,
                Title = title,
                FileContent = ms.ToArray(),
                FileName = Path.GetFileName(file.FileName),
            };
            _context.Certificates.Add(cert);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Certificate Uploaded", cert.Id });

        }

        [HttpPost("submit")]
        public async Task<IActionResult> Approval([FromForm] int certificateID, [FromForm] string subject, [FromForm] string body, [FromForm] string employeeName, [FromForm] string certificationType, [FromForm] string justification)
        {
            var cert = await _context.Certificates.FindAsync(certificateID);
            if (cert == null)
            {
                return BadRequest("Certificate not found");
            }
            cert.Status = "Under Review";

            await _context.SaveChangesAsync();
            var htmlBody = _emailService.GenerateCertificationRequestTemplate(employeeName, certificationType, justification);

            var client = new PostmarkClient("83aaacca-bbea-408c-847b-41a2276ff5a9");
            var email = "shankhosuvro.ghosh@gmail.com";
            var message = new PostmarkMessage()
            {
                To = email,
                From = "21052869@kiit.ac.in",
                TrackOpens = true,
                Subject = subject,
                HtmlBody = htmlBody,
                TextBody = body,
            };
            await client.SendMessageAsync(message);
            return Ok(new { message = "Email sent successfully" });
        }


        [HttpPost("review")]
        public async Task<ActionResult> Review([FromForm] int certificateID, [FromForm] string email, [FromForm] string subject, [FromForm] string body,[FromForm] int hrID, [FromForm] bool isApproved, [FromForm] string? remarks, [FromForm] string EmployeeName, [FromForm] string CertificationType, [FromForm] string adminName, [FromForm] string responseDate, [FromForm] string comments = "", [FromForm] string nextSteps = "")
        {
            var cert = await _context.Certificates.FindAsync(certificateID);
            if (cert == null)
            {
                return BadRequest("Certificate not found");
            }
            var htmlBody = _emailService.GenerateCertificationResponseTemplate(EmployeeName, CertificationType, adminName, responseDate, isApproved, comments, nextSteps);
            cert.Status = isApproved ? "Approved" : "Rejected";
            cert.ReviewedBy = hrID;
            cert.ReviewedOn = DateTime.Now;
            cert.Remarks = remarks;
            var client = new PostmarkClient("83aaacca-bbea-408c-847b-41a2276ff5a9");

            var message = new PostmarkMessage()
            {
                To = email,
                From = "21052869@kiit.ac.in",
                TrackOpens = true,
                Subject = subject,
                HtmlBody = htmlBody,
                TextBody = body,
            };
            await client.SendMessageAsync(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Certificate review successfully" });
        }
        [HttpGet("download/{certificateID}")]
        public async Task<ActionResult> downloadCertificate(int certificateID)
        {
            var material = await _context.Certificates.FindAsync(certificateID);
            if (material == null)
            {
                return BadRequest("No Certificate found");
            }
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(material.FileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return File(material.FileContent, contentType, material.FileName);
        }
        [HttpGet("{Traineeid}")]
        public async Task<ActionResult<Certificate>> GetStudyMaterial(int Traineeid)
        {

            var certificate = await _context.Certificates
             .Where(u => u.TraineeId == Traineeid)
             .Select(c => new
             {
                 c.Id,
                 c.TraineeId,
                 c.Title,
                 c.FileName,
                 c.SubmittedOn,
                 c.Status,
                 c.ReviewedOn,
                 c.ReviewedBy,
                 c.Remarks
             })
             .ToListAsync();

            if (certificate == null)
            {
                return NotFound();
            }

            return Ok(certificate);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCertificate(int id, IFormFile? file, [FromForm] string? title)
        {
            var cert = await _context.Certificates.FindAsync(id);
            if (cert == null)
            {
                return NotFound("Certificate not found");
            }

            // Update title if provided
            if (!string.IsNullOrEmpty(title))
            {
                cert.Title = title;
            }

            // Update file if provided
            if (file != null && file.Length > 0)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                cert.FileContent = ms.ToArray();
                cert.FileName = Path.GetFileName(file.FileName);
            }

            cert.SubmittedOn = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Certificate updated successfully", cert.Id });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCertificate(int id)
        {
            var cert = await _context.Certificates.FindAsync(id);
            if (cert == null)
            {
                return NotFound("Certificate not found");
            }

            _context.Certificates.Remove(cert);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Certificate deleted successfully" });
        }

    }
}
