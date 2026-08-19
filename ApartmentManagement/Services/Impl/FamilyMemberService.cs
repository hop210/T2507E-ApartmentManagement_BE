using ApartmentManagement.DTOs.FamilyMember;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class FamilyMemberService : IFamilyMemberService
    {
        private readonly IFamilyMemberRepository _repository;
        public FamilyMemberService(IFamilyMemberRepository repository) { _repository = repository; }

        public async Task<IEnumerable<FamilyMemberDTO>> GetMembersByResidentAsync(int residentId)
        {
            var members = await _repository.GetByResidentIdAsync(residentId);
            return members.Select(m => new FamilyMemberDTO
            {
                Id = m.Id,
                ResidentId = m.ResidentId,
                FullName = m.FullName,
                Relationship = m.Relationship,
                IdentityCard = m.IdentityCard
            });
        }

        public async Task<FamilyMemberDTO> AddMemberAsync(CreateFamilyMemberDTO dto)
        {
            var member = new ApartmentManagement.Entities.FamilyMember
            {
                ResidentId = dto.ResidentId,
                FullName = dto.FullName,
                Relationship = dto.Relationship,
                IdentityCard = dto.IdentityCard
            };
            var created = await _repository.AddAsync(member);
            return new FamilyMemberDTO
            {
                Id = created.Id,
                ResidentId = created.ResidentId,
                FullName = created.FullName,
                Relationship = created.Relationship,
                IdentityCard = created.IdentityCard
            };
        }

        public async Task<bool> RemoveMemberAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}