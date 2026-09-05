using ApartmentManagement.DTOs.Floor;
using ApartmentManagement.DTOs.Apartment; 
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;
using ApartmentManagement.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ApartmentManagement.Services.Impl
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _repository;
        private readonly IBuildingRepository _buildingRepository;

        public FloorService(IFloorRepository repository, IBuildingRepository buildingRepository)
        {
            _repository = repository;
            _buildingRepository = buildingRepository;
        }

        public async Task<IEnumerable<FloorDTO>> GetAllByBuildingIdAsync(int buildingId)
        {
            var floors = await _repository.GetAllByBuildingIdAsync(buildingId);
            return floors.Select(f => new FloorDTO
            {
                Id = f.Id,
                FloorNumber = f.FloorNumber,
                // Bổ sung gắp mảng Căn hộ bỏ vào DTO
                Apartments = f.Apartments?.Select(a => new ApartmentDTO
                {
                    Id = a.Id,
                    ApartmentNumber = a.ApartmentNumber,
                    RentPrice = a.RentPrice,
                    Status = a.Status
                }).ToList() ?? new List<ApartmentDTO>()
            });
        }

        public async Task<FloorDTO?> GetByIdAsync(int id)
        {
            var floor = await _repository.GetByIdAsync(id);
            if (floor == null) return null;

            return new FloorDTO
            {
                Id = floor.Id,
                FloorNumber = floor.FloorNumber,
                
                Apartments = floor.Apartments?.Select(a => new ApartmentDTO
                {
                    Id = a.Id,
                    ApartmentNumber = a.ApartmentNumber,
                    RentPrice = a.RentPrice,
                    Status = a.Status
                }).ToList() ?? new List<ApartmentDTO>()
            };
        }

        public async Task<FloorDTO> CreateFloorAsync(CreateFloorDTO dto)
        {
            // 1. Tìm xem tòa nhà này có tồn tại không
            var building = await _buildingRepository.GetByIdAsync(dto.BuildingId);
            if (building == null)
            {
                throw new AppException("Không tìm thấy tòa nhà.", StatusCodes.Status404NotFound);
            }

            // 2. Đếm số tầng hiện tại của tòa nhà
            var currentFloors = await _repository.GetAllByBuildingIdAsync(dto.BuildingId);
            if (currentFloors.Count() >= building.TotalFloors)
            {
                throw new AppException($"Tòa nhà '{building.Name}' đã đạt giới hạn tối đa {building.TotalFloors} tầng. Không thể xây thêm!", StatusCodes.Status400BadRequest);
            }

            // 3. Nếu mọi thứ hợp lệ thì mới lưu
            var floor = new Floor
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