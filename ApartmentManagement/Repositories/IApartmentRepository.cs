using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IApartmentRepository
    {
        Task<IEnumerable<Apartment>> GetAllAsync();
        Task<Apartment> AddAsync(Apartment apartment);
        Task<Apartment?> GetByIdAsync(int id);
        Task UpdateAsync(Apartment apartment);
        Task DeleteAsync(Apartment apartment);
    }
}