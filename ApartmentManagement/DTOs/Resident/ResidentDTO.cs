namespace ApartmentManagement.DTOs.Resident
{
    public class ResidentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
        public int? ApartmentId { get; set; }
    }
}