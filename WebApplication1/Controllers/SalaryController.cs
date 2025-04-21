using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/salary")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly UserDBContext _Context;
        public SalaryController(UserDBContext context)
        {
            _Context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var salaries = await _Context.Salaries
                .Include(s => s.User)
                .Select(s => new
                {
                    s.Id,
                    s.UserId,
                    UserName = s.User.Name,
                    s.SalaryBasic,
                    s.WorkDay,
                    s.TotalSalary,
                    s.CreateDate
                })
                .ToListAsync();
            return Ok(salaries);
        }
        [HttpPost("Calculate")]
        public async Task<IActionResult> CalculateSalary([FromBody] SalaryDto model)
        {
            var user = await _Context.Users
                .Include(u => u.Position) // Include để lấy hệ số lương
                .FirstOrDefaultAsync(u => u.UserId == model.UserId);

            if (user == null)
            {
                return NotFound("Nhân viên không tồn tại");
            }

            var workDay = user.Cong;
            var heSo = user.Position.HeSo;
            var totalSalary = workDay * model.SalaryBasic * heSo;

            var salary = new SalaryModels
            {
                UserId = model.UserId,
                WorkDay = workDay,
                SalaryBasic = model.SalaryBasic,
                TotalSalary = totalSalary,
                CreateDate = DateTime.Now
            };

            _Context.Salaries.Add(salary);
            await _Context.SaveChangesAsync();

            return Ok(salary);
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var salary = await _Context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            _Context.Salaries.Remove(salary);
            await _Context.SaveChangesAsync();
            return NoContent();
        }
    }
}