
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class workingInfoService : IWorkingInfoService
    {
        private readonly UserDBContext _Context;

        public workingInfoService(UserDBContext context)
        {
            _Context = context;
        }

        public async Task<IEnumerable<object>> FilterWorkingInfoAsync(string? UserId)
        {
            var query = from w in _Context.WorkingInfos
                        join p in _Context.Positions on w.PositionId equals p.PositionId
                        join d in _Context.Departments on w.DepartmentId equals d.DepartmentId
                        join u in _Context.Users on w.UserId equals u.UserId
                        select new
                        {
                            UserId = u.UserId,
                            User = u.Name,
                            Position = p.Name,
                            Department = d.Name,
                            Time = w.Time,
                            EndDate = w.EndDate,
                        };
            if(!string.IsNullOrEmpty(UserId))
            {
                query = query.Where(x => x.UserId == UserId);
            }
            return await query.ToListAsync();

        }

        public async Task<IEnumerable<object>> GetWorkingInfoAsync(string userId = null)
        {
            var query = from w in _Context.WorkingInfos
                        join p in _Context.Positions on w.PositionId equals p.PositionId
                        join d in _Context.Departments on w.DepartmentId equals d.DepartmentId
                        join u in _Context.Users on w.UserId equals u.UserId
                        where userId == null || w.UserId == userId // chỉ lọc nếu có userId
                        orderby w.Time descending
                        select new
                        {
                            UserId = u.UserId,
                            User = u.Name,
                            Position = p.Name,
                            Department = d.Name,
                            Time = w.Time,
                            EndDate = w.EndDate,
                        };

            return await query.ToListAsync();
        }



        //public async Task<bool> UpdateWorkingInfoTimeAsync()
        //{
        //    var workingInfos = await _Context.WorkingInfos.ToListAsync();

        //    foreach (var w in workingInfos)
        //    {
        //        var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserId == w.UserId);
        //        if (user != null)
        //        {
        //            w.Time = user.CreateDate;
        //        }
        //    }

        //    await _Context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<bool> SeedWorkingInfoFromUsersAsync()
        //{
        //    var users = await _Context.Users.ToListAsync();

        //    var workingInfos = users.Select(u => new WorkingInfoModel
        //    {
        //        UserId = u.UserId,
        //        PositionId = u.PositionId,
        //        DepartmentId = u.DepartmentId,
        //        Time = DateTime.Now // hoặc u.CreateAt nếu bạn có thời gian tạo user
        //    }).ToList();

        //    _Context.WorkingInfos.AddRange(workingInfos);
        //    await _Context.SaveChangesAsync();
        //    return true;
        //}
    }
}
