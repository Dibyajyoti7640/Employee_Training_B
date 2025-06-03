using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int? GeneratedBy { get; set; }

    public string ReportType { get; set; } = null!;

    public DateTime? ReportDate { get; set; }

    public string ReportUrl { get; set; } = null!;

    public virtual User? GeneratedByNavigation { get; set; }
}
