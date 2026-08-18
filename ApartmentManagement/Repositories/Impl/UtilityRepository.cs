using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class UtilityRepository : IUtilityRepository
    {
        private readonly ApplicationDbContext _context;

        public UtilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Utility>> GetAllAsync()
        {
            return await _context.Utilities.ToListAsync();
        }

        public async Task<Utility?> GetByIdAsync(int id)
        {
            return await _context.Utilities.FindAsync(id);
        }

        public async Task<Utility> AddAsync(Utility utility)
        {
            _context.Utilities.Add(utility);
            await _context.SaveChangesAsync();
            return utility;
        }

        public async Task UpdateAsync(Utility utility)
        {
            _context.Utilities.Update(utility);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Utility utility)
        {
            _context.Utilities.Remove(utility);
            await _context.SaveChangesAsync();
        }
    }
}