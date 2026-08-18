using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IUtilityRepository
    {
        Task<IEnumerable<Utility>> GetAllAsync();
        Task<Utility?> GetByIdAsync(int id);
        Task<Utility> AddAsync(Utility utility);
        Task UpdateAsync(Utility utility);
        Task DeleteAsync(Utility utility);
    }
}