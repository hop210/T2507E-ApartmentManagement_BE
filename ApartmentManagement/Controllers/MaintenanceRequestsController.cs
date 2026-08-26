using ApartmentManagement.DTOs.Maintenance;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRequestsController : ControllerBase
    {
        private readonly IMaintenanceRequestService _service;

        public MaintenanceRequestsController(IMaintenanceRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _service.GetRequestByIdAsync(id);
            if (request == null) return NotFound("Không tìm thấy yêu cầu bảo trì này.");
            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMaintenanceRequestDTO dto)
        {
         
            var result = await _service.CreateRequestAsync(dto);
            return Ok(new { message = "Gửi yêu cầu bảo trì thành công!", data = result });
        }

        [HttpPut("{id}/assign")]
        public async Task<IActionResult> AssignStaff(int id, [FromBody] AssignMaintenanceDTO dto)
        {
            var result = await _service.AssignStaffAsync(id, dto);
            if (!result) return NotFound("Không tìm thấy yêu cầu bảo trì này.");
            return Ok(new { message = "Đã phân công nhân viên kỹ thuật thành công!" });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromForm] UpdateMaintenanceStatusDTO dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto);
            if (!result) return NotFound("Không tìm thấy yêu cầu bảo trì này.");
            return Ok(new { message = "Đã cập nhật tiến độ bảo trì!" });
        }
    }
}