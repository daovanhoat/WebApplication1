using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("Position")]
    public class PositionModel
    {
        [Key]
        public int PositionId { get; set; }
        public string Name { get; set; }
        public decimal HeSo { get; set; }
        public DateTime createAt { get; set; }

        public ICollection<UserModel> User { get; set; }

    }
}
