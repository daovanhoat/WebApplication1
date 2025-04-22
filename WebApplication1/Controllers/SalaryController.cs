using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/salary")]
    [ApiController]
    public class SalaryController : ControllerBase
    {
        private readonly ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _salaryService.GetAllAsync();
            return Ok(result);
        }
        [HttpPost("Calculate")]
        public async Task<IActionResult> CalculateSalary([FromBody] SalaryDto dto)
        {
            var result = await _salaryService.CalculateSalaryAsync(dto);
            if (result == null) return NotFound("Nhân viên không tồn tại");
            return Ok(result);
        }
    
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _salaryService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}