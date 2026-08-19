using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Profile;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;

        public ProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDTO?> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;

            return new UserProfileDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.Phone,
                Role = user.Role.ToString()
            };
        }

        public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDTO dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.PhoneNumber;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");

            // Kiểm tra mật khẩu cũ bằng Argon2
            if (!Argon2.Verify(user.PasswordHash, dto.CurrentPassword))
            {
                throw new Exception("Mật khẩu hiện tại không chính xác.");
            }

            // Băm mật khẩu mới và lưu
            user.PasswordHash = Argon2.Hash(dto.NewPassword);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}