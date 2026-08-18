using ApartmentManagement.Entities;

namespace ApartmentManagement.Repositories
{
    public interface IContractRepository
    {
        Task<IEnumerable<Contract>> GetAllAsync();
        Task<Contract?> GetByIdAsync(int id);
        Task<Contract> AddAsync(Contract contract);
    }
}