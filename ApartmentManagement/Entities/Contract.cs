using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApartmentManagement.Enums;
// hợp đồng
namespace ApartmentManagement.Entities
{
    public class Contract
    {
        [Key]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }

        public int ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public Resident? Resident { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DepositAmount { get; set; } // Tiền cọc

        [Column(TypeName = "decimal(18,2)")]
        public decimal RentAmount { get; set; } // Giá thuê chốt trong hợp đồng

        public ContractStatus Status { get; set; } = ContractStatus.Active;

        [MaxLength(255)]
        public string DocumentUrl { get; set; } = string.Empty; // Lưu đường dẫn file PDF trên MinIO

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}