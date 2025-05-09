namespace WebApplication1.Models
{
    public class AttendenceLogRequestDto
    {
        public List<string> UserIds { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public string Description { get; set; }
    }
}
