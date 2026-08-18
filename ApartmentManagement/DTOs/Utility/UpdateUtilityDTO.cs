using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Utility
{
    public class UpdateUtilityDTO
    {
        [Required(ErrorMessage = "Tên dịch vụ không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public decimal UnitPrice { get; set; }

        [Required]
        public string Unit { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
