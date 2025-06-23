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
        public CertificatesController(EmpTdsContext context,EmailService service)
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
        public async Task<IActionResult> Approval(int certficateID, [FromForm] string subject, [FromForm]string body,[FromForm] string  employeeName,[FromForm] string certificationType,[FromForm] string justification)
        {
            var cert = await _context.Certificates.FindAsync(certficateID);
            if (cert == null)
            {
                return BadRequest("Certificate not found");
            }
            var htmlBody = _emailService.GenerateCertificationRequestTemplate(employeeName,certificationType,justification);
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
        public async Task<ActionResult> Review(int certificateID,string email,[FromForm]String subject,[FromForm]string body, int hrID, bool isApproved, string? remarks, [FromForm] string EmployeeName, [FromForm] string CertificationType, [FromForm] string adminName, [FromForm] string responseDate, [FromForm] string comments = "", [FromForm] string nextSteps = "")
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
        public async Task<ActionResult> downloadCertificate(string certificateID)
        {
            var material = await _context.Certificates.FindAsync(certificateID);
            if(material == null)
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
            var certificate = await _context.Certificates.FirstOrDefaultAsync(u=>u.TraineeId == Traineeid);

            if (certificate == null)
            {
                return NotFound();
            }

            return certificate;
        }

    }
}
