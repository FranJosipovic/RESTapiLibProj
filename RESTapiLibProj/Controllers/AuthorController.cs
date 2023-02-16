using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapiLibProj.Models;

namespace RESTapiLibProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        private readonly BooksLibraryContext dbContext;

        public AuthorController(BooksLibraryContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var result = await dbContext.Authors.ToListAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var result = await dbContext.Authors.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAuthor(string name, int year)
        {
            var newItem = new Author() { AuthorName = name, YearOfBirth = year };
            dbContext.Authors.Add(newItem);
            await dbContext.SaveChangesAsync();
            return Ok(newItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAuthor(int id, string? name, int year)
        {
            var author = await dbContext.Authors.FindAsync(id);

            if (author == null) { return NotFound(); }

            author.AuthorName = name ?? author.AuthorName;

            author.YearOfBirth = year > 0 ? year : author.YearOfBirth;

            await dbContext.SaveChangesAsync();

            return Ok(author);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await dbContext.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            dbContext.Authors.Remove(author);
            await dbContext.SaveChangesAsync();
            return Ok(author);
        }
    }
}
