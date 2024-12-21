using filmizleme.Entities;
using filmizleme.Services;
using Microsoft.AspNetCore.Mvc;

namespace filmizleme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class FilmController : ControllerBase
    {
        private readonly FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpGet("films")]
        public async Task<IActionResult> GetFilms()
        {
            var films = await _filmService.GetAllFilmsAsync();
            return Ok(films);
        }

        [HttpGet("films/{id}")]
        public async Task<IActionResult> GetFilm(int id)
        {
            var film = await _filmService.GetFilmByIdAsync(id);
            if (film == null) return NotFound();
            return Ok(film);
        }

        [HttpPost("films")]
        public async Task<IActionResult> AddFilm(Film film)
        {
            await _filmService.AddFilmAsync(film);
            return CreatedAtAction(nameof(GetFilm), new { id = film.Id }, film);
        }

        [HttpPost("films/{id}/watched")]
         public async Task<IActionResult> SetFilmWatched(int id, int userId)
        {
           var film = await _filmService.MarkFilmAsWatched(userId,id);
           return Ok(film);
        }
        
    }
}
