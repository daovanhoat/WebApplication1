using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/attendenceLog")]
    [ApiController]
    public class AttendenceLogController : ControllerBase
    {
       private readonly IAttendanceLogService _attendanceLogService;
       
       public AttendenceLogController(IAttendanceLogService attendanceLogService)
        {
            _attendanceLogService = attendanceLogService;
        }

        public class AttendanceLogRequest
        {
            public List<string> UserIds { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public TimeSpan CheckInTime { get; set; }
            public TimeSpan CheckOutTime { get; set; }

            public string Description { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceLogs()
        {
            var logs = await _attendanceLogService.GetAttendanceLogsAsync();
            return Ok(logs);
        }

        [HttpPost]
        public async Task<IActionResult> LogAttendance([FromBody] AttendanceLogRequest request)
        {
            var result = await _attendanceLogService.LogAttendanceAsync(
                request.UserIds,
                request.FromDate,
                request.ToDate,
                request.CheckInTime,
                request.CheckOutTime,
                request.Description
            );

            if (result)
                return Ok(new { success = true, message = "Chấm công thành công" });
            else
                return BadRequest(new { success = false, message = "Chấm công thất bại" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendenceLog(int id)
        {
            var result = await _attendanceLogService.DeleteAttendenceLog(id);
            if(!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterAttendence([FromQuery] string? userId)
        {
            var result = await _attendanceLogService.FilterByUserAsync(userId);
            return Ok(result);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFromExcelAsync(IFormFile file)
        {
            var result = await _attendanceLogService.ImportFromExcelAsync(file);

            if (!result)
            {
                // Tìm file lỗi mới nhất trong thư mục
                var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "errors"));
                var fileError = dir.GetFiles("ImportErrors_*.xlsx")
                    .OrderByDescending(f => f.CreationTime)
                    .FirstOrDefault();

                if (fileError != null)
                {
                    var fileBytes = await System.IO.File.ReadAllBytesAsync(fileError.FullName);
                    return File(fileBytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Import_Errors.xlsx");
                }

                return BadRequest("Có lỗi khi xử lý file, nhưng không tìm thấy file lỗi.");
            }
            return Ok("Import thanh cong");
        }
    }
}
