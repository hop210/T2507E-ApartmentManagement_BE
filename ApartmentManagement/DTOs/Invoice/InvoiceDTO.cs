namespace ApartmentManagement.DTOs.Invoice
{
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty; // Unpaid, Paid, Overdue
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }

        // Danh sách chi tiết hóa đơn (Tiền nhà, Tiền điện, Tiền nước...)
        public List<InvoiceDetailDTO> Details { get; set; } = new List<InvoiceDetailDTO>();
    }
}
