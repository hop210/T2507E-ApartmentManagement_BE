using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Invoice
{
    public class CreateInvoiceDTO
    {
        [Required]
        public int ApartmentId { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        // Ngày đến hạn thanh toán
        [Required]
        public DateTime DueDate { get; set; }
    }
}