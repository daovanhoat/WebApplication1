using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("WorkingInfo")]
    public class WorkingInfoModel
    {
        [Key]
        public int WorkingInfoId { get; set; }
        public string UserId { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime Time {  get; set; }  // ngày bắt đầu làm

        public DateTime? EndDate { get; set; } // ngày kết thúc
    }
}
