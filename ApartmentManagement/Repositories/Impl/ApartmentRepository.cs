using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ApartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync()
        {
            // Include(a => a.Building) để lấy luôn thông tin Tòa nhà nếu cần
            return await _context.Apartments.Include(a => a.Building).ToListAsync();
        }

        public async Task<Apartment> AddAsync(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();
            return apartment;
        }
        public async Task<Apartment?> GetByIdAsync(int id)
        {
            return await _context.Apartments.FindAsync(id);
        }

        public async Task UpdateAsync(Apartment apartment)
        {
            _context.Apartments.Update(apartment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Apartment apartment)
        {
            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();
        }
    }
}