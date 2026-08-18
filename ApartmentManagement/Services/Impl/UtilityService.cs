using ApartmentManagement.DTOs.Utility;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class UtilityService : IUtilityService
    {
        private readonly IUtilityRepository _repository;

        public UtilityService(IUtilityRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UtilityDTO>> GetAllUtilitiesAsync()
        {
            var utilities = await _repository.GetAllAsync();
            return utilities.Select(u => new UtilityDTO
            {
                Id = u.Id,
                Name = u.Name,
                UnitPrice = u.UnitPrice,
                Unit = u.Unit,
                IsActive = u.IsActive
            });
        }

        public async Task<UtilityDTO?> GetUtilityByIdAsync(int id)
        {
            var utility = await _repository.GetByIdAsync(id);
            if (utility == null) return null;

            return new UtilityDTO
            {
                Id = utility.Id,
                Name = utility.Name,
                UnitPrice = utility.UnitPrice,
                Unit = utility.Unit,
                IsActive = utility.IsActive
            };
        }

        public async Task<UtilityDTO> CreateUtilityAsync(CreateUtilityDTO dto)
        {
            var utility = new Utility
            {
                Name = dto.Name,
                UnitPrice = dto.UnitPrice,
                Unit = dto.Unit,
                IsActive = true
            };

            var created = await _repository.AddAsync(utility);

            return new UtilityDTO
            {
                Id = created.Id,
                Name = created.Name,
                UnitPrice = created.UnitPrice,
                Unit = created.Unit,
                IsActive = created.IsActive
            };
        }

        public async Task<bool> UpdateUtilityAsync(int id, UpdateUtilityDTO dto)
        {
            var utility = await _repository.GetByIdAsync(id);
            if (utility == null) return false;

            utility.Name = dto.Name;
            utility.UnitPrice = dto.UnitPrice;
            utility.Unit = dto.Unit;
            utility.IsActive = dto.IsActive;

            await _repository.UpdateAsync(utility);
            return true;
        }

        public async Task<bool> DeleteUtilityAsync(int id)
        {
            var utility = await _repository.GetByIdAsync(id);
            if (utility == null) return false;

            await _repository.DeleteAsync(utility);
            return true;
        }
    }
}