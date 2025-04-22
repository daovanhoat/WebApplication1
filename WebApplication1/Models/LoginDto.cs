using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
