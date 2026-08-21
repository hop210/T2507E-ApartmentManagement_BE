using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class Resident
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(100)]
        public string IdentityCard { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Khóa ngoại liên kết lên Căn hộ
        public int? ApartmentId { get; set; }
        [ForeignKey("ApartmentId")]
        public Apartment? Apartment { get; set; }
    }
}