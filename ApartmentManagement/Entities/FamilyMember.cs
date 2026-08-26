using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class FamilyMember
    {
        [Key]
        public int Id { get; set; }

        public int ResidentId { get; set; }
        [ForeignKey("ResidentId")]
        public Resident? Resident { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Relationship { get; set; } = string.Empty; // Vợ, Chồng, Con, Anh/Chị, Bạn bè,...

        [MaxLength(20)]
        public string IdentityCard { get; set; } = string.Empty; // CCCD (nếu có)

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}