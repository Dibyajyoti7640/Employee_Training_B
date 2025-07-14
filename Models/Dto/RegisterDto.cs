namespace Employee_Training.Models.Dto
{
    public class RegisterDto
    {
        public int EmpID { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        
    }
}
