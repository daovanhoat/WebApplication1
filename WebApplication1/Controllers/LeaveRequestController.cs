using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/leavequest")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> submitLeaveRequest([FromBody] LeaveRequestDto dto)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userID = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(role))
                return Unauthorized("Không xác định được người dùng hoặc vai trò");
            if(role != "Admin")
            {
                dto.UserId = userID;
            }

            var result = await _leaveRequestService.SubmitLeaveRequest(dto);
            return result ? Ok("Đăng ký nghỉ thành công") : BadRequest("Thất bại");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllLeave(
            [FromQuery] string? userId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? keyword
        )
            
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userID = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(role))
                return Unauthorized("Không xác định được quyền truy cập.");
            if(role == "Admin")
            {
                var all = await _leaveRequestService.GetAllLeaveRequest(userId, fromDate, toDate, keyword);
                return Ok(all);
            }
            if (string.IsNullOrEmpty(userID))
                return Unauthorized("Không xác định được người dùng.");
            var list = await _leaveRequestService.GetAllLeaveRequest(userID, fromDate, toDate, keyword);
            return Ok(list);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
                var result = await _leaveRequestService.ApproveRequest(id);
                return Ok(result);          
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _leaveRequestService.RejectRequest(id);
            return Ok(result);
        }
    }
}
