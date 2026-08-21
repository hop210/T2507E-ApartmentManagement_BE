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
            try
            {
                // Thử gọi hàm tạo (hoặc hồi sinh) Cư dân
                var result = await _service.CreateResidentAsync(dto);
                return Ok(result); // Trả về 200 OK nếu thành công
            }
            catch (Exception ex)
            {
                // Nếu Service ném ra lỗi (ví dụ: CCCD đã tồn tại và đang hoạt động)
                // Hệ thống sẽ bắt lấy và trả về mã 400 kèm câu thông báo lỗi
                return BadRequest(new { message = ex.Message });
            }
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