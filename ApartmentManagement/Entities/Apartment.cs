using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApartmentManagement.Enums;

namespace ApartmentManagement.Entities
{
    public class Apartment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string ApartmentNumber { get; set; } = string.Empty; // Số phòng (VD: P101, A-202)

        public double Area { get; set; } // Diện tích (m2)

        [Column(TypeName = "decimal(18,2)")]
        public decimal RentPrice { get; set; } // Giá thuê

        // Thêm trường Status
        public ApartmentStatus Status { get; set; } = ApartmentStatus.Available;

        // --- Bắt đầu phần thiết lập Khóa ngoại (Foreign Key) ---
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public Building? Building { get; set; }
        public ICollection<Tenant>? Tenants { get; set; }
    }
}