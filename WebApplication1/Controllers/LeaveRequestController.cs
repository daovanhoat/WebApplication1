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
        [HttpPost]
        public async Task<IActionResult> submitLeaveRequest([FromBody] LeaveRequestDto dto)
        {
            Console.WriteLine("UserId nhận được: " + dto.UserId);
            var result = await _leaveRequestService.SubmitLeaveRequest(dto);
            return result ? Ok("Đăng ký nghỉ thành công") : BadRequest("Thất bại");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeave(
            [FromQuery] string? userId,
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? keyword
        )
            
        {
            var list = await _leaveRequestService.GetAllLeaveRequest(userId, fromDate, toDate, keyword);
            return Ok(list);
        }

        [HttpPost("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _leaveRequestService.ApproveRequest(id);
            return Ok(result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _leaveRequestService.RejectRequest(id);
            return Ok(result);
        }
    }
}
