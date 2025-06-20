// LevelUp/Controllers/VolunteerApplicationController.cs
using Microsoft.AspNetCore.Mvc;
using LevelUp.Data;
using LevelUp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore; 

namespace LevelUp.Controllers
{
    [Route("api")]
    [ApiController]
    public class VolunteerApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VolunteerApplicationController> _logger;

        public VolunteerApplicationController(AppDbContext context, ILogger<VolunteerApplicationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Endpoint ĐĂNG KÝ (giữ nguyên cho demo)
        [HttpPost("volunteer-applications/{userId}/{volunteerId}")]
        public async Task<IActionResult> RegisterVolunteer(int userId, int volunteerId, [FromBody] VolunteerApplication application)
        {
            application.UserId = userId;
            application.VolunteerID = volunteerId;

            ModelState.Remove("User");
            ModelState.Remove("Volunteer");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for volunteer application.");
                return BadRequest(ModelState);
            }

            application.ApplicationDate = DateTime.UtcNow;

            try
            {
                _context.VolunteerApplications.Add(application);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Volunteer application for VolunteerID {volunteerId} by UserID {userId} saved successfully. Id: {application.Id}");
                return Ok(new { message = "Đăng ký tình nguyện thành công!", applicationId = application.Id });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"Error saving volunteer application for VolunteerID {volunteerId} by UserID {userId}.");
                return StatusCode(500, new { message = "Đã có lỗi xảy ra khi đăng ký tình nguyện. Vui lòng thử lại sau.", error = ex.Message });
            }
        }

        [HttpGet("users/{userId}/volunteer-applications")]
        public async Task<ActionResult<IEnumerable<VolunteerApplication>>> GetVolunteerApplicationsForUser(int userId)
        {
            var applications = await _context.VolunteerApplications
                                             .Where(va => va.UserId == userId)
                                             .Include(va => va.Volunteer) 
                                             .ToListAsync();

            if (applications == null || !applications.Any())
            {
                return NotFound(new { message = $"Không tìm thấy đăng ký tình nguyện nào cho người dùng ID {userId}." });
            }
            var result = applications.Select(app => new
            {
                app.Id,
                app.ApplicationDate, 
                Volunteer = new 
                {
                    app.Volunteer.VolunteerID,
                    app.Volunteer.Title,
                    app.Volunteer.Location,
                    app.Volunteer.Industry,
                    app.Volunteer.Type,
                    // Thêm các trường khác của Volunteer mà bạn muốn hiển thị
                }
            }).ToList();

            return Ok(result);
        }
    }
}