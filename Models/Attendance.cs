using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? EmpId { get; set; }

    public string? FullName { get; set; }

    public string? MeetingDuration { get; set; }

    public string? EmailId { get; set; }
}
