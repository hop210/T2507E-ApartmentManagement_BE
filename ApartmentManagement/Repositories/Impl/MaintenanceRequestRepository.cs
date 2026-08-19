using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class MaintenanceRequestRepository : IMaintenanceRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public MaintenanceRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MaintenanceRequest>> GetAllAsync()
        {
            return await _context.MaintenanceRequests
                .Include(m => m.Apartment)
                .Include(m => m.Resident)
                .Include(m => m.Images) // Nhớ Include bảng ảnh con
                .OrderByDescending(m => m.CreatedAt) // Ưu tiên hiển thị yêu cầu mới nhất lên đầu
                .ToListAsync();
        }

        public async Task<MaintenanceRequest?> GetByIdAsync(int id)
        {
            return await _context.MaintenanceRequests
                .Include(m => m.Apartment)
                .Include(m => m.Resident)
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<MaintenanceRequest> AddAsync(MaintenanceRequest request)
        {
            _context.MaintenanceRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }
        public async Task<MaintenanceRequest> UpdateAsync(MaintenanceRequest request)
        {
            _context.MaintenanceRequests.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}