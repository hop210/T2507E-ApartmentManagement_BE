using ApartmentManagement.DTOs.Apartment;
using ApartmentManagement.DTOs.Apartment.Parameters;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize] //  Bắt buộc phải có Token mới được vào Controller này
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IApartmentService _service;

        public ApartmentsController(IApartmentService service)
        {
            _service = service; 
        }

        // Cư dân (RESIDENT) hoặc ai có token đều được xem danh sách
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ApartmentParameters parameters)
        {
            var apartments = await _service.GetAllApartmentsAsync(parameters);
            return Ok(apartments);
        }

        //  CHỈ ADMIN VÀ MANAGER MỚI ĐƯỢC TẠO MỚI
        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApartmentDTO dto)
        {
            var result = await _service.CreateApartmentAsync(dto);
            return Ok(result);
        }

        // Ai có token cũng xem được chi tiết
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var apartment = await _service.GetApartmentByIdAsync(id);
            if (apartment == null) return NotFound("Không tìm thấy căn hộ này.");
            return Ok(apartment);
        }

        //  CHỈ ADMIN VÀ MANAGER MỚI ĐƯỢC SỬA
        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateApartmentDTO dto)
        {
            var isUpdated = await _service.UpdateApartmentAsync(id, dto);
            if (!isUpdated) return NotFound("Không tìm thấy căn hộ để cập nhật.");

            return Ok("Cập nhật thông tin căn hộ thành công!");
        }

        //  CHỈ ADMIN MỚI ĐƯỢC XÓA
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _service.DeleteApartmentAsync(id);
            if (!isDeleted) return NotFound("Không tìm thấy căn hộ để xóa.");

            return Ok("Đã xóa căn hộ thành công!");
        }
    }
}