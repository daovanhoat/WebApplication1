using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class PositionService : IPositionService
    {
        private readonly UserDBContext _context;
        private readonly IMapper _mapper;

        public PositionService(UserDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PositionDtocs> AddAnsyc(PositionDtocs dto)
        {
            var position = _mapper.Map<PositionModel>(dto);
            position.createAt = DateTime.Now;

            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return _mapper.Map<PositionDtocs>(position);
        }

        public async Task<bool> DeleteAnsyc(int id)
        {
            var pos = await _context.Positions.FindAsync(id);
            if (pos == null) return false;

            _context.Positions.Remove(pos);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PositionDtocs>> GetAllAnsyc()
        {
            var positions = await _context.Positions.ToListAsync();
            return _mapper.Map<List<PositionDtocs>>(positions);
        }

        public async Task<bool> UpdateAnsyc(int id, PositionDtocs dto)
        {
            var pos = await _context.Positions.FindAsync(id);
            if (pos == null) return false;

            pos.Name = dto.Name;
            pos.HeSo = dto.HeSo;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
