using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phải phân quyền cho tài khoản")]
        [RegularExpression("^(ADMIN|MANAGER|STAFF|RESIDENT)$", ErrorMessage = "Role chỉ được là ADMIN, MANAGER, STAFF hoặc RESIDENT")]
        public string Role { get; set; } = "RESIDENT";
    }
}