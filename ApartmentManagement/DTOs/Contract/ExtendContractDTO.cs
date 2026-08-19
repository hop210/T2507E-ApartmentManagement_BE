using System.ComponentModel.DataAnnotations;

namespace ApartmentManagement.DTOs.Contract
{
    public class ExtendContractDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc mới")]
        public DateTime NewEndDate { get; set; }
    }
}