using ApartmentManagement.DTOs.Resident;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResidentsController : ControllerBase
    {
        private readonly IResidentService _service;

        public ResidentsController(IResidentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var residents = await _service.GetAllResidentsAsync();
            return Ok(residents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resident = await _service.GetResidentByIdAsync(id);
            if (resident == null) return NotFound("Không tìm thấy cư dân này.");
            return Ok(resident);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateResidentDTO dto)
        {
            var result = await _service.CreateResidentAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateResidentDTO dto)
        {
            var isUpdated = await _service.UpdateResidentAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy cư dân để cập nhật.");
            return Ok("Cập nhật thông tin thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteResidentAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy cư dân để xóa.");
            return Ok("Đã xóa cư dân thành công!");
        }
    }
}