using PostmarkDotNet;
namespace Employee_Training_B.Services
{
    public class EmailService
    {
        public string GenerateCertificationRequestTemplate(string employeeName, string certificationType, string justification)
        {
            var logo = "https://gyansys.com/wp-content/uploads/2023/06/gyansys.gif";
            var htmlTemplate = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Certification Approval Request</title>
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
            background: linear-gradient(135deg, #28a745 0%, #20c997 100%);
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
        .request-details {{
            background-color: #f8f9fa;
            padding: 20px;
            border-left: 4px solid #28a745;
            margin: 20px 0;
            border-radius: 4px;
        }}
        .detail-row {{
            display: flex;
            justify-content: space-between;
            margin: 10px 0;
            padding: 8px 0;
            border-bottom: 1px solid #e9ecef;
        }}
        .detail-label {{
            font-weight: bold;
            color: #495057;
            flex: 1;
        }}
        .detail-value {{
            color: #333;
            flex: 2;
        }}
        .justification-section {{
            background-color: #e8f5e8;
            padding: 15px;
            border-radius: 4px;
            margin: 20px 0;
        }}
        .justification-section h4 {{
            margin: 0 0 10px 0;
            color: #28a745;
        }}
        .action-buttons {{
            text-align: center;
            margin: 30px 0;
        }}
        .btn {{
            display: inline-block;
            padding: 12px 30px;
            margin: 0 10px;
            text-decoration: none;
            border-radius: 5px;
            font-weight: bold;
            font-size: 16px;
        }}
        .btn-approve {{
            background-color: #28a745;
            color: white;
        }}
        .btn-reject {{
            background-color: #dc3545;
            color: white;
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
            color: #28a745;
            margin: 0 0 10px 0;
            font-size: 18px;
        }}
        .highlight {{
            background-color: #d1ecf1;
            border: 1px solid #bee5eb;
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
            .detail-row {{
                flex-direction: column;
            }}
            .detail-label, .detail-value {{
                flex: none;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <img src='{logo}' alt=""GyanSys Logo"" style=""max-height: 60px; margin-bottom: 15px;"">
            <h1>Certification Approval Request</h1>
        </div>
        <div class=""content"">
            <h2 style=""color: #333; margin-top: 0;"">New Certification Request Submitted</h2>
            
            <div class=""request-details"">
                <div class=""detail-row"">
                    <div class=""detail-label"">Employee Name:</div>
                    <div class=""detail-value"">{employeeName}</div>
                </div>
                <div class=""detail-row"">
                    <div class=""detail-label"">Certification Type:</div>
                    <div class=""detail-value"">{certificationType}</div>
                </div>
                
            </div>

            <div class=""justification-section"">
                <h4>📝 Justification</h4>
                <p style=""margin: 0; color: #333;"">{justification}</p>
            </div>

            

            <div class=""highlight"">
                <strong>⏰ Action Required:</strong> Please review and respond to this certification request within 3 business days.
            </div>

            <div class=""company-info"">
                <h3>Employee Training Department</h3>
                <p style=""color: #666; margin: 5px 0;"">
                    🏢 GyanSys
                </p>
            </div>
        </div>
        <div class=""footer"">
            <p>This request requires admin approval. Please login to the system to take action.</p>
            <p style=""margin-top: 10px;"">© 2025 GyanSys Inc. - Employee Training System</p>
        </div>
    </div>
</body>
</html>";

            return htmlTemplate;
        }
        public string GenerateCertificationResponseTemplate(string employeeName, string certificationType, string adminName, string responseDate, bool isApproved, string comments = "", string nextSteps = "")
        {
            var logo = "https://gyansys.com/wp-content/uploads/2023/06/gyansys.gif";
            var statusColor = isApproved ? "#28a745" : "#dc3545";
            var statusText = isApproved ? "APPROVED" : "REJECTED";
            var statusIcon = isApproved ? "✅" : "❌";
            var headerGradient = isApproved ? "linear-gradient(135deg, #28a745 0%, #20c997 100%)" : "linear-gradient(135deg, #dc3545 0%, #fd7e14 100%)";

            var htmlTemplate = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Certification Request {statusText}</title>
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
            background: {headerGradient};
            color: white;
            padding: 30px 20px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
            font-weight: 300;
        }}
        .status-badge {{
            display: inline-block;
            background-color: rgba(255, 255, 255, 0.2);
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 14px;
            font-weight: bold;
            margin-top: 10px;
        }}
        .content {{
            padding: 30px;
        }}
        .response-details {{
            background-color: #f8f9fa;
            padding: 20px;
            border-left: 4px solid {statusColor};
            margin: 20px 0;
            border-radius: 4px;
        }}
        .detail-row {{
            display: flex;
            justify-content: space-between;
            margin: 10px 0;
            padding: 8px 0;
            border-bottom: 1px solid #e9ecef;
        }}
        .detail-label {{
            font-weight: bold;
            color: #495057;
            flex: 1;
        }}
        .detail-value {{
            color: #333;
            flex: 2;
        }}
        .status-section {{
            text-align: center;
            margin: 30px 0;
            padding: 20px;
            background-color: {(isApproved ? "#d4edda" : "#f8d7da")};
            border: 1px solid {(isApproved ? "#c3e6cb" : "#f5c6cb")};
            border-radius: 8px;
        }}
        .status-icon {{
            font-size: 48px;
            margin: 10px 0;
        }}
        .status-text {{
            font-size: 24px;
            font-weight: bold;
            color: {statusColor};
            margin: 10px 0;
        }}
        .comments-section {{
            background-color: #e9ecef;
            padding: 15px;
            border-radius: 4px;
            margin: 20px 0;
        }}
        .comments-section h4 {{
            margin: 0 0 10px 0;
            color: #495057;
        }}
        .next-steps-section {{
            background-color: {(isApproved ? "#d1ecf1" : "#f8d7da")};
            padding: 15px;
            border-radius: 4px;
            margin: 20px 0;
        }}
        .next-steps-section h4 {{
            margin: 0 0 10px 0;
            color: {statusColor};
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
            color: {statusColor};
            margin: 0 0 10px 0;
            font-size: 18px;
        }}
        .highlight {{
            background-color: {(isApproved ? "#d1ecf1" : "#f8d7da")};
            border: 1px solid {(isApproved ? "#bee5eb" : "#f5c6cb")};
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
            .detail-row {{
                flex-direction: column;
            }}
            .detail-label, .detail-value {{
                flex: none;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""header"">
            <img src='{logo}' alt=""GyanSys Logo"" style=""max-height: 60px; margin-bottom: 15px;"">
             `
            <div class=""status-badge"">{statusIcon} {statusText}</div>
        </div>
        <div class=""content"">
            <h2 style=""color: #333; margin-top: 0;"">Hello {employeeName},</h2>
            
            <div class=""status-section"">
                <div class=""status-icon"">{statusIcon}</div>
                <div class=""status-text"">Request {statusText}</div>
                <p style=""margin: 10px 0; color: #666;"">Your certification request has been {(isApproved ? "approved" : "rejected")} by the administration.</p>
            </div>

            <div class=""response-details"">
                <div class=""detail-row"">
                    <div class=""detail-label"">Certification Type:</div>
                    <div class=""detail-value"">{certificationType}</div>
                </div>
                <div class=""detail-row"">
                    <div class=""detail-label"">Reviewed By:</div>
                    <div class=""detail-value"">{adminName}</div>
                </div>
                <div class=""detail-row"">
                    <div class=""detail-label"">Response Date:</div>
                    <div class=""detail-value"">{responseDate}</div>
                </div>
                <div class=""detail-row"">
                    <div class=""detail-label"">Status:</div>
                    <div class=""detail-value"" style=""color: {statusColor}; font-weight: bold;"">{statusText}</div>
                </div>
            </div>

            {(string.IsNullOrEmpty(comments) ? "" : $@"
            <div class=""comments-section"">
                <h4>💬 Admin Comments</h4>
                <p style=""margin: 0; color: #333;"">{comments}</p>
            </div>")}

            {(string.IsNullOrEmpty(nextSteps) ? "" : $@"
            <div class=""next-steps-section"">
                <h4>{(isApproved ? "🎯 Next Steps" : "🔄 Alternative Options")}</h4>
                <p style=""margin: 0; color: #333;"">{nextSteps}</p>
            </div>")}

            <div class=""highlight"">
                <strong>{(isApproved ? "🎉 Congratulations!" : "📋 Important:")}</strong> 
                {(isApproved ? "You can now proceed with your certification. Please check the learning management system for enrollment details." : "If you have questions about this decision, please contact HR department for further clarification.")}
            </div>

            <div class=""company-info"">
                <h3>Employee Training Department</h3>
                <p style=""color: #666; margin: 5px 0;"">
                    🏢 GyanSys 
                </p>
            </div>
        </div>
        <div class=""footer"">
            <p>This is an automated response notification. For questions, please contact L&D department.</p>
            <p style=""margin-top: 10px;"">© 2025 GyanSys Inc. - Employee Training System</p>
        </div>
    </div>
</body>
</html>";

            return htmlTemplate;
        }


    }
}
