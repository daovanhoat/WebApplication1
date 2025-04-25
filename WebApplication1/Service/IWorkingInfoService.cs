namespace WebApplication1.Service
{
    public interface IWorkingInfoService
    {
        Task<IEnumerable<object>> GetWorkingInfoAsync();
        //Task<bool> SeedWorkingInfoFromUsersAsync();
        //Task<bool> UpdateWorkingInfoTimeAsync();
        Task<IEnumerable<object>> FilterWorkingInfoAsync(string? UserId);

    }
}
