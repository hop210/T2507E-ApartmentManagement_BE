using ApartmentManagement.DTOs.Profile;

namespace ApartmentManagement.Services
{
    public interface IProfileService
    {
        Task<UserProfileDTO?> GetProfileAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, UpdateProfileDTO dto);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    }
}