using System;
using System.Collections.Generic;

namespace RESTapiLibProj.Models;

public partial class Book
{
    public int Id { get; set; }

    public string? BookName { get; set; }

    public int AuthorId { get; set; }

    public int GenreId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
