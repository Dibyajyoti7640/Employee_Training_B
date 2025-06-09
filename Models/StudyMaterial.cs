using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class StudyMaterial
{
    public int Id { get; set; }

    public string? DocumentName { get; set; }

    public byte[]? Content { get; set; }

    public int? CourseId { get; set; }
}
