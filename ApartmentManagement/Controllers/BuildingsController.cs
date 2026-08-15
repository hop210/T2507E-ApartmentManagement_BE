using ApartmentManagement.DTOs.Building;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{ 

[Route("api/[controller]")]
[ApiController]
public class BuildingsController : ControllerBase
{
        private readonly IBuildingService _service;

        public BuildingsController(IBuildingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var buildings = await _service.GetAllBuildingsAsync();
            return Ok(buildings);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBuildingDTO dto)
        {
            var result = await _service.CreateBuildingAsync(dto);
            return Ok(result);
        }
    }
}
