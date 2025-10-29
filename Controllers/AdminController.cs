using Microsoft.AspNetCore.Mvc;
using JobCardBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JobCardBackend.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _db;
        public UserController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (user == null)
                return BadRequest(new { message = "Invalid request body" });

            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest(new { message = "Email and Password are required" });

            if (await _db.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest(new { message = "Email already exists" });

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.CompanyName,
                user.ContactNumber
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return BadRequest(new { message = "Email and Password required" });

            var user = _db.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null)
                return BadRequest(new { message = "User not found" });

            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                return BadRequest(new { message = "Incorrect password" });

            return Ok(new
            {
                id = user.Id,
                name = user.Name,
                email = user.Email,
                company = user.CompanyName,
                contact = user.ContactNumber,
                message = "Login successful"
            });
        }
    }
}
