using ApartmentManagement.DTOs.Apartment.Parameters;
using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IApartmentRepository
    {
        Task<IEnumerable<Apartment>> GetAllAsync(ApartmentParameters parameters);
        Task<Apartment> AddAsync(Apartment apartment);
        Task<Apartment?> GetByIdAsync(int id);
        Task UpdateAsync(Apartment apartment);
        Task DeleteAsync(Apartment apartment);
    }
}