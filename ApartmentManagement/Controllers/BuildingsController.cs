using ApartmentManagement.DTOs.Building;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
public class BuildingsController : ControllerBase
{
        private readonly IBuildingService _service;

        public BuildingsController(IBuildingService service)
        {
            _service = service;
        }

        // Lấy danh sách nhẹ (Dành cho bảng)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buildings = await _service.GetAllBuildingsAsync();
            return Ok(buildings);
        }

        // API ĐẶC BIỆT: Lấy toàn bộ sơ đồ (Dành cho việc vẽ cây thư mục)
        [HttpGet("{id}/tree")]
        public async Task<IActionResult> GetBuildingTree(int id)
        {
            var tree = await _service.GetBuildingTreeAsync(id);
            if (tree == null)
            {
                throw new ApartmentManagement.Exceptions.AppException("Không tìm thấy tòa nhà.", StatusCodes.Status404NotFound);
            }
            return Ok(tree);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuildingDTO dto)
        {
            var result = await _service.CreateBuildingAsync(dto);
            return Ok(result);
        }
    }
}
