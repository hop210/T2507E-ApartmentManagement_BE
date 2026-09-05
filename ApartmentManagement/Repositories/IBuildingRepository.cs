using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IBuildingRepository
    {
        Task<IEnumerable<Building>> GetAllAsync();
        Task<Building?> GetByIdAsync(int id);
        Task<Building?> GetBuildingTreeAsync(int id);
        Task<Building> AddAsync(Building building);
    }
}
