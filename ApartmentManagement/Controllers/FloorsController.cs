using ApartmentManagement.DTOs.Floor;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly IFloorService _service;

        public FloorsController(IFloorService service)
        {
            _service = service;
        }

        [HttpGet("building/{buildingId}")]
        public async Task<IActionResult> GetAllByBuildingId(int buildingId)
        {
            var floors = await _service.GetAllByBuildingIdAsync(buildingId);
            return Ok(floors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var floor = await _service.GetByIdAsync(id);
            if (floor == null) return NotFound("Không tìm thấy tầng này.");
            return Ok(floor);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFloorDTO dto)
        {
            var result = await _service.CreateFloorAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFloorDTO dto)
        {
            var isUpdated = await _service.UpdateFloorAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy tầng để cập nhật.");
            return Ok("Cập nhật tầng thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteFloorAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy tầng để xóa.");
            return Ok("Đã xóa tầng thành công!");
        }
    }
}