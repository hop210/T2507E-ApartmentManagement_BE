using ApartmentManagement.DTOs.Invoice;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _service;

        public InvoicesController(IInvoiceService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _service.GetAllInvoicesAsync();
            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _service.GetInvoiceByIdAsync(id);
            if (invoice == null) return NotFound("Không tìm thấy hóa đơn này.");
            return Ok(invoice);
        }

        //  Chỉ có Ban quản lý mới được phép chốt hóa đơn
        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.CreateInvoiceAsync(dto);
                return Ok(new { Message = "Chốt hóa đơn thành công!", Data = result });
            }
            catch (InvalidOperationException ex)
            {
                // Các lỗi nghiệp vụ như: phòng trống, đã xuất hóa đơn rồi...
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống
                return StatusCode(500, new { message = "Lỗi hệ thống: " + ex.Message });
            }
        }
    }
}