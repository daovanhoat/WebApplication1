namespace WebApplication1.Models
{
    public class AttendenceDto
    {
        public List<string> UserId { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public double TotalHours { get; set; }
        public string Description { get; set; }
    }
}
