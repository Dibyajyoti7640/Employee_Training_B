using System;
using System.Collections.Generic;

namespace Employee_Training_B.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int ResortId { get; set; }

    public int? Capacity { get; set; }

    public bool? SmokingAllowed { get; set; }

    public bool? Balcony { get; set; }

    public bool? PrivateSwimmingPool { get; set; }

    public bool? Booked { get; set; }

    public decimal? CarpetArea { get; set; }

    public decimal? Price { get; set; }

    public virtual Resort Resort { get; set; } = null!;
}
