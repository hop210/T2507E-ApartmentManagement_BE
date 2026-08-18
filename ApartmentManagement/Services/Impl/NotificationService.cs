using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Notification;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly ApplicationDbContext _context;

        public NotificationService(INotificationRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync()
        {
            var notifications = await _repository.GetAllAsync();
            var result = new List<NotificationDTO>();

            foreach (var n in notifications)
            {
                string? aptNumber = null;
                // Nếu là thông báo riêng, chạy vào Database tìm số phòng ghép vào cho đẹp
                if (!n.IsGlobal && n.ApartmentId.HasValue)
                {
                    var apt = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == n.ApartmentId.Value);
                    aptNumber = apt?.ApartmentNumber;
                }

                result.Add(new NotificationDTO
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt,
                    IsGlobal = n.IsGlobal,
                    ApartmentId = n.ApartmentId,
                    ApartmentNumber = aptNumber
                });
            }

            return result;
        }

        public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto)
        {
            // Validate: Đã chọn gửi riêng thì bắt buộc phải có ID phòng
            if (!dto.IsGlobal && !dto.ApartmentId.HasValue)
            {
                throw new ArgumentException("Phải chọn một căn hộ cụ thể nếu đây không phải là thông báo chung.");
            }

            var notification = new Notification
            {
                Title = dto.Title,
                Content = dto.Content,
                IsGlobal = dto.IsGlobal,
                // Chặn lỗi người dùng: Chọn Global nhưng vẫn gửi ID phòng lên thì tự ép về null
                ApartmentId = dto.IsGlobal ? null : dto.ApartmentId
            };

            var created = await _repository.AddAsync(notification);
            string? aptNumber = null;

            if (!created.IsGlobal && created.ApartmentId.HasValue)
            {
                var apt = await _context.Apartments.FirstOrDefaultAsync(a => a.Id == created.ApartmentId.Value);
                aptNumber = apt?.ApartmentNumber;
            }

            return new NotificationDTO
            {
                Id = created.Id,
                Title = created.Title,
                Content = created.Content,
                CreatedAt = created.CreatedAt,
                IsGlobal = created.IsGlobal,
                ApartmentId = created.ApartmentId,
                ApartmentNumber = aptNumber
            };
        }
    }
}