using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Enum;
using WebApplication1.Helper;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserDBContext _Context;
        private readonly IMapper _mapper;
        private readonly tokenHelper _tokenHelper;

        public AccountService(UserDBContext context, IMapper mapper, tokenHelper tokenHelper)
        {
            _Context = context;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
        }

        public async Task<string> RegisterAsync(RegisterDto registerDto)
        {
            // Kiểm tra xem UserId có tồn tại trong bảng User không
            var userExists = await _Context.Users
                .AnyAsync(u => u.UserId == registerDto.UserId);

            if (!userExists)
                return "Mã nhân viên không tồn tại";

            // Kiểm tra xem tài khoản đã tồn tại chưa
            var accountExists = await _Context.Accounts
                .AnyAsync(a => a.UserName == registerDto.UserId || a.Email == registerDto.Email);

            if (accountExists)
                return "Tài khoản đã tồn tại";

            // Tạo tài khoản mới
            var account = new AccountModel
            {
                UserName = registerDto.UserId,
                Password = registerDto.Password, 
                Email = registerDto.Email,
                UserId = registerDto.UserId,
                Role = Role.User, // mặc định là User
                createAt = DateTime.Now,
                IsFirstLogin = true,
            };

            _Context.Accounts.Add(account);
            await _Context.SaveChangesAsync();
            return "Đăng ký thành công";
        }

        public async Task<AccountDto> LoginAsync(LoginDto loginDto)
        {
            var account = await _Context.Accounts
                .FirstOrDefaultAsync(a => a.UserName == loginDto.UserName && a.Password == loginDto.Password);
            if (account == null)
            {
                return null;
            }

            // Lấy UserId từ bảng Users (nếu có)
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.AccountId == account.AccountId);
            if (user != null)
            {
                account.UserId = user.UserId; // ← Gán lại UserId đúng từ bảng Users
            }

            var accountDto = _mapper.Map<AccountDto>(account);
            accountDto.Token = _tokenHelper.GenerateJwtToken(account); // Tạo JWT và gán vào DTO

            return accountDto;
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
