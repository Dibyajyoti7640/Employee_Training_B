using Microsoft.AspNetCore.Mvc;
using Employee_Training.Models;
using Employee_Training.Services;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using Employee_Training.Models.Dto;
using Employee_Training_B.Models;


namespace Employee_Training.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly EmpTdsContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(JwtService jwtService, EmpTdsContext context, IConfiguration config, ILogger<AuthController> logger)
        {
            _jwtService = jwtService;
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            // Include additional claims (e.g., userId and role) as needed
            var token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email already exists" });
            }

            var user = new User
            {
                FullName = registerDto.Fullname,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = registerDto.Role,
                Department = registerDto.Department,
                EmpId = registerDto.EmpID
            };

            Console.WriteLine(user);

            _context.Users.Add(user);
            _context.SaveChanges();

            // Return a Created status code with a resource URI if available
            return CreatedAtAction(nameof(Register), new { user.FullName, user.Email });
        }
    }
}
