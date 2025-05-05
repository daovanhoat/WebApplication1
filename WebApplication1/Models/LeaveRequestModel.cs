using System.ComponentModel.DataAnnotations;
using WebApplication1.Enum;

namespace WebApplication1.Models
{
    public class LeaveRequestModel
    {
        [Key]
        public int IdLeaveRequest { get; set; }
        public string UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public LeaveType Type { get; set; }
        public LeaveShift? Shift { get; set; }
        public LeaveReason Reason { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsApproved { get; set; } = false;
    }
}
