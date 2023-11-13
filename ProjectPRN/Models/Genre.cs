using System;
using System.Collections.Generic;

namespace ProjectPRN.Models;

public partial class Genre
{
    public int GenreId { get; set; }

    public string GenreName { get; set; } = null!;

    public string? GenreImage { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
