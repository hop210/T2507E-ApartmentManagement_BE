using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IBuildingRepository
    {
        Task<IEnumerable<Building>> GetAllAsync();
        Task<Building> AddAsync(Building building);
    }
}
