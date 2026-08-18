using ApartmentManagement.DTOs.UtilityUsage;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;

namespace ApartmentManagement.Services.Impl
{
    public class UtilityUsageService : IUtilityUsageService
    {
        private readonly IUtilityUsageRepository _repository;

        public UtilityUsageService(IUtilityUsageRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UtilityUsageDTO>> GetAllUsagesAsync()
        {
            var usages = await _repository.GetAllAsync();
            return usages.Select(u => new UtilityUsageDTO
            {
                Id = u.Id,
                ApartmentId = u.ApartmentId,
                UtilityId = u.UtilityId,
                UtilityName = u.Utility?.Name ?? "",
                Month = u.Month,
                Year = u.Year,
                OldIndicator = u.OldIndicator,
                NewIndicator = u.NewIndicator,
                UsageAmount = u.UsageAmount
            });
        }

        public async Task<UtilityUsageDTO?> CreateUsageAsync(CreateUtilityUsageDTO dto)
        {
            // 1. Validate: Chỉ số mới phải lớn hơn hoặc bằng chỉ số cũ
            if (dto.NewIndicator < dto.OldIndicator)
            {
                throw new ArgumentException("Chỉ số mới không được nhỏ hơn chỉ số cũ.");
            }

            // 2. Validate: Kiểm tra tháng này đã chốt số chưa
            var existingRecord = await _repository.GetByMonthYearAsync(dto.ApartmentId, dto.UtilityId, dto.Month, dto.Year);
            if (existingRecord != null)
            {
                throw new InvalidOperationException($"Phòng {dto.ApartmentId} đã chốt chỉ số cho dịch vụ này trong tháng {dto.Month}/{dto.Year}.");
            }

            // 3. Logic tính toán tự động
            double calculatedUsage = dto.NewIndicator - dto.OldIndicator;

            var usage = new UtilityUsage
            {
                ApartmentId = dto.ApartmentId,
                UtilityId = dto.UtilityId,
                Month = dto.Month,
                Year = dto.Year,
                OldIndicator = dto.OldIndicator,
                NewIndicator = dto.NewIndicator,
                UsageAmount = calculatedUsage // Lưu mức tiêu thụ tự động tính
            };

            var created = await _repository.AddAsync(usage);

            return new UtilityUsageDTO
            {
                Id = created.Id,
                ApartmentId = created.ApartmentId,
                UtilityId = created.UtilityId,
                Month = created.Month,
                Year = created.Year,
                OldIndicator = created.OldIndicator,
                NewIndicator = created.NewIndicator,
                UsageAmount = created.UsageAmount
            };
        }
    }
}