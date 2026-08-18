using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; // Bắt buộc phải có để dùng IFormFile

namespace ApartmentManagement.DTOs.Maintenance
{
    public class CreateMaintenanceRequestDTO
    {
        [Required]
        public int ApartmentId { get; set; }

        [Required]
        public int ResidentId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả sự cố để ban quản lý nắm thông tin")]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Dùng List<IFormFile> để cho phép người dùng upload cùng lúc nhiều ảnh
        public List<IFormFile>? ImageFiles { get; set; }
    }
}