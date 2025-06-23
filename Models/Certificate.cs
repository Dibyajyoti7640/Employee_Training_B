using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Certificate
{
    public int Id { get; set; }

    public int TraineeId { get; set; }

    public string? Title { get; set; }

    public byte[]? FileContent { get; set; }

    public string? FileName { get; set; }

    public DateTime? SubmittedOn { get; set; }

    public string? Status { get; set; }

    public DateTime? ReviewedOn { get; set; }

    public int? ReviewedBy { get; set; }

    public string? Remarks { get; set; }
}
