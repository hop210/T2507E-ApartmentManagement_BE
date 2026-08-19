using ApartmentManagement.DTOs.User;
using ApartmentManagement.Entities;
using ApartmentManagement.Repositories;
using Isopoh.Cryptography.Argon2;

namespace ApartmentManagement.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt
            });
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO dto)
        {
            // Kiểm tra trùng lặp Username
            var existingUser = await _repository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new ArgumentException("Tên đăng nhập đã tồn tại trong hệ thống.");
            }

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = Argon2.Hash(dto.Password), // Băm mật khẩu an toàn
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role.ToUpper(),
                IsActive = true // Mặc định tạo ra là hoạt động
            };

            var created = await _repository.AddAsync(user);

            return new UserDTO
            {
                Id = created.Id,
                Username = created.Username,
                FullName = created.FullName,
                Email = created.Email,
                Phone = created.Phone,
                Role = created.Role,
                IsActive = created.IsActive,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDTO dto)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null) return false;

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Role = dto.Role.ToUpper();
            user.IsActive = dto.IsActive; // Admin có quyền Khóa/Mở khóa ở đây

            await _repository.UpdateAsync(user);
            return true;
        }
    }
}