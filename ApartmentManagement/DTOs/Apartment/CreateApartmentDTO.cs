using ApartmentManagement.Enums;

namespace ApartmentManagement.DTOs.Apartment
{
    public class CreateApartmentDTO
    {
        public string ApartmentNumber { get; set; } = string.Empty;
        public double Area { get; set; }
        public decimal RentPrice { get; set; }
        public ApartmentStatus Status { get; set; }
        public int BuildingId { get; set; }
    }
}