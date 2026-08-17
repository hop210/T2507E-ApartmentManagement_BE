using ApartmentManagement.DTOs.Floor;

namespace ApartmentManagement.DTOs.Building
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalFloors { get; set; }

        // Đổi từ ApartmentDTO sang FloorDTO
        public List<FloorDTO> Floors { get; set; } = new List<FloorDTO>();
    }

    public class CreateBuildingDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
    }
}