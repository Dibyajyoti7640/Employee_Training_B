using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class TrainingFeedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public int? ProgramId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public virtual TrainingProgram? Program { get; set; }

    public virtual User? User { get; set; }
}
