using ApartmentManagement.DTOs.UtilityUsage;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize(Roles = "ADMIN,MANAGER")]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityUsagesController : ControllerBase
    {
        private readonly IUtilityUsageService _service;

        public UtilityUsagesController(IUtilityUsageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usages = await _service.GetAllUsagesAsync();
            return Ok(usages);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilityUsageDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.CreateUsageAsync(dto);
                return Ok(new { Message = "Ghi chỉ số thành công!", Data = result });
            }
            catch (ArgumentException ex)
            {
                // Chỉ số mới nhỏ hơn chỉ số cũ (Lỗi 400)
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Phòng này đã chốt số trong tháng rồi (Lỗi 409 - Xung đột dữ liệu)
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Các lỗi hệ thống khác
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}