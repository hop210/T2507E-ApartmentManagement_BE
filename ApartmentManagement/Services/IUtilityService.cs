using ApartmentManagement.DTOs.Utility;

namespace ApartmentManagement.Services
{
    public interface IUtilityService
    {
        Task<IEnumerable<UtilityDTO>> GetAllUtilitiesAsync();
        Task<UtilityDTO?> GetUtilityByIdAsync(int id);
        Task<UtilityDTO> CreateUtilityAsync(CreateUtilityDTO dto);
        Task<bool> UpdateUtilityAsync(int id, UpdateUtilityDTO dto);
        Task<bool> DeleteUtilityAsync(int id);
    }
}