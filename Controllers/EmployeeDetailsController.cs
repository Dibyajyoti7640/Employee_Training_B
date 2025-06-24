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
            var prorgramNames = new List<string>();
            var userName = await _context.Users.FindAsync(userId);
            var certificates = await _context.Certificates.Where(U=>U.TraineeId == userId).Select(c => new
            {
                c.Title,c.SubmittedOn,c.Id
            }).ToListAsync();
            var registeredPrograms = await _context.Registrations.Where(u=>u.UserId==userId).Select(r=>r.ProgramId).ToListAsync();
            foreach (var programID in  registeredPrograms)
            {
                var programName = await _context.TrainingPrograms.Where(t=>t.ProgramId==programID).Select(c=>c.Title).FirstOrDefaultAsync();
                prorgramNames.Add(programName.ToString());
                Console.WriteLine($"PROGRAM NAME::::::{programName}");
            }
            var result = new
            {
                fullName = userName.FullName,
                email = userName.Email,
                certficateList = certificates,
                programList = prorgramNames.AsEnumerable(),
            };
            return Ok(result);
            

        }
    }
}
