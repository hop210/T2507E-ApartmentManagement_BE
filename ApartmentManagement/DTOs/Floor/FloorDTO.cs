using ApartmentManagement.DTOs.Apartment;

namespace ApartmentManagement.DTOs.Floor
{
    public class FloorDTO
    {
        public int Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;

        // Trả về luôn danh sách căn hộ nằm trong tầng này
        public List<ApartmentDTO> Apartments { get; set; } = new List<ApartmentDTO>();
    }
}
