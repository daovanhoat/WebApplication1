﻿using WebApplication1.Models;

namespace WebApplication1.Service
{
    public interface IDepartmentService
    {
        Task<object> CreateDepartmentAsync(DepartmentDto dto);
        Task<List<DepartmentDto>> GetAllAsync();
        Task<bool> DeleteDepartment(int id);
        Task<bool> UpdateDepartment(int id, DepartmentDto dto);
    }
}
