using filmizleme.Services;
using Microsoft.AspNetCore.Mvc;

namespace filmizleme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("users")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            try
            {
                var user = await _userService.RegisterUserAsync(dto.Username, dto.Password, dto.Email);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("users/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                var user = await _userService.LoginUserAsync(dto.Username, dto.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id,string username,string password)
        {
            await _userService.UpdateUserById(id, username, password);
            return Ok();
        }

        [HttpGet("api/users/{id}/watched")]
        public async Task<IActionResult> GetWatchedFilms(int id)
        {
            var films = await _userService.GetWatchedFilmsByUser(id);
            return  Ok(films);
        }

        [HttpPost("api/users/{id}/watched/{filmId}")]
        public async Task<IActionResult> SetUserFilmWatched(int id, int filmId)
        {
            var film = await _userService.MarkFilmAsWatched(id,filmId);
            return Ok(film);
        }

       

    }

    public record UserRegistrationDto(string Username, string Password, string Email);
    public record UserLoginDto(string Username, string Password);
}
