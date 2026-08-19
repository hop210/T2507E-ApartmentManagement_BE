using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.User
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        [RegularExpression("^(ADMIN|MANAGER|STAFF|RESIDENT)$", ErrorMessage = "Role chỉ được là ADMIN, MANAGER, STAFF hoặc RESIDENT")]
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; } // Dùng để Khóa / Mở khóa tài khoản
    }
}