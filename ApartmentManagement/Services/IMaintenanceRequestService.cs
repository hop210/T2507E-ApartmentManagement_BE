using ApartmentManagement.DTOs.Maintenance;

namespace ApartmentManagement.Services
{
    public interface IMaintenanceRequestService
    {
        Task<IEnumerable<MaintenanceRequestDTO>> GetAllRequestsAsync();
        Task<MaintenanceRequestDTO?> GetRequestByIdAsync(int id);
        Task<MaintenanceRequestDTO> CreateRequestAsync(CreateMaintenanceRequestDTO dto);
    }
}