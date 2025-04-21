using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Salary")]
    public class SalaryModels
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int WorkDay { get; set; }
        public decimal SalaryBasic { get; set; }
        public decimal TotalSalary { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public UserModel User { get; set; }
    }
}