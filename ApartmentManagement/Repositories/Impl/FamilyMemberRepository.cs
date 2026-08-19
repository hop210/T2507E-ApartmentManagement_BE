using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;
namespace ApartmentManagement.Repositories.Impl
{
    public class FamilyMemberRepository : IFamilyMemberRepository
    {
        private readonly ApplicationDbContext _context;
        public FamilyMemberRepository(ApplicationDbContext context) { _context = context; }

        public async Task<IEnumerable<FamilyMember>> GetByResidentIdAsync(int residentId)
        {
            return await _context.FamilyMembers.Where(f => f.ResidentId == residentId).ToListAsync();
        }

        public async Task<FamilyMember> AddAsync(FamilyMember member)
        {
            _context.FamilyMembers.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var member = await _context.FamilyMembers.FindAsync(id);
            if (member == null) return false;
            _context.FamilyMembers.Remove(member);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}