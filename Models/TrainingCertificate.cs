using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class TrainingCertificate
{
    public int CertificateId { get; set; }

    public int? UserId { get; set; }

    public int? ProgramId { get; set; }

    public DateTime? IssuedAt { get; set; }

    public string CertificateUrl { get; set; } = null!;

    public virtual TrainingProgram? Program { get; set; }

    public virtual User? User { get; set; }
}
