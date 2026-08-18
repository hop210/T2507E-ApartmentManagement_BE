using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Notification
{
    public class CreateNotificationDTO
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public bool IsGlobal { get; set; } = true;

        // Nếu IsGlobal = false, FE bắt buộc phải gửi mã phòng lên
        public int? ApartmentId { get; set; }
    }
}