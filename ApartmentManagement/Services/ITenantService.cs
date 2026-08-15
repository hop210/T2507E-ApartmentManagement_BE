using ApartmentManagement.DTOs.Tenant;

namespace ApartmentManagement.Services
{
    public interface ITenantService
    {
        Task<IEnumerable<TenantDTO>> GetAllTenantsAsync();
        Task<TenantDTO?> GetTenantByIdAsync(int id);
        Task<TenantDTO> CreateTenantAsync(CreateTenantDTO dto);
        Task<bool> UpdateTenantAsync(int id, UpdateTenantDTO dto);
        Task<bool> DeleteTenantAsync(int id);
    }
}