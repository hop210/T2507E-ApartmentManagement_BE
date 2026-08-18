using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Resident> Residents { get; set; }

        // Các bảng mới thêm
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<UtilityUsage> UtilityUsages { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
        public DbSet<MaintenanceImage> MaintenanceImages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quy tắc xóa cho Tòa nhà -> Tầng -> Căn hộ (Cascade)
            modelBuilder.Entity<Building>()
                .HasMany(b => b.Floors)
                .WithOne(f => f.Building)
                .HasForeignKey(f => f.BuildingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Floor>()
                .HasMany(f => f.Apartments)
                .WithOne(a => a.Floor)
                .HasForeignKey(a => a.FloorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình chống xóa dây chuyền (Restrict) cho các bảng phức tạp
            // Không cho phép tự động xóa Hợp đồng nếu lỡ tay xóa Cư dân hoặc Căn hộ
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Apartment)
                .WithMany()
                .HasForeignKey(c => c.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Resident)
                .WithMany()
                .HasForeignKey(c => c.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tương tự cho Yêu cầu sửa chữa
            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(m => m.Apartment)
                .WithMany()
                .HasForeignKey(m => m.ApartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(m => m.Resident)
                .WithMany()
                .HasForeignKey(m => m.ResidentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaintenanceRequest>()
                .HasOne(m => m.AssignedStaff)
                .WithMany()
                .HasForeignKey(m => m.AssignedStaffId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}