using ApartmentManagement.DTOs.Building;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class BuildingService : IBuildingService
    {
        private readonly IBuildingRepository _repository;

        public BuildingService(IBuildingRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BuildingDTO>> GetAllBuildingsAsync()
        {
            var buildings = await _repository.GetAllAsync();
            // Chuyển đổi từ Entity sang DTO để trả về
            return buildings.Select(b => new BuildingDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                TotalFloors = b.TotalFloors
            });
        }

        public async Task<BuildingDTO> CreateBuildingAsync(CreateBuildingDTO dto)
        {
            // Chuyển đổi từ DTO sang Entity để lưu vào DB
            var building = new Building
            {
                Name = dto.Name,
                Address = dto.Address,
                TotalFloors = dto.TotalFloors
            };

            var created = await _repository.AddAsync(building);

            return new BuildingDTO
            {
                Id = created.Id,
                Name = created.Name,
                Address = created.Address,
                TotalFloors = created.TotalFloors
            };
        }
    }
}
