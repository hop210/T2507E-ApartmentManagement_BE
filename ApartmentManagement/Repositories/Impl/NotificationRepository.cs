using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            // Lấy danh sách và sắp xếp thông báo mới nhất lên đầu
            return await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notification> AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}