using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserDBContext _Context;
        private readonly IMapper _mapper;

        public AccountService(UserDBContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            var exist = await _Context.Accounts
                .AnyAsync(a => a.UserName == registerDto.UserName || a.Email == registerDto.Email);
            if (exist)
            {
                return "Tai khoan da ton tai";
            }
            var account = _mapper.Map<AccountModel>(registerDto);
            account.createAt = DateTime.Now;

            _Context.Accounts.Add(account);
            await _Context.SaveChangesAsync();

            return "Dang ky thanh cong";
        }

        public async Task<AccountDto> LoginAsync(LoginDto loginDto)
        {
            var account = await _Context.Accounts
                .FirstOrDefaultAsync(a => a.UserName == loginDto.UserName && a.Password == loginDto.Password);
            if (account == null)
            {
                return null;
            }

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<List<AccountDto>> GetAllAsync()
        {
            var account = await _Context.Accounts.ToListAsync();
            return _mapper.Map< List<AccountDto>>(account);
        }

        public async Task<string> DeleteAccountAsync(int id)
        {
            var account = await _Context.Accounts.FindAsync(id);
            if (account == null)
            {
                return "Khong tim thay tai khoan";
            }
            _Context.Accounts.Remove(account);
            await _Context.SaveChangesAsync();
            return "Xoa thanh cong";
        }
    }
}
