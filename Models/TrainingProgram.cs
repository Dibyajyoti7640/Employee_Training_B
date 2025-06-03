using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class TrainingProgram
{
    public int ProgramId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Trainer { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Mode { get; set; } = null!;

    public int DurationHours { get; set; }

    public int? MaxParticipants { get; set; }

    public string? Category { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? ManagedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ManagedByNavigation { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<TrainingAssessment> TrainingAssessments { get; set; } = new List<TrainingAssessment>();

    public virtual ICollection<TrainingCertificate> TrainingCertificates { get; set; } = new List<TrainingCertificate>();

    public virtual ICollection<TrainingFeedback> TrainingFeedbacks { get; set; } = new List<TrainingFeedback>();
}
