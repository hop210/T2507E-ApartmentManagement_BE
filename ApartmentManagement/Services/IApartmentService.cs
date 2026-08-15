using ApartmentManagement.DTOs.Apartment;

namespace ApartmentManagement.Services
{
    public interface IApartmentService
    {
        Task<IEnumerable<ApartmentDTO>> GetAllApartmentsAsync();
        Task<ApartmentDTO> CreateApartmentAsync(CreateApartmentDTO dto);
        Task<ApartmentDTO?> GetApartmentByIdAsync(int id);
        Task<bool> UpdateApartmentAsync(int id, UpdateApartmentDTO dto);
        Task<bool> DeleteApartmentAsync(int id);
    }
}