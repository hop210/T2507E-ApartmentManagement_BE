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
            // 1. Kiểm tra xem CCCD này đã từng tồn tại trong hệ thống chưa
            var existingResident = await _repository.GetByIdentityCardAsync(dto.IdentityCard);

            if (existingResident != null)
            {
                if (existingResident.IsActive)
                {
                    // Khách đang ở trong hệ thống mà tạo thêm là báo lỗi ngay
                    throw new Exception("Căn cước công dân này đã tồn tại và đang hoạt động trong hệ thống.");
                }
                else
                {
                    // 2. KỊCH BẢN KHÁCH CŨ QUAY LẠI: Kích hoạt lại và cập nhật thông tin
                    existingResident.IsActive = true;
                    existingResident.FullName = dto.FullName; // Lỡ khách đổi tên
                    existingResident.PhoneNumber = dto.PhoneNumber; // Lỡ khách đổi số điện thoại
                    existingResident.ApartmentId = dto.ApartmentId;

                    await _repository.UpdateAsync(existingResident);

                    return new ResidentDTO
                    {
                        Id = existingResident.Id,
                        FullName = existingResident.FullName,
                        PhoneNumber = existingResident.PhoneNumber,
                        IdentityCard = existingResident.IdentityCard,
                        ApartmentId = existingResident.ApartmentId
                    };
                }
            }

            // 3. Nếu chưa từng tồn tại -> Tạo mới hoàn toàn như bình thường
            var resident = new Resident
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                IdentityCard = dto.IdentityCard,
                ApartmentId = dto.ApartmentId,
                IsActive = true 
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