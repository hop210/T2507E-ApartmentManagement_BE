namespace ApartmentManagement.DTOs.Payment
{
    public class PaymentDTO
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Bank Transfer, Momo...
        public string ReferenceCode { get; set; } = string.Empty; // Mã giao dịch ngân hàng (nếu có)
    }
}