using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int? UserId { get; set; }

    public int? ProgramId { get; set; }

    public string? Status { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public virtual TrainingProgram? Program { get; set; }

    public virtual User? User { get; set; }
}
