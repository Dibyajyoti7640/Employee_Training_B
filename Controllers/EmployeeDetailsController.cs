using Employee_Training_B.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Training_B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetailsController : ControllerBase
    {
        private readonly EmpTdsContext _context;
        public EmployeeDetailsController(EmpTdsContext context)
        {
            _context = context;
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetEmployeeDetails(int userId)
        {
            var programNames = new List<string>();
            var userName = await _context.Users.FindAsync(userId);
            var certificates = await _context.Certificates.Where(U => U.TraineeId == userId && U.Status == "Approved").Select(c => new
            {
                c.Title,
                c.SubmittedOn,
                c.Id,
                c.Status,
            }).ToListAsync();
            var registeredPrograms = await _context.Registrations.Where(u => u.UserId == userId).Select(r => r.ProgramId).ToListAsync();
            foreach (var programID in registeredPrograms)
            {

                var programInfo = await _context.TrainingPrograms
                 .Where(t => t.ProgramId == programID)
                 .Select(t => new { t.Title, t.EndDate,t.StartDate })
                 .FirstOrDefaultAsync();

                if (programInfo != null)
                {
                    var formatted = $"{programInfo.Title}(Starts on:{programInfo.StartDate: dd MMM yyyy}) (Ends on: {programInfo.EndDate:dd MMM yyyy})";
                    programNames.Add(formatted);
                    Console.WriteLine($"PROGRAM NAME::::::{formatted}");

                }
            }
                var result = new
                {
                    fullName = userName.FullName,
                    department = userName.Department,
                    email = userName.Email,
                    certficateList = certificates,
                    programList = programNames.AsEnumerable(),
                };
                return Ok(result);


            }
        }
    }

