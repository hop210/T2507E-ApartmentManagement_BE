namespace ApartmentManagement.DTOs.Invoice
{
    public class InvoiceDetailDTO
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
