using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Dashboard;
using ApartmentManagement.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardSummaryDTO> GetSummaryAsync()
        {
            // 1. Thống kê Căn hộ
            var totalApts = await _context.Apartments.CountAsync();
            var rentedApts = await _context.Apartments.CountAsync(a => a.Status == ApartmentStatus.Rented);
            var availableApts = await _context.Apartments.CountAsync(a => a.Status == ApartmentStatus.Available);

            // 2. Thống kê Cư dân & Hợp đồng
            var totalResidents = await _context.Residents.CountAsync();
            var activeContracts = await _context.Contracts.CountAsync(c => c.Status == ContractStatus.Active);

            // 3. Thống kê Tài chính (Giả sử bảng Invoice của bạn có cột TotalAmount lưu tổng tiền)
            var totalRevenue = await _context.Invoices
                .Where(i => i.Status == InvoiceStatus.Paid)
                .SumAsync(i => i.TotalAmount);

            var unpaidInvoices = await _context.Invoices.CountAsync(i => i.Status == InvoiceStatus.Unpaid);

            // 4. Thống kê Bảo trì
            var pendingMaintenance = await _context.MaintenanceRequests.CountAsync(m => m.Status == MaintenanceStatus.Pending);

            return new DashboardSummaryDTO
            {
                TotalApartments = totalApts,
                RentedApartments = rentedApts,
                AvailableApartments = availableApts,
                TotalResidents = totalResidents,
                ActiveContracts = activeContracts,
                TotalRevenue = totalRevenue,
                UnpaidInvoicesCount = unpaidInvoices,
                PendingMaintenanceCount = pendingMaintenance
            };
        }
    }
}