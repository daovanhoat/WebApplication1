using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface ISalaryService
    {
        Task<List<object>> GetAllAsync();
        Task<object> CalculateSalaryAsync(SalaryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
