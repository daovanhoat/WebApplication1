using WebApplication1.Enum;

namespace WebApplication1.Helper
{
    public static class EnumHelper
    {
        public static string GetReasonName(LeaveReason reason)
        {
            return reason switch
            {
                LeaveReason.Paid => "Có tính công",
                LeaveReason.Unpaid => "Không tính công",
                LeaveReason.Annual => "Phép năm",
                _ => ""
            };
        }

        public static string GetTypeName(LeaveType type)
        {
            return type switch
            {
                LeaveType.FullDay => "Cả ngày",
                LeaveType.HalfDay => "Nửa ngày",
                LeaveType.HourDay => "Theo giờ",
                _ => ""
            };
        }

        public static string GetShiftName(LeaveShift shift)
        {
            return shift switch
            {
                LeaveShift.morning => "sáng",
                LeaveShift.afternoon => "chiều",
                _ => ""
            };
        }
    }
}
