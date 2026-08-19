using ApartmentManagement.Entities;
namespace ApartmentManagement.Repositories
{
    public interface IFamilyMemberRepository
    {
        Task<IEnumerable<FamilyMember>> GetByResidentIdAsync(int residentId);
        Task<FamilyMember> AddAsync(FamilyMember member);
        Task<bool> DeleteAsync(int id);
    }
}