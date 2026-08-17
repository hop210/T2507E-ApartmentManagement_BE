using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class ServiceUsage
    {
        [Key]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public double OldIndicator { get; set; } // Chỉ số cũ (Ví dụ: điện tháng trước)
        public double NewIndicator { get; set; } // Chỉ số mới
        public double UsageAmount { get; set; }  // Mức tiêu thụ thực tế
    }
}