namespace RESTapiLibProj.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuthorDto Author { get; set; }
        public string Genre { get; set; }   
    }
}
