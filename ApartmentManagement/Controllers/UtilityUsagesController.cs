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

            var result = await _service.CreateUsageAsync(dto);
            return Ok(new { Message = "Ghi chỉ số thành công!", Data = result });
        }
    }
}