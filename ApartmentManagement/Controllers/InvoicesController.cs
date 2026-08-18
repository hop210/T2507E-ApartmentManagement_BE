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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDTO dto)
        {
            try
            {
                var result = await _service.CreateInvoiceAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Bắt lỗi nghiệp vụ (ví dụ: đã xuất hóa đơn rồi, chưa có hợp đồng...)
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}