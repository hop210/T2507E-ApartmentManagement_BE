using ApartmentManagement.DTOs.User;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    // Cực kỳ quan trọng: Chỉ có Role ADMIN mới được gọi các API này
    [Authorize(Roles = "ADMIN")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _service.GetUserByIdAsync(id);
            if (user == null) return NotFound("Không tìm thấy tài khoản.");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
        {
            var result = await _service.CreateUserAsync(dto);
            return Ok(new { message = "Tạo tài khoản thành công!", data = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto)
        {
            var result = await _service.UpdateUserAsync(id, dto);
            if (!result) return NotFound("Không tìm thấy tài khoản.");

            var statusStr = dto.IsActive ? "Cập nhật" : "Khóa";
            return Ok(new { message = $"Đã {statusStr} tài khoản thành công!" });
        }
    }
}