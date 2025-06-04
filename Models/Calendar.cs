using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Calendar
{
    public int Id { get; set; }

    public string? MeetingName { get; set; }

    public DateTime? Time { get; set; }

    public string? TeamsLink { get; set; }
}
