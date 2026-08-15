using ApartmentManagement.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Apartment
{
    public class CreateApartmentDTO
    {
        [Required(ErrorMessage = "Số phòng không được để trống.")]
        public string ApartmentNumber { get; set; } = string.Empty;

        [Required]
        [Range(10.0, 500.0, ErrorMessage = "Diện tích phải nằm trong khoảng từ 10m2 đến 500m2.")]
        public double Area { get; set; }

        [Required]
        [Range(1000000, 100000000, ErrorMessage = "Giá thuê phải từ 1 triệu đến 100 triệu.")]
        public decimal RentPrice { get; set; }

        public ApartmentStatus Status { get; set; } = ApartmentStatus.Available;

        [Required(ErrorMessage = "Vui lòng chọn tòa nhà.")]
        public int BuildingId { get; set; }
    }
}