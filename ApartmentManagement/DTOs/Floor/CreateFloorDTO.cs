using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Floor
{
    public class CreateFloorDTO
    {
        [Required(ErrorMessage = "Tên tầng không được để trống.")]
        public string FloorNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phải chọn tòa nhà cho tầng này.")]
        public int BuildingId { get; set; }
    }
}
