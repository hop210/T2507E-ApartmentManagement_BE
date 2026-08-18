using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class UtilityUsage
    {
        [Key]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }

        public int UtilityId { get; set; } // Đã đổi từ ServiceId
        [ForeignKey("UtilityId")]
        public Utility? Utility { get; set; }

        public int Month { get; set; }
        public int Year { get; set; }

        public double OldIndicator { get; set; }
        public double NewIndicator { get; set; }
        public double UsageAmount { get; set; }
    }
}