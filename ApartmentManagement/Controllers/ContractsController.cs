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
    }
}