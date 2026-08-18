using ApartmentManagement.DTOs.Maintenance;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ApartmentManagement.Services.Impl
{
    public class MaintenanceRequestService : IMaintenanceRequestService
    {
        private readonly IMaintenanceRequestRepository _repository;
        private readonly IWebHostEnvironment _env;

        public MaintenanceRequestService(IMaintenanceRequestRepository repository, IWebHostEnvironment env)
        {
            _repository = repository;
            _env = env;
        }

        public async Task<IEnumerable<MaintenanceRequestDTO>> GetAllRequestsAsync()
        {
            var requests = await _repository.GetAllAsync();
            return requests.Select(MapToDTO);
        }

        public async Task<MaintenanceRequestDTO?> GetRequestByIdAsync(int id)
        {
            var request = await _repository.GetByIdAsync(id);
            return request == null ? null : MapToDTO(request);
        }

        public async Task<MaintenanceRequestDTO> CreateRequestAsync(CreateMaintenanceRequestDTO dto)
        {
            var images = new List<MaintenanceImage>();

            // Xử lý vòng lặp lưu nhiều file ảnh cùng lúc
            if (dto.ImageFiles != null && dto.ImageFiles.Any())
            {
                var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadsFolder = Path.Combine(webRootPath, "images", "maintenance");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Khởi tạo đối tượng Image và nhét vào danh sách
                        images.Add(new MaintenanceImage
                        {
                            ImageUrl = $"/images/maintenance/{uniqueFileName}"
                        });
                    }
                }
            }

            // Khởi tạo đối tượng Yêu cầu bảo trì tổng
            var request = new MaintenanceRequest
            {
                ApartmentId = dto.ApartmentId,
                ResidentId = dto.ResidentId,
                Description = dto.Description,
                Status = MaintenanceStatus.Pending, // Yêu cầu mới luôn ở trạng thái Chờ xử lý
                Images = images // Gắn nguyên danh sách ảnh vào đây, EF Core sẽ lo phần còn lại
            };

            var created = await _repository.AddAsync(request);

            return MapToDTO(created);
        }

        // Hàm Mapping dùng chung giúp code Clean hơn
        private MaintenanceRequestDTO MapToDTO(MaintenanceRequest request)
        {
            return new MaintenanceRequestDTO
            {
                Id = request.Id,
                ApartmentId = request.ApartmentId,
                ApartmentNumber = request.Apartment?.ApartmentNumber ?? "",
                ResidentId = request.ResidentId,
                ResidentName = request.Resident?.FullName ?? "",
                Description = request.Description,
                Status = request.Status.ToString(),
                CreatedAt = request.CreatedAt,
                // Rút trích danh sách đường link ảnh trả về cho Frontend
                ImageUrls = request.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
            };
        }
    }
}