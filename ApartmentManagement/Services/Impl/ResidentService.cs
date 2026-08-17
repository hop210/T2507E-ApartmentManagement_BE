using ApartmentManagement.DTOs.Resident;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentRepository _repository;

        public ResidentService(IResidentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ResidentDTO>> GetAllResidentsAsync()
        {
            var residents = await _repository.GetAllAsync();
            return residents.Select(r => new ResidentDTO
            {
                Id = r.Id,
                FullName = r.FullName,
                PhoneNumber = r.PhoneNumber,
                IdentityCard = r.IdentityCard,
                ApartmentId = r.ApartmentId
            });
        }

        public async Task<ResidentDTO?> GetResidentByIdAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null) return null;

            return new ResidentDTO
            {
                Id = resident.Id,
                FullName = resident.FullName,
                PhoneNumber = resident.PhoneNumber,
                IdentityCard = resident.IdentityCard,
                ApartmentId = resident.ApartmentId
            };
        }

        public async Task<ResidentDTO> CreateResidentAsync(CreateResidentDTO dto)
        {
            var resident = new Resident
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                IdentityCard = dto.IdentityCard,
                ApartmentId = dto.ApartmentId
            };

            var created = await _repository.AddAsync(resident);

            return new ResidentDTO
            {
                Id = created.Id,
                FullName = created.FullName,
                PhoneNumber = created.PhoneNumber,
                IdentityCard = created.IdentityCard,
                ApartmentId = created.ApartmentId
            };
        }

        public async Task<bool> UpdateResidentAsync(int id, UpdateResidentDTO dto)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null) return false;

            resident.FullName = dto.FullName;
            resident.PhoneNumber = dto.PhoneNumber;
            resident.IdentityCard = dto.IdentityCard;

            await _repository.UpdateAsync(resident);
            return true;
        }

        public async Task<bool> DeleteResidentAsync(int id)
        {
            var resident = await _repository.GetByIdAsync(id);
            if (resident == null) return false;

            await _repository.DeleteAsync(resident);
            return true;
        }
    }
}