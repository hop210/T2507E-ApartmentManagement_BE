using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Profile
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}