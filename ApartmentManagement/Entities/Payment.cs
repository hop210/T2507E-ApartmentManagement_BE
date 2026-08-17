using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//thanh toán
namespace ApartmentManagement.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice? Invoice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "CASH"; // Tiền mặt, Chuyển khoản
    }
}