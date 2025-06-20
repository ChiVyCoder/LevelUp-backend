using LevelUp.Data; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevelUp.Models;

namespace LevelUpDB.Controllers 
{
    [ApiController] 
    [Route("api/job")] 
    public class JobsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public JobsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            // Lấy tất cả các công việc từ bảng Jobs
            var jobs = await _context.Jobs.ToListAsync();
            return Ok(jobs); 
        }
    }
}