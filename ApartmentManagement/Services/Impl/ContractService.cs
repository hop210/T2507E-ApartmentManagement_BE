using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Contract;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Minio;
using Minio.DataModel.Args;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class ContractService : IContractService
    {
        private readonly IContractRepository _repository;
        private readonly IMinioClient _minioClient;
        private readonly ApplicationDbContext _context;

        // Tiêm IMinioClient để giao tiếp với server lưu trữ MinIO
        public ContractService(
            IContractRepository repository,
            IMinioClient minioClient,
            ApplicationDbContext context)
        {
            _repository = repository;
            _minioClient = minioClient;
            _context = context;
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
            // 1. Lấy và kiểm tra thông tin Căn hộ
            var apartment = await _context.Apartments.FindAsync(dto.ApartmentId);
            if (apartment == null)
            {
                throw new Exception("Căn hộ không tồn tại trong hệ thống.");
            }

            if (apartment.Status != ApartmentStatus.Available)
            {
                // Ném lỗi ngay nếu phòng đang có người ở hoặc đang bảo trì
                throw new Exception($"Căn hộ đang ở trạng thái {apartment.Status}, không thể tạo hợp đồng mới! Vui lòng chọn phòng trống.");
            }

            // 2. Lấy và kiểm tra thông tin Cư dân
            var resident = await _context.Residents.FindAsync(dto.ResidentId);
            if (resident == null)
            {
                throw new Exception("Cư dân không tồn tại.");
            }
            if (resident.ApartmentId != null)
            {
                throw new Exception($"Cư dân này đã được xếp vào phòng (ID Phòng: {resident.ApartmentId}). Không thể tạo thêm hợp đồng!");
            }

            string documentUrl = "";

            // 3. Xử lý upload file PDF lên MinIO
            if (dto.DocumentFile != null && dto.DocumentFile.Length > 0)
            {
                string bucketName = "contracts";

                // Kiểm tra bucket tồn tại chưa, chưa có thì tự động tạo
                bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
                if (!found)
                {
                    await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
                } // <--- CHÚ Ý: Ngoặc đóng của khối if nằm ngay đây, ngắt lệnh tạo Bucket.

                // Đưa lệnh SetPolicy ra ngoài if để hệ thống LUÔN LUÔN cấp quyền Public cho dù Bucket đã tạo từ trước hay chưa.
                string policyJson = $@"{{""Version"":""2012-10-17"",""Statement"":[{{""Action"":[""s3:GetObject""],""Effect"":""Allow"",""Principal"":{{""AWS"":[""*""]}},""Resource"":[""arn:aws:s3:::{bucketName}/*""]}}]}}";
                await _minioClient.SetPolicyAsync(new SetPolicyArgs().WithBucket(bucketName).WithPolicy(policyJson));

                // Gắn mã Guid để tên file không bao giờ bị trùng
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.DocumentFile.FileName;

                // Bắt đầu đẩy file lên trạm MinIO
                using (var stream = dto.DocumentFile.OpenReadStream())
                {
                    await _minioClient.PutObjectAsync(new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(uniqueFileName)
                        .WithStreamData(stream)
                        .WithObjectSize(stream.Length)
                        .WithContentType(dto.DocumentFile.ContentType));
                }

                // Lấy đường dẫn trực tiếp từ MinIO lưu vào DB
                documentUrl = $"http://localhost:9000/{bucketName}/{uniqueFileName}";
            }

            // 4. Tạo thực thể Hợp đồng
            var contract = new Contract
            {
                ApartmentId = dto.ApartmentId,
                ResidentId = dto.ResidentId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DepositAmount = dto.DepositAmount,
                RentAmount = dto.RentAmount,
                Status = ContractStatus.Active,
                DocumentUrl = documentUrl
            };

            var created = await _repository.AddAsync(contract);

            // 5. Cập nhật lại phòng thành "Đã cho thuê" sau khi ký hợp đồng
            apartment.Status = ApartmentStatus.Rented;
            _context.Apartments.Update(apartment);

            // 6. Trao chìa khóa: Cập nhật phòng cho Cư dân
            resident.ApartmentId = apartment.Id;
            _context.Residents.Update(resident);

            // 7. Lưu toàn bộ 3 thay đổi trên vào DB cùng lúc
            await _context.SaveChangesAsync();

            // 8. Trả về kết quả hiển thị tên đầy đủ
            return new ContractDTO
            {
                Id = created.Id,
                ApartmentId = created.ApartmentId,
                ApartmentNumber = apartment.ApartmentNumber,
                ResidentId = created.ResidentId,
                ResidentName = resident.FullName,
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



        public async Task<ContractDTO> TransferContractToFamilyMemberAsync(int familyMemberId, CreateContractDTO newContractDto)
        {
            // Bật khiên Transaction: Thành công thì lưu tất, lỗi thì hủy hết!
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Tìm thông tin người nhà (người chuẩn bị được "thăng cấp")
                var familyMember = await _context.FamilyMembers
                    .Include(f => f.Resident)
                    .FirstOrDefaultAsync(f => f.Id == familyMemberId);

                if (familyMember == null || !familyMember.IsActive)
                    throw new Exception("Không tìm thấy thông tin người nhà hoặc người này đã dọn đi.");

                var oldResident = familyMember.Resident;
                if (oldResident == null)
                    throw new Exception("Không tìm thấy chủ hộ cũ.");

                // 2. Tìm và Thanh lý hợp đồng của chủ hộ cũ
                var oldContract = await _context.Contracts
                    .FirstOrDefaultAsync(c => c.ResidentId == oldResident.Id && c.Status == ContractStatus.Active);

                if (oldContract != null)
                {
                    oldContract.Status = ContractStatus.Terminated;
                    oldContract.EndDate = DateTime.Now;
                    _context.Contracts.Update(oldContract);

                    // BỔ SUNG FIX LỖI: Tạm thời chuyển phòng thành Available để lọt qua được cửa ải của hàm CreateContractAsync
                    var apartment = await _context.Apartments.FindAsync(newContractDto.ApartmentId);
                    if (apartment != null)
                    {
                        apartment.Status = ApartmentStatus.Available;
                        _context.Apartments.Update(apartment);
                    }
                }

                // 3. Vô hiệu hóa Chủ hộ cũ và Cấp bậc Người nhà cũ
                oldResident.IsActive = false;
                oldResident.ApartmentId = null; // Trả lại phòng
                _context.Residents.Update(oldResident);

                familyMember.IsActive = false; // Vô hiệu hóa vì họ sắp chuyển sinh thành Resident
                _context.FamilyMembers.Update(familyMember);

                // 4. "Chuyển sinh": Tạo Chủ hộ mới từ dữ liệu người nhà
                var newResident = new Resident
                {
                    FullName = familyMember.FullName,
                    IdentityCard = familyMember.IdentityCard,
                    PhoneNumber = "", // Người nhà tạm thời chưa có sđt, có thể update sau
                    ApartmentId = null,
                    IsActive = true
                };
                _context.Residents.Add(newResident);

                // Lưu tạm để Database sinh ra cái ID cho tân chủ hộ
                await _context.SaveChangesAsync();

                // 5. Gắn ID mới vào DTO và "Tái sử dụng" luồng tạo hợp đồng xịn sò đã có
                newContractDto.ResidentId = newResident.Id;

                // Gọi lại chính hàm CreateContractAsync (có cả MinIO) để xử lý từ A-Z
                var newContract = await CreateContractAsync(newContractDto);

                // 6. Hoàn tất mĩ mãn, chốt sổ Transaction!
                await transaction.CommitAsync();

                return newContract;
            }
            catch (Exception ex)
            {
                // Có bất kỳ lỗi gì xảy ra, quay xe ngay lập tức!
                await transaction.RollbackAsync();
                throw new Exception($"Giao dịch thất bại, đã hoàn tác dữ liệu. Lỗi: {ex.Message}");
            }
        }
    }
}