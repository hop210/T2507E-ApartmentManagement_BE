using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment> AddAsync(Payment payment);
    }
}