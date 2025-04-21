using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserDBContext _Context;

        public AccountController(UserDBContext context)
        {
            _Context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(AccountModel model)
        {
            var exist = await _Context.Accounts
                .AnyAsync(a => a.UserName == model.UserName || a.Email == model.Email);
            if (exist)
            {
                return BadRequest("Tai khoan da ton tai");
            }
            model.createAt = DateTime.Now;
            _Context.Accounts.Add(model);
            await _Context.SaveChangesAsync();

            return Ok("Dang ky thanh cong");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountDto dto)
        {
            var account = await _Context.Accounts
                .FirstOrDefaultAsync(a => a.UserName ==  dto.UserName && a.Password == dto.Password);
            if (account == null)
            {
                return Unauthorized("Sai tai khoan hoac mat khau");
            }

            return Ok(new
            {
                message = "Dang nhap thanh cong",
                accountId = account.AccountId,
                userName = account.UserName
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var account = await _Context.Accounts.ToListAsync();
            return Ok(account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccunt (int id)
        {
            var account = await _Context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            _Context.Accounts.Remove(account);
            await _Context.SaveChangesAsync();
            return Ok("Xóa thành công");
        }
    }
}
