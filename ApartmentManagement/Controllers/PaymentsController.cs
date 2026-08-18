using ApartmentManagement.DTOs.Payment;
using ApartmentManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApartmentManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var payments = await _service.GetAllPaymentsAsync();
            return Ok(payments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDTO dto)
        {
            try
            {
                var result = await _service.CreatePaymentAsync(dto);
                return Ok(new { message = "Ghi nhận thanh toán thành công!", data = result });
            }
            catch (Exception ex)
            {
                // Bắt gọn lỗi thanh toán lố hoặc lỗi không tìm thấy hóa đơn trả về FE
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}