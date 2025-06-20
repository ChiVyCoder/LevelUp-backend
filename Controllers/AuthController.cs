using Microsoft.AspNetCore.Mvc;
using LevelUp.Data;
using LevelUp.Services;
using LevelUp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace LevelUp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;

        public AuthController(AppDbContext context, IConfiguration configuration, IEmailService emailService, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User registerUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Users.AnyAsync(u => u.Email == registerUser.Email))
            {
                return BadRequest(new { message = "Email đã được sử dụng." });
            }

            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(registerUser.Password, salt);

            var newUser = new User
            {
                Username = registerUser.Username,
                Email = registerUser.Email,
                PasswordHash = hashedPassword,
                PasswordSalt = Convert.ToBase64String(salt),
                CreatedAt = DateTime.UtcNow,
                AvatarUrl = "https://blog.cpanel.com/wp-content/uploads/2019/08/user-01.png",
                Bio = "Chưa có mô tả cá nhân."
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            try
            {
                await _emailService.SendRegistrationEmailAsync(newUser.Email, newUser.Username);
                _logger.LogInformation($"Registration email successfully sent to {newUser.Email}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send registration email to {newUser.Email}.");
            }

            var userData = new
            {
                newUser.Id,
                newUser.Email,
                newUser.Username,
                newUser.AvatarUrl
            };

            return Ok(new { message = "Đăng ký thành công.", user = userData });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu." });
            }

            byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt);
            string hashedPassword = HashPassword(loginData.Password, storedSalt);

            if (user.PasswordHash != hashedPassword)
            {
                return Unauthorized(new { message = "Sai email hoặc mật khẩu." });
            }

            var userData = new
            {
                user.Id,
                user.Email,
                user.Username,
                user.AvatarUrl
            };

            return Ok(new { message = "Đăng nhập thành công!", user = userData });
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string HashPassword(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            using (var hmac = new HMACSHA512(salt))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
