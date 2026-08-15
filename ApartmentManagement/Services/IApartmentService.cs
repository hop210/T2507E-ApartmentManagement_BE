using ApartmentManagement.DTOs.Apartment;
using ApartmentManagement.DTOs.Apartment.Parameters;

namespace ApartmentManagement.Services
{
    public interface IApartmentService
    {
        Task<IEnumerable<ApartmentDTO>> GetAllApartmentsAsync(ApartmentParameters parameters);
        Task<ApartmentDTO> CreateApartmentAsync(CreateApartmentDTO dto);
        Task<ApartmentDTO?> GetApartmentByIdAsync(int id);
        Task<bool> UpdateApartmentAsync(int id, UpdateApartmentDTO dto);
        Task<bool> DeleteApartmentAsync(int id);
    }
}