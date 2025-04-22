using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface IPositionService
    {
        Task<List<PositionDtocs>> GetAllAnsyc();
        Task<PositionDtocs> AddAnsyc(PositionDtocs dto);
        Task<bool> DeleteAnsyc(int id);
        Task<bool> UpdateAnsyc(int id, PositionDtocs dto);
    }
}
