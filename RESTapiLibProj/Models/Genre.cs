using System;
using System.Collections.Generic;

namespace RESTapiLibProj.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? GenreName { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
