using LevelUp.Models;
using LevelUp.Data; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevelUp.Controllers
{
    [Route("api/volunteer")] 
    [ApiController]
    public class VolunteersApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VolunteersApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Volunteer>>> GetVolunteers()
        {
            // Kiểm tra xem DbSet Volunteers có null không (phòng trường hợp DB chưa được Seed hoặc lỗi cấu hình)
            if (_context.Volunteers == null)
            {
                return NotFound("DbSet 'Volunteers' is null.");
            }

            // Lấy tất cả các bản ghi từ bảng Volunteers một cách bất đồng bộ
            var volunteers = await _context.Volunteers.ToListAsync();

            // Trả về dữ liệu dưới dạng JSON với mã trạng thái 200 OK
            return Ok(volunteers);
        }
    }
}