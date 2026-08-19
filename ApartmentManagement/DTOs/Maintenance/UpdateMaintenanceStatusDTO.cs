using System.ComponentModel.DataAnnotations;
using ApartmentManagement.Enums;

namespace ApartmentManagement.DTOs.Maintenance
{
    public class UpdateMaintenanceStatusDTO // Kỹ thuật báo cáo tiến độ
    {
        [Required]
        public MaintenanceStatus Status { get; set; }
        public List<IFormFile>? ResultImages { get; set; }
    }
}