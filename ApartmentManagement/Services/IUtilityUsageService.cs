using ApartmentManagement.DTOs.UtilityUsage;

namespace ApartmentManagement.Services
{
    public interface IUtilityUsageService
    {
        Task<IEnumerable<UtilityUsageDTO>> GetAllUsagesAsync();
        Task<UtilityUsageDTO?> CreateUsageAsync(CreateUtilityUsageDTO dto);
    }
}