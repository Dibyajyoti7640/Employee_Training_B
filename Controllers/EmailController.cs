using Employee_Training_B.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PostmarkDotNet;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            try
            {
                var htmlBody = GenerateHtmlTemplate(request.Subject,request.Body);
                var message = new PostmarkMessage()
                {
                    To = request.To,
                    From = "21052869@kiit.ac.in",
                    TrackOpens = true,
                    Subject = request.Subject,
                    TextBody = request.Body,
                    HtmlBody = htmlBody,
                };
                var client = new PostmarkClient("83aaacca-bbea-408c-847b-41a2276ff5a9");
                var res = await client.SendMessageAsync(message);
                return Ok(res);

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
                <strong>📋 Important:</strong> Complete the course within the given time.
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
