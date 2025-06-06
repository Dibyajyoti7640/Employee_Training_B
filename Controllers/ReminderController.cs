//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;
//using CsvHelper;
//using System.Globalization;
//using Employee_Training_B.Models.Dto;
//using PostmarkDotNet;
//using ClosedXML.Excel;  
//namespace Employee_Training_B.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ReminderController : ControllerBase
//    {
//        [HttpPost("upload")]
//        public async Task<IActionResult> UploadEmails(IFormFile file, [FromForm] string subject, [FromForm] string body)
//        {
//            if (file == null || file.Length == 0)
//                return BadRequest("No file uploaded.");

//            var emails = new List<string>();

//            try
//            {
//                var extension = Path.GetExtension(file.FileName).ToLower();

//                if (extension == ".csv")
//                {

//                    using var stream = new StreamReader(file.OpenReadStream());
//                    using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
//                    while (csv.Read())
//                    {
//                        var email = csv.GetField(0); 
//                        if (!string.IsNullOrWhiteSpace(email))
//                            emails.Add(email.Trim());
//                    }
//                }
//                else if (extension == ".xlsx")
//                {

//                    using var stream = file.OpenReadStream();
//                    using var workbook = new XLWorkbook(stream);
//                    var worksheet = workbook.Worksheet(1); 

//                    var emailColumn = worksheet.Column(1); 


//                    foreach (var cell in emailColumn.CellsUsed().Skip(1))
//                    {
//                        var email = cell.Value.ToString();
//                        if (!string.IsNullOrWhiteSpace(email))
//                            emails.Add(email.Trim());
//                    }
//                }
//                else
//                {
//                    return BadRequest("Unsupported file format. Please upload .csv or .xlsx.");
//                }


//                var client = new PostmarkClient("bd47bbcb-2fe2-4b9b-99d4-d7b95a1149dd");
//                foreach (var email in emails)
//                {
//                    var message = new PostmarkMessage()
//                    {
//                        To = email,
//                        From = "21cse591.dibyajyotisahoo@giet.edu",
//                        TrackOpens = true,
//                        Subject = subject,
//                        TextBody = body
//                    };
//                    await client.SendMessageAsync(message);
//                }

//                return Ok(new { Message = $"Emails sent to {emails.Count} recipients." });
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, ex.Message);
//            }
//        }
//    }
//}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using CsvHelper;
using System.Globalization;
using Employee_Training_B.Models.Dto;
using PostmarkDotNet;
using ClosedXML.Excel;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadEmails(IFormFile file, [FromForm] string subject, [FromForm] string body)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var emails = new List<string>();
            try
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (extension == ".csv")
                {
                    using var stream = new StreamReader(file.OpenReadStream());
                    using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);
                    while (csv.Read())
                    {
                        var email = csv.GetField(0);
                        if (!string.IsNullOrWhiteSpace(email))
                            emails.Add(email.Trim());
                    }
                }
                else if (extension == ".xlsx")
                {
                    using var stream = file.OpenReadStream();
                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1);
                    var emailColumn = worksheet.Column(1);

                    foreach (var cell in emailColumn.CellsUsed().Skip(1))
                    {
                        var email = cell.Value.ToString();
                        if (!string.IsNullOrWhiteSpace(email))
                            emails.Add(email.Trim());
                    }
                }
                else
                {
                    return BadRequest("Unsupported file format. Please upload .csv or .xlsx.");
                }

                // Generate HTML content using template
                var htmlBody = GenerateHtmlTemplate(subject, body);

                var client = new PostmarkClient("bd47bbcb-2fe2-4b9b-99d4-d7b95a1149dd");
                foreach (var email in emails)
                {
                    var message = new PostmarkMessage()
                    {
                        To = email,
                        From = "21cse591.dibyajyotisahoo@giet.edu",
                        TrackOpens = true,
                        Subject = subject,
                        HtmlBody = htmlBody,
                        TextBody = body
                    };
                    await client.SendMessageAsync(message);
                }

                return Ok(new { Message = $"Emails sent to {emails.Count} recipients." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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