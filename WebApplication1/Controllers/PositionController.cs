using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

        [HttpGet] 
        public async Task<IActionResult> getAll()
        {
            var result = await _positionService.GetAllAnsyc();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostPosition([FromBody] PositionDtocs dto)
        {
            var created = await _positionService.AddAnsyc(dto);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var success = await _positionService.DeleteAnsyc(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionDtocs dto)
        {
            var success = await _positionService.UpdateAnsyc(id, dto);
            if (!success) return NotFound("Không tìm thấy vị trí");
            return Ok("Sửa thành công");
        }
    }
}
