using ApartmentManagement.DTOs.Contract;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _service;

        public ContractsController(IContractService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contracts = await _service.GetAllContractsAsync();
            return Ok(contracts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contract = await _service.GetContractByIdAsync(id);
            if (contract == null) return NotFound("Không tìm thấy hợp đồng này.");
            return Ok(contract);
        }

        [HttpPost]
        // Bắt buộc dùng [FromForm] để API có thể đọc được file đính kèm từ request
        public async Task<IActionResult> Create([FromForm] CreateContractDTO dto)
        {
            // Validate nhẹ: Ngày bắt đầu không thể sau ngày kết thúc
            if (dto.StartDate >= dto.EndDate)
            {
                return BadRequest("Ngày kết thúc hợp đồng phải lớn hơn ngày bắt đầu.");
            }

            try
            {
                // Gọi xuống Service để xử lý nghiệp vụ tạo hợp đồng
                var result = await _service.CreateContractAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Nếu Service ném ra lỗi (Ví dụ: "Cư dân này đã được xếp vào phòng..."),
                // Lưới catch này sẽ tóm lấy và trả về mã 400 Bad Request kèm theo lời nhắn lỗi
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPut("{id}/extend")]
        public async Task<IActionResult> ExtendContract(int id, [FromBody] ExtendContractDTO dto)
        {
            var result = await _service.ExtendContractAsync(id, dto);
            if (!result) return NotFound("Không tìm thấy hợp đồng hợp lệ để gia hạn.");
            return Ok(new { message = "Đã gia hạn hợp đồng thành công!" });
        }

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPut("{id}/terminate")]
        public async Task<IActionResult> TerminateContract(int id)
        {
            var result = await _service.TerminateContractAsync(id);
            if (!result) return NotFound("Không tìm thấy hợp đồng.");
            return Ok(new { message = "Đã thanh lý hợp đồng và cập nhật trạng thái phòng thành công!" });
        }



        // Chuyển nhượng hợp đồng cho Người nhà (Tạo Chủ hộ mới, thanh lý HĐ cũ)
        // ID của người nhà sẽ được thăng cấp
        // Thông tin hợp đồng mới (kèm file PDF)
        [Authorize(Roles = "ADMIN,MANAGER")] // Chỉ quản lý mới được phép thao tác
        [HttpPost("transfer/{familyMemberId}")]
        public async Task<IActionResult> TransferContract(int familyMemberId, [FromForm] CreateContractDTO newContractDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Gọi "Super API" từ tầng Service mà chúng ta vừa viết
                var newContract = await _service.TransferContractToFamilyMemberAsync(familyMemberId, newContractDto);

                return Ok(new
                {
                    Message = "Chuyển nhượng thành công! Đã tạo chủ hộ mới và hợp đồng mới.",
                    Data = newContract
                });
            }
            catch (Exception ex)
            {
                // Bắt lỗi và hiển thị lên Swagger (ví dụ lỗi không đủ tuổi, không tìm thấy người nhà...)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}