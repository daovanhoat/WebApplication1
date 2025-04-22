using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Validation;

namespace WebApplication1.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDBContext _Context;
        public UserController(UserDBContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _Context.Users
                .Include(u => u.Position) // Nạp cả thông tin vị trí
        .ToListAsync();

            var result = users.Select(u => new
            {
                u.UserId,
                u.Name,
                u.Gener,
                u.Age,
                u.Cong,
                u.PositionId,
                positionName = u.Position.Name,   // Đây là tên vị trí
                heSoLuong = u.Position.HeSo
            }).ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] CreatePostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tìm Position theo Id để lấy hệ số
            var position = await _Context.Positions.FindAsync(dto.PositionId);
            if (position == null)
            {
                return NotFound("Vị trí không tồn tại.");
            }

            var user = new UserModel
            {
                Name = dto.Name,
                Gener = dto.Gener,
                Age = dto.Age,
                Cong = dto.Cong,
                PositionId = dto.PositionId,
                // Các trường tính lương hoặc các logic khác có thể xử lý sau
            };

            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();

            return Ok(new
            {
                id = user.UserId,
                user.Name,
                user.Age,
                user.Cong,
                user.PositionId,
                PositionName = user.Position.Name,
                HeSoLuong = position.HeSo
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _Context.Users!.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _Context.Users.Remove(user);
            await _Context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, CreatePostDto user)
        {

            var existingUser = await _Context.Users.Include(u => u.Position).FirstOrDefaultAsync(u => u.UserId == id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Name = user.Name;
            existingUser.Gener = user.Gener;
            existingUser.Age = user.Age;
            existingUser.Cong = user.Cong;

            var position = await _Context.Positions.FindAsync(user.PositionId);
            if (position != null)
            {
                existingUser.Position = position; // Gán vị trí mới cho người dùng
            }
            else
            {
                return BadRequest("Vị trí không tồn tại.");
            }

            try
            {
                await _Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                    return NotFound();
                
            }
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(String keyword)
        {
            var result = await _Context.Users
                .Where(u => u.Name.Contains(keyword))
                .ToListAsync();
            return Ok(result);
        }
    }
}