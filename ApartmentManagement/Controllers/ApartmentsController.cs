using ApartmentManagement.DTOs.Apartment;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentService _service;

        public ApartmentsController(IApartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apartments = await _service.GetAllApartmentsAsync();
            return Ok(apartments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApartmentDTO dto)
        {
            var result = await _service.CreateApartmentAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apartment = await _service.GetApartmentByIdAsync(id);
            if (apartment == null) return NotFound("Không tìm thấy căn hộ này.");
            return Ok(apartment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateApartmentDTO dto)
        {
            var isUpdated = await _service.UpdateApartmentAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy căn hộ để cập nhật.");

            return Ok("Cập nhật thông tin căn hộ thành công!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteApartmentAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy căn hộ để xóa.");

            return Ok("Đã xóa căn hộ thành công!");
        }
    }
}