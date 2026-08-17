using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IFloorRepository
    {
        Task<IEnumerable<Floor>> GetAllByBuildingIdAsync(int buildingId);
        Task<Floor?> GetByIdAsync(int id);
        Task<Floor> AddAsync(Floor floor);
        Task UpdateAsync(Floor floor);
        Task DeleteAsync(Floor floor);
    }
}