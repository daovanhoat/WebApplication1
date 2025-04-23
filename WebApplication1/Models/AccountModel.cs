using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
