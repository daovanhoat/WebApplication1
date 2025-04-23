namespace WebApplication1.Service
{
    public interface IWorkingInfoService
    {
        Task<IEnumerable<object>> GetWorkingInfoAsync(int UserId);
    }
}
