using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApartmentManagement.Enums;
//Bảo trì
namespace ApartmentManagement.Entities
{
    public class MaintenanceRequest
    {
        [Key]
        public int Id { get; set; }

        public int ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }

        public int ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public Resident? Resident { get; set; }

        public int? AssignedStaffId { get; set; }
        [ForeignKey("AssignedStaffId")]
        public User? AssignedStaff { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<MaintenanceImage>? Images { get; set; }
    }
}