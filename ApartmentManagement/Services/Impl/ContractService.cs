using ApartmentManagement.DTOs.Contract;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ApartmentManagement.Services.Impl
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;
        private readonly IWebHostEnvironment _env;

        // Tiêm IWebHostEnvironment để lấy đường dẫn thư mục gốc của dự án
        public ContractService(IContractRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<IEnumerable<ContractDTO>> GetAllContractsAsync()
        {
            var contracts = await _repository.GetAllAsync();
            return contracts.Select(c => MapToDTO(c));
        }

        public async Task<ContractDTO?> GetContractByIdAsync(int id)
        {
            var contract = await _repository.GetByIdAsync(id);
            return contract == null ? null : MapToDTO(contract);
        }

        public async Task<ContractDTO> CreateContractAsync(CreateContractDTO dto)
        {
            string documentUrl = "";

            // Xử lý upload file PDF nếu có đính kèm
            if (dto.DocumentFile != null && dto.DocumentFile.Length > 0)
            {
                // Tìm đến thư mục wwwroot/contracts (Tự tạo nếu chưa có)
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "contracts");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Gắn mã Guid để tên file là độc nhất vô nhị
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.DocumentFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Copy file từ Request vào ổ cứng
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.DocumentFile.CopyToAsync(fileStream);
                }

                // Đường dẫn tương đối lưu vào DB
                documentUrl = $"/contracts/{uniqueFileName}";
            }

            // Tạo thực thể Hợp đồng
            var contract = new Contract
            {
                ApartmentId = dto.ApartmentId,
                ResidentId = dto.ResidentId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DepositAmount = dto.DepositAmount,
                RentAmount = dto.RentAmount,
                Status = ContractStatus.Active, // Mặc định hợp đồng mới là Active
                DocumentUrl = documentUrl
            };

            var created = await _repository.AddAsync(contract);

            return new ContractDTO
            {
                Id = created.Id,
                ApartmentId = created.ApartmentId,
                ResidentId = created.ResidentId,
                StartDate = created.StartDate,
                EndDate = created.EndDate,
                DepositAmount = created.DepositAmount,
                RentAmount = created.RentAmount,
                Status = created.Status.ToString(),
                DocumentUrl = created.DocumentUrl
            };
        }

        // Hàm phụ trợ giúp chuyển đổi Entity sang DTO cho gọn code
        private ContractDTO MapToDTO(Contract c)
        {
            return new ContractDTO
            {
                Id = c.Id,
                ApartmentId = c.ApartmentId,
                ApartmentNumber = c.Apartment?.ApartmentNumber ?? "",
                ResidentId = c.ResidentId,
                ResidentName = c.Resident?.FullName ?? "",
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                DepositAmount = c.DepositAmount,
                RentAmount = c.RentAmount,
                Status = c.Status.ToString(),
                DocumentUrl = c.DocumentUrl
            };
        }
    }
}