using ApartmentManagement.DTOs.User;

namespace ApartmentManagement.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
        Task<bool> UpdateUserAsync(int id, UpdateUserDTO dto);
    }
}