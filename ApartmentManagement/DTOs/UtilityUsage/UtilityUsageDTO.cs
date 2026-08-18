namespace ApartmentManagement.DTOs.UtilityUsage
{
    public class UtilityUsageDTO
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public int UtilityId { get; set; }
        public string UtilityName { get; set; } = string.Empty; // Trả về tên dịch vụ (Điện/Nước) cho dễ nhìn
        public int Month { get; set; }
        public int Year { get; set; }
        public double OldIndicator { get; set; }
        public double NewIndicator { get; set; }
        public double UsageAmount { get; set; } // Số lượng tiêu thụ
    }
}
