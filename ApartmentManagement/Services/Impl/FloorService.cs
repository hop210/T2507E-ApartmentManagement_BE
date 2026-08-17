using ApartmentManagement.DTOs.Floor;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _repository;

        public FloorService(IFloorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FloorDTO>> GetAllByBuildingIdAsync(int buildingId)
        {
            var floors = await _repository.GetAllByBuildingIdAsync(buildingId);
            return floors.Select(f => new FloorDTO
            {
                Id = f.Id,
                FloorNumber = f.FloorNumber
            });
        }

        public async Task<FloorDTO?> GetByIdAsync(int id)
        {
            var floor = await _repository.GetByIdAsync(id);
            if (floor == null) return null;

            return new FloorDTO
            {
                Id = floor.Id,
                FloorNumber = floor.FloorNumber
            };
        }
        public async Task<FloorDTO> CreateFloorAsync(CreateFloorDTO dto)
        {
            var floor = new Floor // Dùng class Floor từ thư mục Entities
            {
                FloorNumber = dto.FloorNumber,
                BuildingId = dto.BuildingId
            };

            var created = await _repository.AddAsync(floor);

            return new FloorDTO
            {
                Id = created.Id,
                FloorNumber = created.FloorNumber
            };
        }

        public async Task<bool> UpdateFloorAsync(int id, UpdateFloorDTO dto)
        {
            var floor = await _repository.GetByIdAsync(id);
            if (floor == null) return false;

            floor.FloorNumber = dto.FloorNumber;
            await _repository.UpdateAsync(floor);
            return true;
        }

        public async Task<bool> DeleteFloorAsync(int id)
        {
            var floor = await _repository.GetByIdAsync(id);
            if (floor == null) return false;

            await _repository.DeleteAsync(floor);
            return true;
        }
    }
}