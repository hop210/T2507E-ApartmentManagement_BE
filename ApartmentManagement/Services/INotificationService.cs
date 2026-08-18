using ApartmentManagement.DTOs.Notification;

namespace ApartmentManagement.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetAllNotificationsAsync();
        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto);
    }
}