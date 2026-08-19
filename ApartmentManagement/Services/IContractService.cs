using ApartmentManagement.DTOs.Contract;

namespace ApartmentManagement.Services
{
    public interface IContractService
    {
        Task<IEnumerable<ContractDTO>> GetAllContractsAsync();
        Task<ContractDTO?> GetContractByIdAsync(int id);
        Task<ContractDTO> CreateContractAsync(CreateContractDTO dto);
        Task<bool> ExtendContractAsync(int id, ExtendContractDTO dto);
        Task<bool> TerminateContractAsync(int id);
    }
}