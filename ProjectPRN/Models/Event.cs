using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; }

    public DateTime? EventDate { get; set; }

    public string? EventType { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
