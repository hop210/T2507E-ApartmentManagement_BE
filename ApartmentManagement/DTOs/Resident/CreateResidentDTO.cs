using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Resident
{
    public class CreateResidentDTO
    {
        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên phải từ 3 đến 100 ký tự.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ (Phải là số Việt Nam hợp lệ).")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "CCCD/CMND không được để trống.")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "CCCD/CMND phải có từ 9 đến 12 số.")]
        public string IdentityCard { get; set; } = string.Empty;

      
        public int? ApartmentId { get; set; }
    }
}