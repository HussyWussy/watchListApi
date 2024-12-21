using filmizleme.Data;
using Microsoft.EntityFrameworkCore;
using filmizleme.Entities;

namespace filmizleme.Services
{
    public class FilmService
    {
        private readonly AppDbContext _context;

        public FilmService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Film>> GetAllFilmsAsync()
        {
            return await _context.Films.ToListAsync();
        }

        public async Task<Film> GetFilmByIdAsync(int id)
        {
            var film = await _context.Films.FindAsync(id);
            if(film==null){
                throw new Exception("Invalid Film Id");
            }
            return film;
        }

        public async Task AddFilmAsync(Film film)
        {
            _context.Films.Add(film);
            await _context.SaveChangesAsync();
        }

        public async Task<int> MarkFilmAsWatched(int userId, int filmId)
        {

            var watchedFilm = new WatchedFilm
            {
                UserId = userId,
                FilmId = filmId,
                WatchedOn = DateTime.Now
            };

            _context.WatchedFilms.Add(watchedFilm);
            await _context.SaveChangesAsync();

            return watchedFilm.Id;
        }

     
    }
}
