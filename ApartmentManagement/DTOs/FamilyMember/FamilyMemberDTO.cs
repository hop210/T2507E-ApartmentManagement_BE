namespace ApartmentManagement.DTOs.FamilyMember
{
    public class FamilyMemberDTO
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public string IdentityCard { get; set; } = string.Empty;
    }
}