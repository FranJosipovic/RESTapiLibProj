using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTapiLibProj.Models;

namespace RESTapiLibProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly BooksLibraryContext dbContext;

        public GenreController(BooksLibraryContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            var result = await dbContext.Genres.ToListAsync();
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var result = await dbContext.Genres.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenre(string name)
        {
            var newItem = new Genre() { GenreName = name };

            dbContext.Genres.Add(newItem);

            await dbContext.SaveChangesAsync();

            return Ok(newItem);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGenre(int id, string name)
        {
            var genre = await dbContext.Genres.FindAsync(id);

            if (genre == null) { return NotFound(); }

            genre.GenreName = name;

            await dbContext.SaveChangesAsync();
            return Ok(genre);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            dbContext.Genres.Remove(genre);
            await dbContext.SaveChangesAsync();
            return Ok(genre);
        }
    }
}
