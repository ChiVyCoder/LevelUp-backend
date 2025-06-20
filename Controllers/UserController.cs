using LevelUp.Data;
using LevelUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            var userDto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl,
                Bio = user.Bio,
                CreatedAt = user.CreatedAt
            };

            return Ok(userDto);
        }

        [HttpPut("{id}")] // HTTP PUT request, đường dẫn là /api/User/{id}
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserProfileUpdateDto updatedProfile)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng." });
            }

            if (updatedProfile.Username != null)
            {
                user.Username = updatedProfile.Username;
            }
            if (updatedProfile.AvatarUrl != null)
            {
                user.AvatarUrl = updatedProfile.AvatarUrl;
            }
            if (updatedProfile.Bio != null)
            {
                user.Bio = updatedProfile.Bio;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Hồ sơ đã được cập nhật thành công!" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { message = "Lỗi đồng thời khi cập nhật hồ sơ." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        public class UserProfileDto
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string? AvatarUrl { get; set; }
            public string? Bio { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}