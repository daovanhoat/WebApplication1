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
                HeSoLuong = position.HeSo
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

        public async Task<IEnumerable<object>> GetAllAnsyc()
        {
            var users = await _Context.Users.Include(u => u.Position).ToListAsync();
            return users.Select(u => new
            {
                u.UserId,
                u.Name,
                u.Gener,
                u.Age,
                u.Cong,
                u.PositionId,
                positionName = u.Position.Name,
                heSoLuong = u.Position.HeSo
            });
        }

        public async Task<IEnumerable<UserModel>> SearchUserAnsyc(string keyword)
        {
            return await _Context.Users
                .Where(u => u.Name.Contains(keyword)).ToListAsync();
        }

        public async Task<bool> UpdateUserAnsyc(int id, CreatePostDto dto)
        {
            var user = await _Context.Users.Include(u => u.Position).FirstOrDefaultAsync(u => u.UserId ==  id);
            if (user == null) return false;
            var position = await _Context.Positions.FindAsync(dto.PositionId);
            if (position == null) return false;

            _mapper.Map(dto, user);
            user.Position = position;

            await _Context.SaveChangesAsync();
            return true;
        }
    }
}
