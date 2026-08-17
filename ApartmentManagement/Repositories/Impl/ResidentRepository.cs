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
            return await _context.Residents.ToListAsync();
        }

        public async Task<Resident?> GetByIdAsync(int id)
        {
            return await _context.Residents.FindAsync(id);
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
            _context.Residents.Remove(resident);
            await _context.SaveChangesAsync();
        }
    }
}