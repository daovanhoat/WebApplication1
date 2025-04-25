using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<UserModel> Users {  get; set; }
        public DbSet<SalaryModels> Salaries { get; set; }
        public DbSet<PositionModel> Positions { get; set; }

        public DbSet<AccountModel> Accounts { get; set; }

        public DbSet<DepartmentModel> Departments { get; set; }

        public DbSet<WorkingInfoModel> WorkingInfos { get; set; }

        public DbSet<AttendanceLogModel> AttendanceLogs { get; set; }

    }
}
