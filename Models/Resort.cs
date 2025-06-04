using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Resort
{
    public int ResortId { get; set; }

    public string Name { get; set; } = null!;

    public string City { get; set; } = null!;

    public decimal? Rating { get; set; }

    public int? Stars { get; set; }

    public decimal? DistFromCenter { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
