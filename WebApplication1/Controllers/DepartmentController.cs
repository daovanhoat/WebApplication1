using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] DepartmentDto dto)
        {
            var created = await _departmentService.CreateDepartmentAsync(dto);
            return Ok(created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _departmentService.GetAllAsync();
            return Ok(result);
        }
    }
}
