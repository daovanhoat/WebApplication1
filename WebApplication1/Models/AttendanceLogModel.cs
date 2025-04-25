using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AttendanceLogModel
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public DateTime FromDate {  get; set; }
        public DateTime ToDate { get; set; }

        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }

        public double TotalHours { get; set; }
        public string Description { get; set; }
    }
}
