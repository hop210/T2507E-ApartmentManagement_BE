using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApartmentManagement.Enums;
//Hóa đơn
namespace ApartmentManagement.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Unpaid;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } // Hạn chót thanh toán

        public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
        public ICollection<Payment>? Payments { get; set; }
    }
}