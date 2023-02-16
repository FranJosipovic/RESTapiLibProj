using System;
using System.Collections.Generic;

namespace RESTapiLibProj.Models;

public partial class Author
{
    public int Id { get; set; }

    public string AuthorName { get; set; } = null!;

    public int YearOfBirth { get; set; }

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
