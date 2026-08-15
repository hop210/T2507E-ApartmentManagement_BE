using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username không được để trống")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; } = string.Empty;
    }
}