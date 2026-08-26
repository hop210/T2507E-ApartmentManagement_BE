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
        public string ApartmentNumber { get; set; } = string.Empty;

        public double Area { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RentPrice { get; set; }

        public ApartmentStatus Status { get; set; } = ApartmentStatus.Available;

        public int MaxCapacity { get; set; } = 4;

        // Khóa ngoại liên kết lên Tầng
        public int FloorId { get; set; }
        [ForeignKey("FloorId")]
        public Floor? Floor { get; set; }

        // Liên kết với Cư dân
        public ICollection<Resident>? Residents { get; set; }
    }
}