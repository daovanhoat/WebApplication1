using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using AutoMapper;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _accountService.RegisterAsync(registerDto);
            if(result == "Tai khoan da ton tai")
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var account = await _accountService.LoginAsync(dto);
            if (account == null)
                return Unauthorized("Sai tai khoan hoac mat khau");
            return Ok(account);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _accountService.GetAllAsync();
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccunt (int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (result == "Khong tim thay tai khoan")
                return NotFound();
            return Ok(result);
            
        }
    }
}
