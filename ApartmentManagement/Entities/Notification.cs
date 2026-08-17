using System.ComponentModel.DataAnnotations;
// thông báo
namespace ApartmentManagement.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsGlobal { get; set; } = true;

        public int? ApartmentId { get; set; } // Nếu IsGlobal = False, sẽ gửi riêng cho 1 phòng
    }
}