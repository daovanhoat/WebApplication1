using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface IUserService
    {
        Task<IEnumerable<object>> GetAllAnsyc();
        Task<object> CreateUserAnsyc(CreatePostDto dto);
        Task<bool> UpdateUserAnsyc(string id, CreatePostDto dto);
        Task<bool> DeleteUserAnsyc(string id);
        Task<IEnumerable<UserDto>> SearchUserAnsyc(string keyword);
        Task<IEnumerable<object>> FilterByDepartmentAsync(int? departmentId);
        Task<bool> ImportFromExcelAsync(IFormFile file);
    }
}
