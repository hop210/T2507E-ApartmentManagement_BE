using System.Security.Claims;
using ApartmentManagement.DTOs.Profile;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize] // Bắt buộc phải đăng nhập (có Token) mới được gọi API này
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            _service = service;
        }

        // Hàm helper dùng chung để lấy ID người dùng từ Token
        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc không chứa thông tin User ID.");
            }
            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetUserIdFromToken();
            var profile = await _service.GetProfileAsync(userId);

            if (profile == null) return NotFound("Không tìm thấy thông tin cá nhân.");
            return Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDTO dto)
        {
            var userId = GetUserIdFromToken();
            var result = await _service.UpdateProfileAsync(userId, dto);

            if (!result) return BadRequest("Cập nhật thông tin thất bại.");
            return Ok(new { message = "Cập nhật thông tin thành công!" });
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                await _service.ChangePasswordAsync(userId, dto);
                return Ok(new { message = "Đổi mật khẩu thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}