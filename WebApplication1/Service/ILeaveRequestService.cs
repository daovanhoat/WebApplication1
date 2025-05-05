using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface ILeaveRequestService
    {
        Task<bool> SubmitLeaveRequest(LeaveRequestDto dto);
        Task<List<LeaveRequestDto>> GetAllLeaveRequest(
            string? userId = null, 
            DateTime? fromDate = null, 
            DateTime? toDate = null, 
            string? keyword = null
            );
        Task<bool> ApproveRequest(int id);
        Task<bool> RejectRequest(int id);

    }
}
