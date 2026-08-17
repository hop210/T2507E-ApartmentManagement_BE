using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class FloorRepository : IFloorRepository
    {
        private readonly ApplicationDbContext _context;

        public FloorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Floor>> GetAllByBuildingIdAsync(int buildingId)
        {
            return await _context.Floors
                .Include(f => f.Apartments)
                .Where(f => f.BuildingId == buildingId)
                .ToListAsync();
        }

        public async Task<Floor?> GetByIdAsync(int id)
        {
            return await _context.Floors
                .Include(f => f.Apartments)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Floor> AddAsync(Floor floor)
        {
            _context.Floors.Add(floor);
            await _context.SaveChangesAsync();
            return floor;
        }

        public async Task UpdateAsync(Floor floor)
        {
            _context.Floors.Update(floor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Floor floor)
        {
            _context.Floors.Remove(floor);
            await _context.SaveChangesAsync();
        }
    }
}