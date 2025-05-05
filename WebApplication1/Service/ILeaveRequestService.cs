using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface ILeaveRequestService
    {
        Task<bool> SubmitLeaveRequest(LeaveRequestDto dto);
        Task<List<LeaveRequestDto>> GetAllLeaveRequest();
        Task<List<LeaveRequestModel>> GetByUserIdLeaveRequest(string userId);
        //Task<bool> ApproveRequest(int id);
        //Task<bool> RejectRequest(int  id);

        Task<bool> DeleteLeaveRequest(int id);

    }
}
