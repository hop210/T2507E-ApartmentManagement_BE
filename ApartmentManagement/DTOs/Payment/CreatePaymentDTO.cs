using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Payment
{
    public class CreatePaymentDTO
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Range(typeof(decimal), "1", "10000000000", ErrorMessage = "Số tiền thanh toán phải lớn hơn 0")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        public string ReferenceCode { get; set; } = string.Empty;
    }
}