using ApartmentManagement.DTOs.Building;

namespace ApartmentManagement.Services
{
    public interface IBuildingService
    {
        Task<IEnumerable<BuildingDTO>> GetAllBuildingsAsync();
        Task<BuildingDTO> CreateBuildingAsync(CreateBuildingDTO dto);
    }
}
