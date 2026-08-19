using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Contract;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        // Tiêm IWebHostEnvironment để lấy đường dẫn thư mục gốc của dự án
        public ContractService(
            IContractRepository repository,
            IWebHostEnvironment env,
            ApplicationDbContext context)
        {
            _repository = repository;
            _env = env;
            _context = context; // Gán giá trị để sử dụng bên dưới
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
        public async Task<bool> ExtendContractAsync(int id, ExtendContractDTO dto)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return false;

            // Chỉ cho phép gia hạn nếu hợp đồng đang Active (Dùng Enum chuẩn theo file của bạn)
            if (contract.Status != ContractStatus.Active)
            {
                throw new Exception("Chỉ có thể gia hạn hợp đồng đang trong trạng thái Hoạt động.");
            }

            // Cập nhật ngày kết thúc
            contract.EndDate = dto.NewEndDate;

            _context.Contracts.Update(contract);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TerminateContractAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return false;

            // 1. Chuyển trạng thái hợp đồng thành Terminated (Dùng Enum)
            contract.Status = ContractStatus.Terminated; // Tên Enum này tuỳ thuộc vào file Enums/ContractStatus.cs của bạn
            contract.EndDate = DateTime.Now; // Chốt ngày thanh lý là hôm nay

            _context.Contracts.Update(contract);

            // 2. Logic trả lại phòng trống
            var apartment = await _context.Apartments.FindAsync(contract.ApartmentId);
            if (apartment != null)
            {
                // Ghi chú: Chỗ này mình dùng ApartmentStatus.Available. 
                // Bạn nhớ check file Apartment.cs xem trạng thái phòng bạn đang lưu là kiểu gì để sửa lại cho khớp nhé!
                apartment.Status = ApartmentStatus.Available;
                _context.Apartments.Update(apartment);
            }
            // 3. Logic dọn dẹp: Ngắt kết nối Cư dân khỏi căn hộ
            var resident = await _context.Residents.FindAsync(contract.ResidentId);
            if (resident != null)
            {
                // Cắt đứt liên kết phòng (Nếu Entity Resident của bạn thiết kế ApartmentId là int?)
                resident.ApartmentId = null;

                // NẾU bạn có trường trạng thái cư dân, có thể đánh dấu họ đã dọn đi
                // resident.Status = "MovedOut"; hoặc resident.IsActive = false;

                _context.Residents.Update(resident);
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}