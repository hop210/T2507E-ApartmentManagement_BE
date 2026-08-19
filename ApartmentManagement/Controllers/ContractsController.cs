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

            var result = await _service.CreateContractAsync(dto);
            return Ok(result);
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
    }
}