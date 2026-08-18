using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;

namespace ApartmentManagement.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDataAsync(ApplicationDbContext context)
        {
            // 1. Tạo tài khoản Admin mặc định (nếu chưa có)
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var adminUser = new User
                {
                    Username = "admin",
                    // Thay BCrypt bằng Argon2 cho đồng bộ với AuthController
                    PasswordHash = Argon2.Hash("admin123"),
                    FullName = "System Administrator",
                    Role = "ADMIN",
                    IsActive = true
                };
                await context.Users.AddAsync(adminUser);
            }

            // 2. Tạo dữ liệu mẫu Tòa nhà -> Tầng -> Căn hộ (nếu chưa có)
            if (!await context.Buildings.AnyAsync())
            {
                var buildingA = new Building
                {
                    Name = "Tòa A - Alpha",
                    Address = "Số 1, Đường X, Hà Nội",
                    TotalFloors = 2,
                    Floors = new List<Floor>
                    {
                        new Floor
                        {
                            FloorNumber = "Tầng 1",
                            Apartments = new List<Apartment>
                            {
                                new Apartment { ApartmentNumber = "A101", Area = 65.5, RentPrice = 5000000, Status = ApartmentStatus.Available },
                                new Apartment { ApartmentNumber = "A102", Area = 75.0, RentPrice = 6500000, Status = ApartmentStatus.Available }
                            }
                        },
                        new Floor
                        {
                            FloorNumber = "Tầng 2",
                            Apartments = new List<Apartment>
                            {
                                new Apartment { ApartmentNumber = "A201", Area = 65.5, RentPrice = 5200000, Status = ApartmentStatus.Available }
                            }
                        }
                    }
                };

                await context.Buildings.AddAsync(buildingA);
            }

            // Lưu toàn bộ dữ liệu mẫu xuống Database
            await context.SaveChangesAsync();
        }
    }
}