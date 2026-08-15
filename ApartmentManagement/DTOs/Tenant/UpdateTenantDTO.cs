namespace ApartmentManagement.DTOs.Tenant
{
    public class UpdateTenantDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
    }
}