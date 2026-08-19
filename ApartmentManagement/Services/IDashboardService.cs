using ApartmentManagement.DTOs.Dashboard;

namespace ApartmentManagement.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDTO> GetSummaryAsync();
    }
}