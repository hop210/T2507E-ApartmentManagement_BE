using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.FamilyMember
{
    public class CreateFamilyMemberDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn chủ hộ")]
        public int ResidentId { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mối quan hệ (Vợ, chồng, con...)")]
        public string Relationship { get; set; } = string.Empty;

        public string IdentityCard { get; set; } = string.Empty;
    }
}