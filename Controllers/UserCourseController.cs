using Microsoft.AspNetCore.Mvc;
using LevelUp.Data;
using LevelUp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LevelUp.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserCourseController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserCourseController> _logger;

        public UserCourseController(AppDbContext context, ILogger<UserCourseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPut("users/{userId}/courses/{courseId}/finish")]
        public async Task<IActionResult> FinishCourse(int userId, int courseId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { message = $"Người dùng với ID {userId} không tồn tại." });
            }

            var userCourse = await _context.UserCourses
                                            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (userCourse == null)
            {
                userCourse = new UserCourse
                {
                    UserId = userId,
                    CourseId = courseId,
                    EnrolledAt = DateTime.Now,
                    Completed = true
                };
                _context.UserCourses.Add(userCourse);
            }
            else
            {
                if (userCourse.Completed)
                {
                    return Conflict(new { message = "Khóa học này đã được đánh dấu hoàn thành rồi." });
                }
                userCourse.Completed = true;
                _context.UserCourses.Update(userCourse);
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Khóa học đã được đánh dấu hoàn thành!", userId = userId, courseId = courseId, completed = userCourse.Completed, enrolledAt = userCourse.EnrolledAt });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finishing course {courseId} for user {userId}.");
                return StatusCode(500, new { message = "Đã có lỗi xảy ra khi đánh dấu hoàn thành khóa học. Vui lòng thử lại sau.", error = ex.InnerException?.Message ?? ex.Message });
            }
        }



        [HttpGet("users/{userId}/courses/completed")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserCompletedCourses(int userId)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound(new { message = $"Người dùng với ID {userId} không tồn tại." });
            }

            var completedCourses = await _context.UserCourses
                                                 .Where(uc => uc.UserId == userId && uc.Completed)
                                                 // XÓA DÒNG NÀY: .Include(uc => uc.Course)
                                                 .ToListAsync();

            if (completedCourses == null || !completedCourses.Any())
            {
                return NotFound(new { message = $"Không tìm thấy khóa học đã hoàn thành nào cho người dùng ID {userId}." });
            }

            var result = completedCourses.Select(uc => new
            {
                uc.Id,
                uc.UserId,
                uc.CourseId,
                uc.EnrolledAt,
                uc.Completed,
                CourseTitle = (string)null 
                                          
            }).ToList();

            return Ok(result);
        }
    }
}