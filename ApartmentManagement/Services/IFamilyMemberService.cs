using ApartmentManagement.DTOs.FamilyMember;
namespace ApartmentManagement.Services
{
    public interface IFamilyMemberService
    {
        Task<IEnumerable<FamilyMemberDTO>> GetMembersByResidentAsync(int residentId);
        Task<FamilyMemberDTO> AddMemberAsync(CreateFamilyMemberDTO dto);
        Task<bool> RemoveMemberAsync(int id);
    }
}