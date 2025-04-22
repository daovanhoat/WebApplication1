using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class SalaryService : ISalaryService
    {
        private readonly UserDBContext _context;
        private readonly IMapper _mapper;

        public SalaryService(UserDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<object> CalculateSalaryAsync(SalaryDto dto)
        {
            var user = await _context.Users.Include(u => u.Position)
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                return null;

            var totalSalary = user.Cong * dto.SalaryBasic * user.Position.HeSo;

            var salary = new SalaryModels
            {
                UserId = dto.UserId,
                WorkDay = user.Cong,
                SalaryBasic = dto.SalaryBasic,
                TotalSalary = totalSalary,
                CreateDate = DateTime.Now
            };

            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();

            return new
            {
                salary.Id,
                salary.UserId,
                user = new
                {
                    user.UserId,
                    user.Name
                },
                salary.SalaryBasic,
                salary.WorkDay,
                salary.TotalSalary,
                salary.CreateDate
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null) return false;

            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<object>> GetAllAsync()
        {
            var salaries = await _context.Salaries
                .Include(s => s.User)
                .Select(s => new
                {
                    s.Id,
                    s.UserId,
                    UserName = s.User.Name,
                    s.SalaryBasic,
                    s.WorkDay,
                    s.TotalSalary,
                    s.CreateDate
                })
                .ToListAsync();

            return salaries.Cast<object>().ToList();
        }
    }
}
