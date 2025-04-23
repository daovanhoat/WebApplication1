using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Department")]
    public class DepartmentModel
    {
        [Key] 
        public int DepartmentId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
