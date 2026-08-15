using ApartmentManagement.DTOs.Tenant;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _service;

        public TenantsController(ITenantService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _service.GetAllTenantsAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenant = await _service.GetTenantByIdAsync(id);
            if (tenant == null) return NotFound("Không tìm thấy cư dân này.");
            return Ok(tenant);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantDTO dto)
        {
            var result = await _service.CreateTenantAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantDTO dto)
        {
            var isUpdated = await _service.UpdateTenantAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy cư dân để cập nhật.");
            return Ok("Cập nhật thông tin thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteTenantAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy cư dân để xóa.");
            return Ok("Đã xóa cư dân thành công!");
        }
    }
}