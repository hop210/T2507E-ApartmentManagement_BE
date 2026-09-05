using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly ApplicationDbContext _context;

        public BuildingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. TRẢ LẠI SỰ NHẸ NHÀNG: Chặn không cho kéo Tầng/Phòng
        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            return await _context.Buildings.ToListAsync(); // Siêu nhẹ, gọi mất 0.01s
        }

        public async Task<Building?> GetByIdAsync(int id)
        {
            return await _context.Buildings.FirstOrDefaultAsync(b => b.Id == id);
        }

        // 2. API HẠNG NẶNG: Kéo nguyên cái cây phả hệ
        public async Task<Building?> GetBuildingTreeAsync(int id)
        {
            return await _context.Buildings
                .Include(b => b.Floors)
                    .ThenInclude(f => f.Apartments)
                        .ThenInclude(a => a.Residents)
                            .ThenInclude(r => r.FamilyMembers) // Bật lại dòng này, đảm bảo hết sạch lỗi đỏ!
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Building> AddAsync(Building building)
        {
            _context.Buildings.Add(building);
            await _context.SaveChangesAsync();
            return building;
        }
    }
}