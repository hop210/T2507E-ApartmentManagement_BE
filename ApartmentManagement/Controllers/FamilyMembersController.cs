using ApartmentManagement.DTOs.FamilyMember;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize(Roles = "ADMIN,MANAGER")]
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyMembersController : ControllerBase
    {
        private readonly IFamilyMemberService _service;

        public FamilyMembersController(IFamilyMemberService service)
        {
            _service = service;
        }

        [HttpGet("resident/{residentId}")]
        public async Task<IActionResult> GetByResident(int residentId)
        {
            var members = await _service.GetMembersByResidentAsync(residentId);
            return Ok(members);
        }

        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] CreateFamilyMemberDTO dto)
        {
            try
            {
                var result = await _service.AddMemberAsync(dto);
                return Ok(new { message = "Thêm thành viên thành công!", data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveMember(int id)
        {
            var result = await _service.RemoveMemberAsync(id);
            if (!result) return NotFound("Không tìm thấy thành viên này.");
            return Ok(new { message = "Đã xóa thành viên khỏi hộ gia đình." });
        }
    }
}