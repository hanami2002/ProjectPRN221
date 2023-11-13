using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Role { get; set; }

    public string Username { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
