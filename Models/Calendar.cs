using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Calendar
{
    public int Id { get; set; }

    public string? MeetingName { get; set; }

    public DateTime? Time { get; set; }

    public string? TeamsLink { get; set; }

    public string? Trainer { get; set; }

    public string? Organiser { get; set; }

    public DateOnly? StartingDate { get; set; }

    public DateOnly? EndingDate { get; set; }

    public string? Venue { get; set; }

    public string? Description { get; set; }

    public DateTime? EndTime { get; set; }

    public int? CourseId { get; set; }
}
