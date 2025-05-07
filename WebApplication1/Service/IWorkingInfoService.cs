namespace WebApplication1.Service
{
    public interface IWorkingInfoService
    {
        Task<IEnumerable<object>> GetWorkingInfoAsync(string userId = null);
        Task<IEnumerable<object>> FilterWorkingInfoAsync(string? UserId);

    }
}
