using ApartmentManagement.DTOs.Contract;

namespace ApartmentManagement.Services
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDTO>> GetAllContractsAsync();
        Task<ContractDTO?> GetContractByIdAsync(int id);
        Task<ContractDTO> CreateContractAsync(CreateContractDTO dto);
    }
}