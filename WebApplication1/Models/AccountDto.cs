using WebApplication1.Enum;

namespace WebApplication1.Models
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } = "******"; // Ẩn mật khẩu
        public string Email { get; set; } = "hidden@example.com"; // Ẩn email
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; }
        public Role Role { get; set; }
    }
}
