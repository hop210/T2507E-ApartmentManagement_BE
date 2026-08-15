using ApartmentManagement.Entities;
using Isopoh.Cryptography.Argon2;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // Kiểm tra xem bảng Users đã có tài khoản ADMIN nào chưa
            if (!await context.Users.AnyAsync(u => u.Role == "ADMIN"))
            {
                // Nếu chưa có, tạo mới một tài khoản Admin 
                var adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = Argon2.Hash("admin123"), // Mật khẩu mặc định là admin123
                    FullName = "System Administrator",
                    Email = "admin@apartment.com",
                    Phone = "0999999999",
                    Role = "ADMIN",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}