﻿using WebApplication1.Enum;

namespace WebApplication1.Models
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public TimeSpan? ToTime { get; set; }
        public LeaveType Type { get; set; }

        public LeaveShift? Shift { get; set; }
        public LeaveReason Reason { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ShiftName { get; set; } = "";
        public string TypeName { get; set; } = "";
        public string ReasonName { get; set; } = "";
        public bool IsApproved { get; set; } = false;
    }
}
