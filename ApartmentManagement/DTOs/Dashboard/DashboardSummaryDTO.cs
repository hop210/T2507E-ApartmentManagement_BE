namespace ApartmentManagement.DTOs.Dashboard
{
    public class DashboardSummaryDTO
    {
        public int TotalApartments { get; set; }
        public int RentedApartments { get; set; }
        public int AvailableApartments { get; set; }
        public int TotalResidents { get; set; }
        public int ActiveContracts { get; set; }
        public decimal TotalRevenue { get; set; } // Tổng doanh thu từ hóa đơn đã thu
        public int UnpaidInvoicesCount { get; set; } // Số hóa đơn đang nợ
        public int PendingMaintenanceCount { get; set; } // Số yêu cầu bảo trì đang chờ
    }
}