using ApartmentManagement.DTOs.Resident;

namespace ApartmentManagement.Services
{
    public interface IResidentService
    {
        Task<IEnumerable<ResidentDTO>> GetAllResidentsAsync();
        Task<ResidentDTO?> GetResidentByIdAsync(int id);
        Task<ResidentDTO> CreateResidentAsync(CreateResidentDTO dto);
        Task<bool> UpdateResidentAsync(int id, UpdateResidentDTO dto);
        Task<bool> DeleteResidentAsync(int id);
    }
}