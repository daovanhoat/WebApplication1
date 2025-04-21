using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/position")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly UserDBContext _Context;
        public PositionController(UserDBContext context)
        {
            _Context = context;
        }

        [HttpGet] 
        public async Task<IActionResult> getAll()
        {
            var position = await _Context.Positions.ToListAsync();
            return Ok(position);
        }

        [HttpPost]
        public async Task<IActionResult> PostPosition([FromBody] PositionDtocs positionDto)
        {
            var newPosition = new PositionModel
            {
                Name = positionDto.Name,
                HeSo = positionDto.HeSo,
                createAt = DateTime.Now // Tự động gán ngày tạo
            };

            _Context.Positions.Add(newPosition);
            await _Context.SaveChangesAsync();

            // Có thể trả lại DTO hoặc entity tùy bạn muốn
            return Ok(new
            {
                newPosition.PositionId,
                newPosition.Name,
                newPosition.HeSo
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _Context.Positions.FindAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            _Context.Positions.Remove(position);
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, PositionDtocs positionDto)
        {
            var pos = await _Context.Positions.FindAsync(id);
            if (pos == null)
            {
                return NotFound();
            }
            pos.Name = positionDto.Name;
            pos.HeSo = positionDto.HeSo;

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            return Ok("Sua thanh cong");
        }
    }
}
