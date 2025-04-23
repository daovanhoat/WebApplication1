using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Service;
using WebApplication1.Validation;

namespace WebApplication1.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAnsyc();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] CreatePostDto dto)
        {
            var result = await _userService.CreateUserAnsyc(dto);
            if (result == null)
                return NotFound("Vi tri khong ton tai");
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAnsyc(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, CreatePostDto dto)
        {
            var result = await _userService.UpdateUserAnsyc(id, dto);
            if (!result)
                return BadRequest("Vi tri khong ton tai");
            return NoContent();

        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(String keyword)
        {
            var result = await _userService.SearchUserAnsyc(keyword);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterByDepartment([FromQuery] int? departmentId)
        {
            var result = await _userService.FilterByDepartmentAsync(departmentId);
            return Ok(result);
        }
    }
}