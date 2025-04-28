
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AttendenceLogService : IAttendanceLogService
    {
        private readonly UserDBContext _Context;

        public AttendenceLogService(UserDBContext context)
        {
            _Context = context;
        }

        public async Task<bool> DeleteAttendenceLog(int id)
        {
            var attendence = await _Context.AttendanceLogs.FindAsync(id);
            if(attendence == null) return false;
            _Context.AttendanceLogs.Remove(attendence);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<object>> FilterByUserAsync(string? userId)
        {
            var query = from a in _Context.AttendanceLogs
                        join u in _Context.Users on a.userId equals u.UserId
                        select new 
                        {
                            u.UserId,
                            UserName = u.Name,
                            a.FromDate,
                            a.ToDate,
                            a.CheckInTime,
                            a.CheckOutTime,
                            a.TotalHours,
                            a.Description
                        };
            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }
            return await query.ToListAsync();

        }

        public async Task<List<AttendenceDto>> GetAttendanceLogsAsync()
        {
            var result = await (from log in _Context.AttendanceLogs
                                join user in _Context.Users on log.userId equals user.UserId
                                select new AttendenceDto
                                {
                                    UserId = new List<string> { log.userId },
                                    UserName = user.Name,
                                    Id = log.Id,
                                    FromDate = log.FromDate,
                                    ToDate = log.ToDate,
                                    CheckInTime = log.CheckInTime,
                                    CheckOutTime = log.CheckOutTime,
                                    TotalHours = log.TotalHours,
                                    Description = log.Description
                                })
                    .OrderByDescending(x => x.FromDate)
                    .ToListAsync();
            return result;
        }

        public async Task<bool> ImportFromExcelAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Nguyen Van A");
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets[0];
            var rowCount = workSheet.Dimension.Rows;

            var userDictionary = _Context.Users.ToDictionary(u => u.UserId, u => u.Name);

            var validAttendances = new List<AttendanceLogModel>();
            bool hasError = false;

            // Thêm cột "Lỗi" nếu chưa có
            workSheet.Cells[1, 8].Value = "Lỗi";


            for (int row = 2; row <= rowCount; row++)
            {
                string error = "";

                var userId = workSheet.Cells[row, 1].Text.Trim();
                var userNameFromExcel = workSheet.Cells[row, 2].Text.Trim();

                if (!userDictionary.TryGetValue(userId, out var actualUserName))
                {
                    error += "Không tìm thấy mã nhân viên; ";
                }
                else if (!string.Equals(userNameFromExcel, actualUserName, StringComparison.OrdinalIgnoreCase))
                {
                    error += $"Tên nhân viên không khớp (Excel='{userNameFromExcel}' DB='{actualUserName}'); ";
                }

                if (!DateTime.TryParse(workSheet.Cells[row, 3].Text, out var fromDate))
                {
                    error += "Từ ngày sai định dạng hoặc trống; ";
                }
                if (!DateTime.TryParse(workSheet.Cells[row, 4].Text, out var toDate))
                {
                    error += "Đến ngày sai định dạng hoặc trống; ";
                }
                if (!DateTime.TryParse(workSheet.Cells[row, 5].Text, out var checkInTime))
                {
                    error += "Giờ vào sai định dạng hoặc trống; ";
                }
                if (!DateTime.TryParse(workSheet.Cells[row, 6].Text, out var checkOutTime))
                {
                    error += "Giờ ra sai định dạng hoặc trống; ";
                }

                // Nếu không có lỗi định dạng ngày giờ, mới kiểm tra trùng khoảng thời gian
                if (string.IsNullOrEmpty(error))
                {
                    bool isDuplicate = await _Context.AttendanceLogs
                        .AnyAsync(a => a.userId == userId &&
                                       ((fromDate >= a.FromDate && fromDate <= a.ToDate) ||
                                        (toDate >= a.FromDate && toDate <= a.ToDate) ||
                                        (fromDate <= a.FromDate && toDate >= a.ToDate)));
                    if (isDuplicate)
                    {
                        error += "Khoảng thời gian bị trùng; ";
                    }
                }

                if (!string.IsNullOrEmpty(error))
                {
                    hasError = true;
                    workSheet.Cells[row, 8].Value = error.Trim();
                    continue;
                }

                // Tính công nếu không có lỗi
                double totalHours = 0;
                if (checkInTime.TimeOfDay == TimeSpan.Zero || checkOutTime.TimeOfDay == TimeSpan.Zero)
                {
                    totalHours = 0;
                }
                else
                {
                    var workingHours = (checkOutTime - checkInTime).TotalHours - 1.5;
                    if (workingHours >= 8)
                        totalHours = 1.0;
                    else if (workingHours > 0)
                        totalHours = 0.5;
                    else
                        totalHours = 0;
                }

                var attendance = new AttendanceLogModel
                {
                    userId = userId,
                    FromDate = fromDate,
                    ToDate = toDate,
                    CheckInTime = checkInTime,
                    CheckOutTime = checkOutTime,
                    TotalHours = totalHours,
                    Description = workSheet.Cells[row, 7].Text.Trim()
                };

                _Context.AttendanceLogs.Add(attendance);
            }

            if (hasError)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "errors");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, $"ImportErrors_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                File.WriteAllBytes(filePath, package.GetAsByteArray());

                return false; // báo về controller rằng có lỗi
            }

            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> LogAttendanceAsync(List<string> userIds, DateTime fromDate, DateTime toDate, TimeSpan checkInTime, TimeSpan checkOutTime, String Description)
        {
            //chỉ tính công cho những nhân viên còn làm việc

            var totalDays = (toDate - fromDate).Days + 1;

            foreach (var userId in userIds)
            {
                for (int i = 0; i < totalDays; i++)
                {
                    var currentDate = fromDate.AddDays(i);

                    var checkIn = currentDate.Date.Add(checkInTime);
                    var checkOut = currentDate.Date.Add(checkOutTime);

                    if (checkInTime == TimeSpan.Zero || checkOutTime == TimeSpan.Zero)
                    {
                        // Không có giờ vào hoặc ra -> 0 công
                        continue;
                    }

                    var workingHours = (checkOut - checkIn).TotalHours - 1.5; // Trừ nghỉ trưa 1 tiếng 30 phút

                    double totalHours = 0;
                    if (workingHours >= 8)
                    {
                        totalHours = 1.0;
                    }
                    else if (workingHours > 0)
                    {
                        totalHours = 0.5;
                    }
                    else
                    {
                        totalHours = 0;
                    }

                    var log = new AttendanceLogModel
                    {
                        userId = userId,
                        FromDate = currentDate,
                        ToDate = currentDate,
                        CheckInTime = checkIn,
                        CheckOutTime = checkOut,
                        TotalHours = totalHours,
                        Description = Description
                    };

                    _Context.AttendanceLogs.Add(log);
                }
            }

            await _Context.SaveChangesAsync();
            return true;
        }

    }
}
