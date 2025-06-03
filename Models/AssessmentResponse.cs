using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class AssessmentResponse
{
    public int ResponseId { get; set; }

    public int? AssessmentId { get; set; }

    public int? UserId { get; set; }

    public string ResponseText { get; set; } = null!;

    public DateTime? SubmittedAt { get; set; }

    public virtual TrainingAssessment? Assessment { get; set; }

    public virtual User? User { get; set; }
}
