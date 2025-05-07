using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/workingInfo")]
    [ApiController]
    public class WorkingInfoController : ControllerBase
    {
        private readonly IWorkingInfoService _workingInfoService;
        private readonly IAccountService _accountService;
        
        public WorkingInfoController(IWorkingInfoService workingInfoService, IAccountService accountService)
        {
            _workingInfoService = workingInfoService;
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet("working-info")]
        public async Task<IActionResult> GetWorkingInfo()
        {
            // Lấy role và userId từ claims
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(role))
                return Unauthorized("Không xác định được quyền truy cập.");

            if (role == "Admin")
            {
                var all = await _workingInfoService.GetWorkingInfoAsync();
                return Ok(all);
            }

            // Với role không phải Admin, thì bắt buộc phải có userId
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Không xác định được người dùng.");

            var mine = await _workingInfoService.GetWorkingInfoAsync(userId);
            return Ok(mine);
        }


        [HttpGet("filter")]

        public async Task<IActionResult> FilterWorkingInfo([FromQuery] string? UserId)
        {
            var result = await _workingInfoService.FilterWorkingInfoAsync(UserId);
            return Ok(result);
        }
    }
}
