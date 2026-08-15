using ApartmentManagement.DTOs.Apartment;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _repository;

        public ApartmentService(IApartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ApartmentDTO>> GetAllApartmentsAsync()
        {
            var apartments = await _repository.GetAllAsync();

            // Chuyển đổi từ Entity sang DTO
            return apartments.Select(a => new ApartmentDTO
            {
                Id = a.Id,
                ApartmentNumber = a.ApartmentNumber,
                Area = a.Area,
                RentPrice = a.RentPrice,
                Status = a.Status,
                BuildingId = a.BuildingId
            });
        }

        public async Task<ApartmentDTO> CreateApartmentAsync(CreateApartmentDTO dto)
        {
            var apartment = new Apartment
            {
                ApartmentNumber = dto.ApartmentNumber,
                Area = dto.Area,
                RentPrice = dto.RentPrice,
                Status = dto.Status,
                BuildingId = dto.BuildingId
            };

            var created = await _repository.AddAsync(apartment);

            return new ApartmentDTO
            {
                Id = created.Id,
                ApartmentNumber = created.ApartmentNumber,
                Area = created.Area,
                RentPrice = created.RentPrice,
                Status = dto.Status,
                BuildingId = created.BuildingId
            };
        }
        public async Task<ApartmentDTO?> GetApartmentByIdAsync(int id)
        {
            var apartment = await _repository.GetByIdAsync(id);
            if (apartment == null) return null;

            return new ApartmentDTO
            {
                Id = apartment.Id,
                ApartmentNumber = apartment.ApartmentNumber,
                Area = apartment.Area,
                RentPrice = apartment.RentPrice,
                Status = apartment.Status,
                BuildingId = apartment.BuildingId
            };
        }

        public async Task<bool> UpdateApartmentAsync(int id, UpdateApartmentDTO dto)
        {
            var apartment = await _repository.GetByIdAsync(id);
            if (apartment == null) return false;

            // Cập nhật thông tin mới
            apartment.ApartmentNumber = dto.ApartmentNumber;
            apartment.Area = dto.Area;
            apartment.RentPrice = dto.RentPrice;
            apartment.Status = dto.Status;

            await _repository.UpdateAsync(apartment);
            return true;
        }

        public async Task<bool> DeleteApartmentAsync(int id)
        {
            var apartment = await _repository.GetByIdAsync(id);
            if (apartment == null) return false;

            await _repository.DeleteAsync(apartment);
            return true;
        }
    }
}