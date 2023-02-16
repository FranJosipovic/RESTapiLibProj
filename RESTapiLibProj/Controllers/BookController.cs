using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapiLibProj.Models;

namespace RESTapiLibProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BooksLibraryContext dbContext;

        public BookController(BooksLibraryContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await dbContext.Books.Include(book => book.Author).Include(book => book.Genre).Select(book =>
            new BookDto()
            {
                Id = book.Id,
                Title = book.BookName,
                CreatedAt = book.CreatedAt,
                Author = new AuthorDto()
                {
                    Name = book.Author.AuthorName,
                    YearOfBirth = book.Author.YearOfBirth
                },
                Genre = book.Genre.GenreName
            }).ToListAsync();

            return Ok(books);
        }

        [HttpGet("id")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await dbContext.Books.Include(book => book.Author).Include(book => book.Genre).Select(book =>
            new BookDto()
            {
                Id = book.Id,
                Title = book.BookName,
                CreatedAt = book.CreatedAt,
                Author = new AuthorDto()
                {
                    Name = book.Author.AuthorName,
                    YearOfBirth = book.Author.YearOfBirth
                },
                Genre = book.Genre.GenreName
            }).SingleOrDefaultAsync(book => book.Id == id);

            if (book == null) { return NotFound(); }

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBook(string bookName, int authorId, int genreId, DateTime CreatedAt)
        {
            bool authorExists = await Exists(dbContext.Authors,authorId);
            if (!authorExists) 
            {
                return BadRequest("Author doesn't exist");
            }
            bool genreExists = await Exists(dbContext.Genres,genreId);
            if (!genreExists)
            {
                return BadRequest("Genre doesn't exist");
            }
            if(CreatedAt > DateTime.UtcNow)
            {
                return BadRequest("Can't be created in future");
            }
            var book = new Book() { BookName = bookName, AuthorId = authorId, GenreId = genreId, CreatedAt = CreatedAt };
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();

            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, string? name, int? authorId, int? genreId, DateTime? createdAt)
        {
            var book = await dbContext.Books.FindAsync(id);

            if (book == null) { return NotFound(); }

            if (authorId != null)
            {
                bool exists = await Exists(dbContext.Authors, (int)authorId);
                if (exists)
                {
                    book.AuthorId = (int)authorId;
                }
                else
                {
                    return BadRequest("Author doesn't exist");
                }
            }

            if (genreId != null)
            {
                bool exists = await Exists(dbContext.Genres, (int)genreId);
                if (exists)
                {
                    book.AuthorId = (int)genreId;
                }
                else
                {
                    return BadRequest("Genre doesn't exist");
                }
            }

            if(createdAt != null)
            {
                if(createdAt > DateTime.Now)
                {
                    return BadRequest("Can't be created in future");
                }
                else
                {
                    book.CreatedAt = (DateTime)createdAt;
                }
            }
            
            book.BookName = name ?? book.BookName;

            await dbContext.SaveChangesAsync();

            return Ok(book);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBook(int id)
        {
            var book = await dbContext.Books.FindAsync(id);
            if (book == null) { return NotFound(); }
            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();
            return Ok(book);
        }

        private async Task<bool> Exists<T>(DbSet<T> dbSet,int id) where T : class
        {
            return await dbSet.FindAsync(id) != null;
        }
    }
}
