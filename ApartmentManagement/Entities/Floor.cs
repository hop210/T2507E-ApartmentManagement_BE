using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApartmentManagement.Entities
{
    public class Floor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FloorNumber { get; set; } = string.Empty; // VD: Tầng 1, Tầng 2

        // Khóa ngoại liên kết lên Tòa nhà
        public int BuildingId { get; set; }
        [ForeignKey("BuildingId")]
        public Building? Building { get; set; }

        // Một Tầng có nhiều Căn hộ
        public ICollection<Apartment>? Apartments { get; set; }
    }
}