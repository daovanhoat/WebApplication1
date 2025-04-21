using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
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
