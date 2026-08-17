using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Floor
{
    public class UpdateFloorDTO
    {
        [Required(ErrorMessage = "Tên tầng không được để trống.")]
        public string FloorNumber { get; set; } = string.Empty;
    }
}
