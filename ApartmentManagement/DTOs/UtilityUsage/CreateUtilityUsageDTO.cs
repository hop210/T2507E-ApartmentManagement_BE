using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.UtilityUsage
{
    public class CreateUtilityUsageDTO
    {
        [Required] public int ApartmentId { get; set; }
        [Required] public int UtilityId { get; set; }
        [Required][Range(1, 12, ErrorMessage = "Tháng phải từ 1 đến 12")] public int Month { get; set; }
        [Required] public int Year { get; set; }
        [Required][Range(0, double.MaxValue)] public double OldIndicator { get; set; }
        [Required][Range(0, double.MaxValue)] public double NewIndicator { get; set; }
    }
}
