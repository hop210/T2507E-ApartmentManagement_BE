using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Maintenance
{
    public class AssignMaintenanceDTO // Quản lý gán việc
    {
        [Required(ErrorMessage = "Vui lòng chọn nhân viên kỹ thuật")]
        public int StaffId { get; set; }
    }
}