using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IUtilityUsageRepository
    {
        Task<IEnumerable<UtilityUsage>> GetAllAsync();
        Task<UtilityUsage?> GetByIdAsync(int id);
        Task<UtilityUsage?> GetByMonthYearAsync(int apartmentId, int utilityId, int month, int year);
        Task<UtilityUsage> AddAsync(UtilityUsage usage);
    }
}