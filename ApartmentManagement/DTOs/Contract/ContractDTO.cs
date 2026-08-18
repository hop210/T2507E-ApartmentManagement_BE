namespace ApartmentManagement.DTOs.Contract
{
    public class ContractDTO
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public string ApartmentNumber { get; set; } = string.Empty; // Trả thêm tên phòng cho dễ nhìn
        public int ResidentId { get; set; }
        public string ResidentName { get; set; } = string.Empty; // Trả thêm tên người thuê
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal RentAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty; // Đường dẫn tải file PDF
    }
}
