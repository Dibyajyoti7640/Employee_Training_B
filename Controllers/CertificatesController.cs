using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using Employee_Training_B.Models;
using Employee_Training_B.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private string GenerateHtmlTemplate(string subject, string body)
        {
            var logo = "https://gyansys.com/wp-content/uploads/2023/06/gyansys.gif";
            var htmlTemplate = $@"

        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>{subject}</title>
            <style>
                body {{
                    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    line-height: 1.6;
                    margin: 0;
                    padding: 0;
                    background-color: #f4f4f4;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                    overflow: hidden;
                }}
                .header {{
                    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                    color: white;
                    padding: 30px 20px;
                    text-align: center;
                }}
                .header h1 {{
                    margin: 0;
                    font-size: 24px;
                    font-weight: 300;
                }}
                .content {{
                    padding: 30px;
                }}
                .message-body {{
                    background-color: #f8f9fa;
                    padding: 20px;
                    border-left: 4px solid #667eea;
                    margin: 20px 0;
                    border-radius: 4px;
                }}
                .message-body p {{
                    margin: 0;
                    color: #333;
                    white-space: pre-line;
                }}
                .footer {{
                    background-color: #f8f9fa;
                    padding: 20px;
                    text-align: center;
                    border-top: 1px solid #e9ecef;
                }}
                .footer p {{
                    margin: 0;
                    color: #6c757d;
                    font-size: 14px;
                }}
                .company-info {{
                    margin-top: 20px;
                    padding-top: 20px;
                    border-top: 1px solid #e9ecef;
                }}
                .company-info h3 {{
                    color: #667eea;
                    margin: 0 0 10px 0;
                    font-size: 18px;
                }}
                .highlight {{
                    background-color: #fff3cd;
                    border: 1px solid #ffeaa7;
                    padding: 15px;
                    border-radius: 4px;
                    margin: 15px 0;
                }}
                @media only screen and (max-width: 600px) {{
                    .email-container {{
                        margin: 10px;
                        border-radius: 0;
                    }}
                    .header, .content, .footer {{
                        padding: 20px 15px;
                    }}
                }}
            </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""header"">
        <img src='{logo}' alt=""GyanSys Logo"" style=""max-height: 60px; margin-bottom: 15px;"">
                    <h1>Employee Training Reminder</h1>
                </div>

                <div class=""content"">
                    <h2 style=""color: #333; margin-top: 0;"">{subject}</h2>

                    <div class=""message-body"">
                        <p>{body}</p>
                    </div>

                    <div class=""highlight"">
                        <strong>📋 Important:</strong> Please ensure you complete your training requirements on time.
                    </div>

                    <div class=""company-info"">
                        <h3>Employee Training Department</h3>
                        <p style=""color: #666; margin: 5px 0;"">
                            🏢 GyanSys
                        </p>
                    </div>
                </div>

                <div class=""footer"">
                    <p>This is an automated reminder. Please do not reply to this email.</p>
                    <p style=""margin-top: 10px;"">© 2025 GyanSys Inc. - Employee Training System</p>
                </div>
            </div>
        </body>
        </html>";

            return htmlTemplate;
        }


    }
}
