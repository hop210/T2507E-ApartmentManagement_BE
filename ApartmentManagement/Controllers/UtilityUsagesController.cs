using ApartmentManagement.DTOs.UtilityUsage;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
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
            try
            {
                var result = await _service.CreateUsageAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Bắt lỗi logic từ Service (ví dụ: số mới < số cũ, hoặc đã chốt rồi)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}