using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface IAttendanceLogService
    {
        Task<bool> LogAttendanceAsync(List<string> userIds, DateTime fromDate, DateTime toDate, TimeSpan checkInTime, TimeSpan checkOutTime, String Description);
        Task<List<AttendenceDto>> GetAttendanceLogsAsync(string userId = null);
        Task<bool> DeleteAttendenceLog(int id);
        Task<IEnumerable<object>> FilterByUserAsync(string? userId);
        Task<bool> ImportFromExcelAsync(IFormFile file);
    }
}
