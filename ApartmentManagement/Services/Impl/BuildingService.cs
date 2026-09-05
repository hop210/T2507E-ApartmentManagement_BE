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

        // TRẢ LẠI SỰ TRONG SÁNG: Map cực ngắn gọn
        public async Task<IEnumerable<BuildingDTO>> GetAllBuildingsAsync()
        {
            var buildings = await _repository.GetAllAsync();
            return buildings.Select(b => new BuildingDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                TotalFloors = b.TotalFloors
                // Đã gỡ bỏ phần Map Tầng ra khỏi đây!
            });
        }

        // HÀM MỚI: Map nguyên cái cây siêu to
        public async Task<BuildingTreeDTO?> GetBuildingTreeAsync(int id)
        {
            var b = await _repository.GetBuildingTreeAsync(id);
            if (b == null) return null;

            return new BuildingTreeDTO
            {
                Id = b.Id,
                Name = b.Name,
                TotalFloors = b.TotalFloors,
                Floors = b.Floors?.Select(f => new FloorTreeDTO
                {
                    Id = f.Id,
                    FloorNumber = f.FloorNumber,
                    Apartments = f.Apartments?.Select(a => new ApartmentTreeDTO
                    {
                        Id = a.Id,
                        ApartmentNumber = a.ApartmentNumber,
                        Area = a.Area,
                        RentPrice = a.RentPrice,
                        Residents = a.Residents?.Select(r => new ResidentTreeDTO
                        {
                            Id = r.Id,
                            FullName = r.FullName,
                            PhoneNumber = r.PhoneNumber,
                            // BẬT LẠI KHÚC MAP NGƯỜI NHÀ Ở ĐÂY
                            FamilyMembers = r.FamilyMembers?.Select(fm => new FamilyMemberTreeDTO
                            {
                                Id = fm.Id,
                                FullName = fm.FullName,
                                Relationship = fm.Relationship
                            }).ToList() ?? new List<FamilyMemberTreeDTO>()
                        }).ToList() ?? new List<ResidentTreeDTO>()
                    }).ToList() ?? new List<ApartmentTreeDTO>()
                }).ToList() ?? new List<FloorTreeDTO>()
            };
        }

        public async Task<BuildingDTO> CreateBuildingAsync(CreateBuildingDTO dto)
        {
            var building = new Building
            {
                Name = dto.Name,
                Address = dto.Address,
                TotalFloors = dto.TotalFloors
            };
            var created = await _repository.AddAsync(building);
            return new BuildingDTO { Id = created.Id, Name = created.Name, Address = created.Address, TotalFloors = created.TotalFloors };
        }
    }
}