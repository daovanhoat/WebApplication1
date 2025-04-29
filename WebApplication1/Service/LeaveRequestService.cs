using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

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

        public async Task<List<LeaveRequestDto>> GetAllLeaveRequest()
        {
            var result = await (from l in _Context.LeaveRequests
                                join u in _Context.Users on l.UserId equals u.UserId
                                select new LeaveRequestDto
                                {
                                    UserId = l.UserId,
                                    UserName = u.Name,
                                    FromDate = l.FromDate,
                                    ToDate = l.ToDate,
                                    FromTime = l.FromTime,
                                    ToTime = l.ToTime,
                                    Type = l.Type,
                                    Reason = l.Reason,
                                    Description = l.Description,
                                    CreatedAt = l.CreatedAt,
                                    IsApproved = l.IsApproved,
                                })
                                .ToListAsync();
            return result;
        }

        public async Task<List<LeaveRequestModel>> GetByUserIdLeaveRequest(string userId)
        {
            return await _Context.LeaveRequests.Where(l => l.UserId == userId).ToListAsync();
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
                throw new Exception("Không tìm thấy người dùng.");

            var leave = new LeaveRequestModel
            {
                UserId = user.UserId,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                FromTime = dto.FromTime,
                ToTime = dto.ToTime,
                Type = dto.Type,
                Reason = dto.Reason,
                Description = dto.Description
            };

            _Context.LeaveRequests.Add(leave);
            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
