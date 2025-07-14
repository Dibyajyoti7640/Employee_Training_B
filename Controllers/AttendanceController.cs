using System.Globalization;
using Employee_Training_B.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly EmpTdsContext _context;
        public AttendanceController(EmpTdsContext context)
        {
            _context = context;
        }

        [HttpPost("extract")]
        public async Task<IActionResult> ExtractDetails(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            var records = new List<Attendance>();
            bool participantsSection = false;
            bool headersParsed = false;

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (string.IsNullOrWhiteSpace(line)) continue;

                // Detect start of Participants section
                if (line.Trim().StartsWith("2. Participants"))
                {
                    participantsSection = true;
                    continue;
                }

                // Once inside Participants section, wait for header
                if (participantsSection && !headersParsed && line.StartsWith("Name"))
                {
                    headersParsed = true;
                    continue;
                }

                // Stop if we reach the next section
                if (participantsSection && line.Trim().StartsWith("3."))
                {
                    break;
                }

                if (participantsSection && headersParsed)
                {
                    // Split by tabs (not commas) since the file is tab-delimited
                    var cols = line.Split('\t');

                    // We need at least 6 columns (based on the sample file structure)
                    if (cols.Length < 6) continue;

                    var record = new Attendance
                    {
                        FullName = cols[0].Trim(),
                        EmailId = cols[4].Trim(),
                        MeetingDuration = cols[3].Trim()
                    };

                    // Optional: Look up UserId from database
                    var userId = await _context.Users
                        .Where(u => u.Email == record.EmailId)
                        .Select(u => u.UserId)
                        .FirstOrDefaultAsync();
                    var empId = await _context.Users.Where(u => u.Email == record.EmailId).Select(u => u.EmpId).FirstOrDefaultAsync();
                    Console.WriteLine(userId);
                    record.UserId = userId;
                    record.EmpId = empId;
                    records.Add(record);
                }
            }

            // Save to database if needed
            if (records.Any())
            {
                await _context.Attendances.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }

            return Ok(records);
        }
    }
}