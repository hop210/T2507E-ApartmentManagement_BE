using ApartmentManagement.DTOs.Floor;

namespace ApartmentManagement.Services
{
    public interface IFloorService
    {
        Task<IEnumerable<FloorDTO>> GetAllByBuildingIdAsync(int buildingId);
        Task<FloorDTO?> GetByIdAsync(int id);
        Task<FloorDTO> CreateFloorAsync(CreateFloorDTO dto);
        Task<bool> UpdateFloorAsync(int id, UpdateFloorDTO dto);
        Task<bool> DeleteFloorAsync(int id);
    }
}