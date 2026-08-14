namespace ApartmentManagement.DTOs
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
    }

    public class CreateBuildingDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
    }
}