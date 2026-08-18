using ApartmentManagement.DTOs.Utility;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IUtilityService _service;

        public UtilitiesController(IUtilityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var utilities = await _service.GetAllUtilitiesAsync();
            return Ok(utilities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var utility = await _service.GetUtilityByIdAsync(id);
            if (utility == null) return NotFound("Không tìm thấy tiện ích này.");
            return Ok(utility);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilityDTO dto)
        {
            var result = await _service.CreateUtilityAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUtilityDTO dto)
        {
            var isUpdated = await _service.UpdateUtilityAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy tiện ích để cập nhật.");
            return Ok("Cập nhật tiện ích thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteUtilityAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy tiện ích để xóa.");
            return Ok("Đã xóa tiện ích thành công!");
        }
    }
}