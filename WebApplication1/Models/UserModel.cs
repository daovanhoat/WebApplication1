using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    [Table("User")]
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Gener { get; set; }
        [Range(0, 100)]
        public int Age { get; set; } 
        [Range(0, 30)]
        public int  Cong { get; set; }

        [ForeignKey("Position")]
        public int PositionId { get; set; }

        public PositionModel Position { get; set; }

    }
}
