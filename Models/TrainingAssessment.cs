using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class TrainingAssessment
{
    public int AssessmentId { get; set; }

    public int? ProgramId { get; set; }

    public string QuestionText { get; set; } = null!;

    public string AnswerType { get; set; } = null!;

    public virtual ICollection<AssessmentResponse> AssessmentResponses { get; set; } = new List<AssessmentResponse>();

    public virtual TrainingProgram? Program { get; set; }
}
