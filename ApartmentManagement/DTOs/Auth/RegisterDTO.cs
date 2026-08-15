using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Username không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Mặc định đăng ký mới sẽ là RESIDENT, nên có thể không cần bắt người dùng nhập Role
    }
}
