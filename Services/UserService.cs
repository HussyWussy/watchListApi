using filmizleme.Data;
using filmizleme.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace filmizleme.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(string username, string password, string email)
        {
            if (await _context.Users.AnyAsync(u => u.Username == username))
                throw new Exception("Username already exists");

            var passwordHash = HashPassword(password);
            var user = new User { Username = username, PasswordHash = passwordHash, Email = email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<User> LoginUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null || !VerifyPassword(password, user.PasswordHash))
                throw new Exception("Invalid username or password");

            return user;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }


        internal async Task UpdateUserById(int id, string username, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            if (await _context.Users.AnyAsync(u => u.Username == username && u.Id != id))
            {
                throw new Exception("Username already exists");
            }

            if (!string.IsNullOrEmpty(username))
            {
                user.Username = HashPassword(username);
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                user.PasswordHash = HashPassword(newPassword);
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        internal async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new Exception("User not found");

            return user;
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


        public async Task<List<Film>> GetWatchedFilmsByUser(int userId)
        {

            var filmIds = await _context.WatchedFilms
                .Where(wf => wf.UserId == userId)
                .Select(wf => wf.FilmId)
                .ToListAsync();

            var films = await _context.Films
                .Where(f => filmIds.Contains(f.Id))
                .ToListAsync();

            return films;
        }

    }
}
