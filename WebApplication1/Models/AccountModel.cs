using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Enum;

namespace WebApplication1.Models
{
    [Table("Account")]
    public class AccountModel
    {
        [Key]
        public int AccountId { get; set; }
        [Required, MaxLength(100)]
        public string UserName { get; set; }
        [Required, MaxLength(100)]
        public string Password { get; set; }
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }
        public DateTime createAt { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public bool IsFirstLogin { get; set; } = true;

        public Role Role { get; set; }
    }
}
