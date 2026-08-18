namespace ApartmentManagement.DTOs.Maintenance
{
    public class MaintenanceRequestDTO
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty;
        public int ResidentId { get; set; }
        public string ResidentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending, Processing, Done...
        public DateTime CreatedAt { get; set; }

        // Danh sách các đường link ảnh đính kèm để Frontend hiển thị
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}