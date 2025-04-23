using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly UserDBContext _Context;
        private readonly IMapper _mapper;

        public DepartmentService(UserDBContext context, IMapper mapper)
        {
            _Context = context;
            _mapper = mapper;
        }
        public async Task<object> CreateDepartmentAsync(DepartmentDto dto)
        {
            var department = _mapper.Map<DepartmentModel>(dto);
            
            _Context.Departments.Add(department);
            await _Context.SaveChangesAsync();
            return _mapper.Map<DepartmentDto>(department);
        }

        public async Task<List<DepartmentDto>> GetAllAsync()
        {
            var department = await _Context.Departments.ToListAsync();
            return _mapper.Map<List<DepartmentDto>>(department);
        }
    }
}
