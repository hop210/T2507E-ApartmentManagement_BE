using ApartmentManagement.DTOs.Tenant;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _repository;

        public TenantService(ITenantRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TenantDTO>> GetAllTenantsAsync()
        {
            var tenants = await _repository.GetAllAsync();
            return tenants.Select(t => new TenantDTO
            {
                Id = t.Id,
                FullName = t.FullName,
                PhoneNumber = t.PhoneNumber,
                IdentityCard = t.IdentityCard,
                ApartmentId = t.ApartmentId
            });
        }

        public async Task<TenantDTO?> GetTenantByIdAsync(int id)
        {
            var tenant = await _repository.GetByIdAsync(id);
            if (tenant == null) return null;

            return new TenantDTO
            {
                Id = tenant.Id,
                FullName = tenant.FullName,
                PhoneNumber = tenant.PhoneNumber,
                IdentityCard = tenant.IdentityCard,
                ApartmentId = tenant.ApartmentId
            };
        }

        public async Task<TenantDTO> CreateTenantAsync(CreateTenantDTO dto)
        {
            var tenant = new Tenant
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                IdentityCard = dto.IdentityCard,
                ApartmentId = dto.ApartmentId
            };

            var created = await _repository.AddAsync(tenant);

            return new TenantDTO
            {
                Id = created.Id,
                FullName = created.FullName,
                PhoneNumber = created.PhoneNumber,
                IdentityCard = created.IdentityCard,
                ApartmentId = created.ApartmentId
            };
        }

        public async Task<bool> UpdateTenantAsync(int id, UpdateTenantDTO dto)
        {
            var tenant = await _repository.GetByIdAsync(id);
            if (tenant == null) return false;

            tenant.FullName = dto.FullName;
            tenant.PhoneNumber = dto.PhoneNumber;
            tenant.IdentityCard = dto.IdentityCard;

            await _repository.UpdateAsync(tenant);
            return true;
        }

        public async Task<bool> DeleteTenantAsync(int id)
        {
            var tenant = await _repository.GetByIdAsync(id);
            if (tenant == null) return false;

            await _repository.DeleteAsync(tenant);
            return true;
        }
    }
}