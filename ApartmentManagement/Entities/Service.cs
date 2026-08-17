using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Điện, Nước, Phí quản lý...

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; } = string.Empty; // kWh, khối, tháng

        public bool IsActive { get; set; } = true;

        public ICollection<ServiceUsage>? ServiceUsages { get; set; }
    }
}