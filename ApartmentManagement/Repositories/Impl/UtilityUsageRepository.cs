using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class UtilityUsageRepository : IUtilityUsageRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilityUsageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UtilityUsage>> GetAllAsync()
        {
            return await _context.UtilityUsages
                .Include(u => u.Utility) // Kéo thông tin dịch vụ
                .ToListAsync();
        }

        public async Task<UtilityUsage?> GetByIdAsync(int id)
        {
            return await _context.UtilityUsages
                .Include(u => u.Utility)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        // Hàm kiểm tra chống trùng lặp dữ liệu trong 1 tháng
        public async Task<UtilityUsage?> GetByMonthYearAsync(int apartmentId, int utilityId, int month, int year)
        {
            return await _context.UtilityUsages
                .FirstOrDefaultAsync(u => u.ApartmentId == apartmentId
                                       && u.UtilityId == utilityId
                                       && u.Month == month
                                       && u.Year == year);
        }

        public async Task<UtilityUsage> AddAsync(UtilityUsage usage)
        {
            _context.UtilityUsages.Add(usage);
            await _context.SaveChangesAsync();
            return usage;
        }
    }
}