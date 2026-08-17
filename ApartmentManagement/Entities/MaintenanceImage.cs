using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class MaintenanceImage
    {
        [Key]
        public int Id { get; set; }

        public int MaintenanceRequestId { get; set; }
        [ForeignKey("MaintenanceRequestId")]
        public MaintenanceRequest? MaintenanceRequest { get; set; }

        [Required]
        [MaxLength(255)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}