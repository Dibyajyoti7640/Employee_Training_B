using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Department { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? EmpId { get; set; }

    public virtual ICollection<AssessmentResponse> AssessmentResponses { get; set; } = new List<AssessmentResponse>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual ICollection<TrainingCertificate> TrainingCertificates { get; set; } = new List<TrainingCertificate>();

    public virtual ICollection<TrainingFeedback> TrainingFeedbacks { get; set; } = new List<TrainingFeedback>();

    public virtual ICollection<TrainingProgram> TrainingProgramCreatedByNavigations { get; set; } = new List<TrainingProgram>();

    public virtual ICollection<TrainingProgram> TrainingProgramManagedByNavigations { get; set; } = new List<TrainingProgram>();
}
