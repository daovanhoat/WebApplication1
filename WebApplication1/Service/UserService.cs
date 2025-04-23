using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class UserService : IUserService
    {
        private readonly UserDBContext _Context;
        private readonly IMapper _mapper;

        public UserService (UserDBContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }

        public async Task<object> CreateUserAnsyc(CreatePostDto dto)
        {
            var position = await _Context.Positions.FindAsync(dto.PositionId);
            if (position == null) return null;

            var department = await _Context.Departments.FindAsync(dto.DepartmentId); 
            if (department == null) return null;

            var user = _mapper.Map<UserModel>(dto);
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();

            return new
            {
                id = user.UserId,
                user.Name,
                user.Age,
                user.Cong,
                user.PositionId,
                PositionName = position.Name,
                HeSoLuong = position.HeSo,
                user.DepartmentId,
                DepartmentName = department.Name,
            };
        }

        public async Task<bool> DeleteUserAnsyc(int id)
        {
            var user = await _Context.Users.FindAsync(id);
            if (user == null) return false;
            _Context.Users.Remove(user);
            await _Context.SaveChangesAsync();
            return true;

        }

        public async Task<IEnumerable<object>> FilterByDepartmentAsync(int? departmentId)
        {
            var query = from u in _Context.Users
                        join p in _Context.Positions on u.PositionId equals p.PositionId
                        join d in _Context.Departments on u.DepartmentId equals d.DepartmentId
                        select new
                        {
                            u.UserId,
                            u.Name,
                            u.Gener,
                            u.Age,
                            u.Cong,
                            u.PositionId,
                            PositionName = p.Name,
                            HeSoLuong = p.HeSo,
                            u.DepartmentId,
                            DepartmentName = d.Name
                        };

            // Lọc nếu có departmentId được truyền vào
            if (departmentId.HasValue)
            {
                query = query.Where(x => x.DepartmentId == departmentId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetAllAnsyc()
        {
            //var users = await _Context.Users.Include(u => u.Position).ToListAsync();
            var user = from u in _Context.Users
                       join p in _Context.Positions on u.PositionId equals p.PositionId
                       join d in _Context.Departments on u.DepartmentId equals d.DepartmentId
                       select new
                       {
                           u.UserId,
                           u.Name,
                           u.Gener,
                           u.Age,
                           u.Cong,
                           u.PositionId,
                           PositionName = p.Name,
                           Hesoluong = p.HeSo,
                           u.DepartmentId,
                           DepartmentName = d.Name,
                       };

            return await user.ToListAsync();
        }

        public async Task<IEnumerable<UserModel>> SearchUserAnsyc(string keyword)
        {
            return await _Context.Users
                .Where(u => u.Name.Contains(keyword)).ToListAsync();
        }

        public async Task<bool> UpdateUserAnsyc(int id, CreatePostDto dto)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return false;

            var position = await _Context.Positions.FindAsync(dto.PositionId);
            if (position == null) return false;

            var department = await _Context.Departments.FindAsync(dto.DepartmentId);
            if (department == null) return false;

            // Cập nhật thông tin thủ công hoặc dùng AutoMapper nếu DTO map đầy đủ
            _mapper.Map(dto, user);

            // Gán ID thay vì navigation property
            user.PositionId = dto.PositionId;
            user.DepartmentId = dto.DepartmentId;

            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
