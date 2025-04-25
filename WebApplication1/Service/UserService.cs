using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

            var workingInfo = new WorkingInfoModel
            {
                UserId = user.UserId,
                PositionId = user.PositionId,
                DepartmentId = user.DepartmentId,
                Time = user.CreateDate,
                EndDate = null
            };
            _Context.WorkingInfos.Add(workingInfo);
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
                Time = user.CreateDate = DateTime.Now,
            };
        }

        public async Task<bool> DeleteUserAnsyc(string id)
        {
            var user = await _Context.Users.FindAsync(id); 
            if (user == null) return false;
            var workingInfos = await _Context.WorkingInfos
                .Where(w => w.UserId == id)
                .ToListAsync();
            _Context.WorkingInfos.RemoveRange(workingInfos);
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
                           u.CreateDate,
                       };

            return await user.ToListAsync();
        }

        public async Task<bool> ImportFromExcelAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Nguyen Van A");
            using var package = new ExcelPackage(stream);
            var workSheet = package.Workbook.Worksheets[0];
            var rowCount = workSheet.Dimension.Rows;

            int errorCol = workSheet.Dimension.Columns + 1;
            workSheet.Cells[1, errorCol].Value = "Lỗi";

            bool hasError = false;

            for (int row = 2; row <= rowCount; row++)
            {
                var errors = new List<string>();

                string userId = workSheet.Cells[row, 1].Text.Trim();
                string name = workSheet.Cells[row, 2].Text.Trim();
                string departmentName = workSheet.Cells[row, 3].Text.Trim();
                string positionName = workSheet.Cells[row, 4].Text.Trim();
                string ageText = workSheet.Cells[row, 5].Text.Trim();
                string gender = workSheet.Cells[row, 6].Text.Trim();
                string congText = workSheet.Cells[row, 7].Text.Trim();

                // Kiểm tra trống
                if (string.IsNullOrWhiteSpace(userId)) errors.Add("Mã nhân viên trống");
                if (string.IsNullOrWhiteSpace(name)) errors.Add("Tên trống");
                if (string.IsNullOrWhiteSpace(departmentName)) errors.Add("Phòng ban trống");
                if (string.IsNullOrWhiteSpace(positionName)) errors.Add("Chức vụ trống");
                if (string.IsNullOrWhiteSpace(ageText)) errors.Add("Tuổi trống");
                if (string.IsNullOrWhiteSpace(gender)) errors.Add("Giới tính trống");
                if (string.IsNullOrWhiteSpace(congText)) errors.Add("Công trống");

                // Kiểm tra trùng mã nhân viên
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    bool isDuplicate = await _Context.Users.AnyAsync(u => u.UserId == userId);
                    if (isDuplicate) errors.Add("Trùng mã nhân viên");
                }

                // Kiểm tra parse số
                bool isAgeValid = int.TryParse(ageText, out int age);
                if (!isAgeValid || age < 18 || age > 65) errors.Add("Tuổi không hợp lệ");

                bool isCongValid = int.TryParse(congText, out int cong);
                if (!isCongValid || cong < 25 || cong > 30) errors.Add("Công không hợp lệ");

                if (errors.Count > 0)
                {
                    workSheet.Cells[row, errorCol].Value = string.Join("; ", errors);
                    hasError = true;
                    continue; // Bỏ qua thêm vào DB nếu có lỗi
                }
                var user = new UserModel
                {
                    UserId = workSheet.Cells[row, 1].Text,
                    Name = workSheet.Cells[row, 2].Text.Trim(),
                    DepartmentId = await GetDepartmentIdByName(workSheet.Cells[row, 3].Text.Trim()),
                    PositionId = await GetPositionIdByName(workSheet.Cells[row, 4].Text.Trim()),
                    Age = int.Parse(workSheet.Cells[row, 5].Text.Trim()),
                    Gener = workSheet.Cells[row, 6].Text.Trim(),
                    Cong = int.Parse(workSheet.Cells[row, 7].Text.Trim()),
                };
                _Context.Users.Add(user);
            }
            if (hasError)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "errors");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, $"ImportErrors_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
                File.WriteAllBytes(filePath, package.GetAsByteArray());

                return false; // báo về controller rằng có lỗi
            }

            await _Context.SaveChangesAsync();
            return true;
        }
        private async Task<int> GetDepartmentIdByName(string name)
        {
            var dept = await _Context.Departments.FirstOrDefaultAsync(x => x.Name == name);
            return dept?.DepartmentId ?? throw new Exception($"Phong ban '{name}' khong ton tai. ");
        }
        private async Task<int> GetPositionIdByName(string name)
        {
            var dept = await _Context.Positions.FirstOrDefaultAsync(x => x.Name == name);
            return dept?.PositionId ?? throw new Exception($"Chuc vu '{name}' khong ton tai. ");
        }

        public async Task<IEnumerable<UserModel>> SearchUserAnsyc(string keyword)
        {
            return await _Context.Users
                .Where(u => u.Name.Contains(keyword)).ToListAsync();
        }

        public async Task<bool> UpdateUserAnsyc(string id, CreatePostDto dto)
        {
            var user = await _Context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null) return false;

            var position = await _Context.Positions.FindAsync(dto.PositionId);
            if (position == null) return false;

            var department = await _Context.Departments.FindAsync(dto.DepartmentId);
            if (department == null) return false;

            bool isChange = (user.PositionId != dto.PositionId || user.DepartmentId != dto.DepartmentId);
            if (isChange)
            {
                var lastWorkingInfo = await _Context.WorkingInfos
                    .Where(w => w.UserId == id && w.EndDate == null)
                    .OrderByDescending(w => w.Time)
                    .FirstOrDefaultAsync();

                if (lastWorkingInfo != null)
                {
                    lastWorkingInfo.EndDate = DateTime.Now;
                }

                _mapper.Map(dto, user);

                // Gán ID thay vì navigation property
                user.PositionId = dto.PositionId;
                user.DepartmentId = dto.DepartmentId;

                var newWorkingInfo = new WorkingInfoModel
                {
                    UserId = user.UserId,
                    PositionId = user.PositionId,
                    DepartmentId = user.DepartmentId,
                    Time = DateTime.Now,
                    EndDate = null
                };

                _Context.WorkingInfos.Add(newWorkingInfo);
                await _Context.SaveChangesAsync();
            } else
            {
                _mapper.Map(dto, user);
            }

            await _Context.SaveChangesAsync();

            return true;
        }
    }
}
