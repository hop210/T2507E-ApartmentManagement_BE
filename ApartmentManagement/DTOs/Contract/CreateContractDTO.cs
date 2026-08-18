using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Contract
{
    public class CreateContractDTO
    {
        [Required] public int ApartmentId { get; set; }
        [Required] public int ResidentId { get; set; }
        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }
        [Required] public decimal DepositAmount { get; set; }
        [Required] public decimal RentAmount { get; set; }

        // Nhận file upload (IFormFile là kiểu dữ liệu chuẩn của ASP.NET để hứng file)
        public IFormFile? DocumentFile { get; set; }
    }
}
