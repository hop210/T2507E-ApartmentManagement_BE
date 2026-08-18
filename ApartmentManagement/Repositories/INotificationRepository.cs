using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification> AddAsync(Notification notification);
    }
}