using ApartmentManagement.Enums;

namespace ApartmentManagement.DTOs.Apartment.Parameters
{
    public class ApartmentParameters
    {// --- Thiết lập Phân trang mặc định ---
        public int PageNumber { get; set; } = 1; // Mặc định ở trang 1
        public int PageSize { get; set; } = 5;   // Mặc định trả về 5 phòng mỗi trang

        // --- Các tiêu chí Lọc (Filter) ---
        // Dùng dấu "?" (nullable) để biểu thị: Nếu người dùng không nhập thì không lọc
        public ApartmentStatus? Status { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
