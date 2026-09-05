namespace ApartmentManagement.DTOs.Building
{
    public class BuildingTreeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
        public List<FloorTreeDTO> Floors { get; set; } = new List<FloorTreeDTO>();
    }

    public class FloorTreeDTO
    {
        public int Id { get; set; }
        public string FloorNumber { get; set; } = string.Empty;
        public List<ApartmentTreeDTO> Apartments { get; set; } = new List<ApartmentTreeDTO>();
    }

    public class ApartmentTreeDTO
    {
        public int Id { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty;
        public double Area { get; set; }
        public decimal RentPrice { get; set; }
        // Giả sử có danh sách cư dân (Thay đổi tên biến nếu bác dùng bảng trung gian ApartmentResidents)
        public List<ResidentTreeDTO> Residents { get; set; } = new List<ResidentTreeDTO>();
    }

    public class ResidentTreeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<FamilyMemberTreeDTO> FamilyMembers { get; set; } = new List<FamilyMemberTreeDTO>();
    }

    public class FamilyMemberTreeDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
    }
}