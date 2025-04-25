using Microsoft.AspNetCore.Mvc;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/workingInfo")]
    [ApiController]
    public class WorkingInfoController : ControllerBase
    {
        private readonly IWorkingInfoService _workingInfoService;
        
        public WorkingInfoController(IWorkingInfoService workingInfoService)
        {
            _workingInfoService = workingInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkingInfo()
        {
            var result = await _workingInfoService.GetWorkingInfoAsync();
            return Ok(result);
        }
        [HttpGet("filter")]

        public async Task<IActionResult> FilterWorkingInfo([FromQuery] int? UserId)
        {
            var result = await _workingInfoService.FilterWorkingInfoAsync(UserId);
            return Ok(result);
        }

        //[HttpPost("seed-working-info")]
        //public async Task<IActionResult> SeedWorkingInfo()
        //{
        //    var result = await _workingInfoService.SeedWorkingInfoFromUsersAsync();
        //    if (!result) return BadRequest("Seeding failed");
        //    return Ok("Seeding successful");
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateWorkingInfo()
        //{
        //    var result = await _workingInfoService.UpdateWorkingInfoTimeAsync();
        //    return Ok(result);
        //}
    }
}
