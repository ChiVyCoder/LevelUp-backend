using Microsoft.AspNetCore.Mvc;
using LevelUp.Data;
using LevelUp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace LevelUp.Controllers
{
    [Route("api")]
    [ApiController]
    public class InternshipApplicationController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InternshipApplicationController> _logger;

        public InternshipApplicationController(AppDbContext context, ILogger<InternshipApplicationController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("internship-applications/{userId}/{jobId}")]
        public async Task<IActionResult> RegisterInternship(int userId, int jobId, [FromBody] InternshipRegistration application)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { message = $"Người dùng với ID {userId} không tồn tại." });
            }

            var jobExists = await _context.Jobs.AnyAsync(j => j.JobID == jobId);
            if (!jobExists)
            {
                return NotFound(new { message = $"Cơ hội thực tập/việc làm với ID {jobId} không tồn tại." });
            }

            application.UserId = userId;
            application.JobID = jobId;
            application.ApplicationDate = DateTime.UtcNow;
            application.Status = "Pending";

            if (application.DateOfBirth.Kind == DateTimeKind.Unspecified || application.DateOfBirth.Kind == DateTimeKind.Local)
            {
                application.DateOfBirth = application.DateOfBirth.ToUniversalTime();
            }

            ModelState.Remove("User");
            ModelState.Remove("Job");
            ModelState.Remove("ApplicationDate");
            ModelState.Remove("Status");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for internship application.");
                return BadRequest(ModelState);
            }

            try
            {
                _context.InternshipRegistrations.Add(application);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Internship application for JobID {jobId} by UserID {userId} saved successfully. Id: {application.Id}");
                return Ok(new { message = "Đăng ký thực tập thành công!", applicationId = application.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving internship application for JobID {jobId} by UserID {userId}. Inner exception: {ex.InnerException?.Message}");

                if (ex.InnerException?.Message.Contains("duplicate key") == true
                    || ex.InnerException?.Message.Contains("Cannot insert duplicate key") == true
                    || ex.InnerException?.Message.Contains("violates unique constraint") == true)
                {
                    return Conflict(new { message = "Bạn đã đăng ký cơ hội này rồi." });
                }

                return StatusCode(500, new
                {
                    message = "Đã có lỗi xảy ra khi đăng ký thực tập. Vui lòng thử lại sau.",
                    error = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpGet("users/{userId}/internship-applications")]
        public async Task<ActionResult<IEnumerable<InternshipRegistration>>> GetInternshipApplicationsForUser(int userId)
        {
            var applications = await _context.InternshipRegistrations
                                            .Where(ir => ir.UserId == userId)
                                            .Include(ir => ir.Job)
                                            .ToListAsync();

            if (!applications.Any())
            {
                return NotFound(new { message = $"Không tìm thấy đăng ký thực tập nào cho người dùng ID {userId}." });
            }

            var result = applications.Select(app => new
            {
                app.Id,
                app.ApplicationDate,
                app.Status,
                Job = app.Job != null ? new
                {
                    app.Job.JobID,
                    app.Job.CompanyName,
                    app.Job.Location,
                    app.Job.Industry,
                } : null
            }).ToList();

            return Ok(result);
        }
    }
}