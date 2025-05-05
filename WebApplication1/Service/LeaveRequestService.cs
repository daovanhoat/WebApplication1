using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Enum;
using WebApplication1.Helper;
using WebApplication1.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly UserDBContext _Context;
        private readonly IMapper _mapper;

        public LeaveRequestService(UserDBContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<bool> ApproveRequest(int id)
        {
            var request = await _Context.LeaveRequests.FindAsync(id);
            if (request == null) return false;
            request.IsApproved = true;
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<List<LeaveRequestDto>> GetAllLeaveRequest(
            string? userId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? keyword = null
        )
        {
            var query = from l in _Context.LeaveRequests
                        join u in _Context.Users on l.UserId equals u.UserId
                        select new LeaveRequestDto
                        {
                            Id = l.IdLeaveRequest,
                            UserId = l.UserId,
                            UserName = u.Name,
                            FromDate = l.FromDate,
                            ToDate = l.ToDate,
                            FromTime = l.FromTime,
                            ToTime = l.ToTime,
                            Type = l.Type,
                            Shift = l.Shift,
                            Reason = l.Reason,
                            Description = l.Description,
                            CreatedAt = l.CreatedAt,
                            IsApproved = l.IsApproved,
                        };

            if (!string.IsNullOrEmpty(userId))
            {
                query = query.Where(x => x.UserId == userId);
            }
            // lọc theo thời gian
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.FromDate.Date >=  fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                query = query.Where(x => x.ToDate.Date <= toDate.Value.Date);
            }

            // Thực thi truy vấn
            var data = await query.ToListAsync();

            // Ánh xạ tiếng Việt
            foreach (var item in data)
            {
                item.TypeName = EnumHelper.GetTypeName(item.Type);
                item.ReasonName = EnumHelper.GetReasonName(item.Reason);
                item.ShiftName = item.Type == LeaveType.HalfDay && item.Shift.HasValue
                                            ? EnumHelper.GetShiftName(item.Shift.Value)
                                            : null;
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                var loweredKeyword = keyword.ToLower();
                data = data.Where(x =>
                    (!string.IsNullOrEmpty(x.UserName) && x.UserName.ToLower().Contains(loweredKeyword)) ||
                    (!string.IsNullOrEmpty(x.TypeName) && x.TypeName.ToLower().Contains(loweredKeyword)) ||
                    (!string.IsNullOrEmpty(x.ShiftName) && x.ShiftName.ToLower().Contains(loweredKeyword)) ||
                    (!string.IsNullOrEmpty(x.ReasonName) && x.ReasonName.ToLower().Contains(loweredKeyword))
                ).ToList();
            }

            return data;
        }

        public async Task<bool> RejectRequest(int id)
        {
            var request = await _Context.LeaveRequests.FindAsync(id);
            if (request == null) return false;
            _Context.LeaveRequests.Remove(request);
            await _Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubmitLeaveRequest(LeaveRequestDto dto)
        {
            var user = await _Context.Users
        .FirstOrDefaultAsync(u => u.UserId == dto.UserId); // hoặc u.EmployeeCode == dto.EmployeeCode

            if (user == null)
                throw new Exception("Không tìm thấy nhân viên.");

            var leave = new LeaveRequestModel
            {
                UserId = user.UserId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                FromTime = dto.FromTime,
                ToTime = dto.ToTime,
                Type = dto.Type,
                Reason = dto.Reason,
                Shift = dto.Shift,
                Description = dto.Description
            };

            _Context.LeaveRequests.Add(leave);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
