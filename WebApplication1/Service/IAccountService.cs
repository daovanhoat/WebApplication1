using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto registerDto);
        Task<AccountDto> LoginAsync(LoginDto loginDto);
        Task<List<AccountDto>> GetAllAsync();
        Task<string> DeleteAccountAsync(int id);
    }
}
