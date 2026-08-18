namespace ApartmentManagement.DTOs.Notification
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsGlobal { get; set; }
        public int? ApartmentId { get; set; } // Nếu có mã phòng thì hiển thị kèm số phòng
        public string? ApartmentNumber { get; set; }
    }
}