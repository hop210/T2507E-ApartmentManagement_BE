using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class ResidentRepository : IResidentRepository
    {
        private readonly ApplicationDbContext _context;

        public ResidentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Resident>> GetAllAsync()
        {
            // Thêm .Where(r => r.IsActive) để chỉ lấy những người còn hoạt động
            return await _context.Residents.Where(r => r.IsActive).ToListAsync();
        }

        public async Task<Resident?> GetByIdAsync(int id)
        {
            // Thêm điều kiện r.IsActive để không trả về người đã bị xóa mềm
            return await _context.Residents.FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        public async Task<Resident> AddAsync(Resident resident)
        {
            _context.Residents.Add(resident);
            await _context.SaveChangesAsync();
            return resident;
        }

        public async Task UpdateAsync(Resident resident)
        {
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resident resident)
        {
            // Đổi trạng thái
            resident.IsActive = false;
            _context.Residents.Update(resident);
            await _context.SaveChangesAsync();
        }
        public async Task<Resident?> GetByIdentityCardAsync(string identityCard)
        {
            // Tìm theo CCCD, KHÔNG lọc IsActive để lấy được cả người đã nghỉ thuê
            return await _context.Residents.FirstOrDefaultAsync(r => r.IdentityCard == identityCard);
        }
    }
}