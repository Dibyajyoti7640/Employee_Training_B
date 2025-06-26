namespace Employee_Training.Models.Dto
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public int EmpID { get; set; }
    }
}
