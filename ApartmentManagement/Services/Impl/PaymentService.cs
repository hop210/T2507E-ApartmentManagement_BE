using ApartmentManagement.DTOs.Payment;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Microsoft.EntityFrameworkCore;
using ApartmentManagement.Data;
using ApartmentManagement.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ApartmentManagement.Services.Impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ApplicationDbContext _context;

        public PaymentService(IPaymentRepository paymentRepository, ApplicationDbContext context)
        {
            _paymentRepository = paymentRepository;
            _context = context;
        }

        public async Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return payments.Select(p => new PaymentDTO
            {
                Id = p.Id,
                InvoiceId = p.InvoiceId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                ReferenceCode = p.ReferenceCode
            });
        }

        public async Task<PaymentDTO> CreatePaymentAsync(CreatePaymentDTO dto)
        {
            // 1. Lấy thông tin Hóa đơn
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId);

            if (invoice == null)
            {
                // Thay bằng AppException 404
                throw new AppException("Không tìm thấy hóa đơn cần thanh toán.", StatusCodes.Status404NotFound);
            }

            if (invoice.Status == InvoiceStatus.Paid)
            {
                // Thay bằng AppException 400
                throw new AppException("Hóa đơn này đã được thanh toán đầy đủ trước đó.", StatusCodes.Status400BadRequest);
            }

            // 2. TÍNH TOÁN NGĂN CHẶN THANH TOÁN LỐ 
            decimal currentPaid = invoice.Payments?.Sum(p => p.Amount) ?? 0;
            decimal remainingDebt = invoice.TotalAmount - currentPaid;

            if (dto.Amount > remainingDebt)
            {
                // Thay bằng AppException 400
                throw new AppException($"Số tiền đóng ({dto.Amount:N0}đ) đang vượt quá số nợ còn lại ({remainingDebt:N0}đ).", StatusCodes.Status400BadRequest);
            }

            // 3. Lưu lịch sử thanh toán
            var payment = new Payment
            {
                InvoiceId = dto.InvoiceId,
                Amount = dto.Amount,
                PaymentDate = DateTime.Now,
                PaymentMethod = dto.PaymentMethod,
                ReferenceCode = dto.ReferenceCode
            };

            await _paymentRepository.AddAsync(payment);

            // 4. Kiểm tra xem đã đóng đủ 100% tiền chưa để đổi trạng thái Hóa đơn
            if ((currentPaid + dto.Amount) >= invoice.TotalAmount)
            {
                invoice.Status = InvoiceStatus.Paid;
                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();
            }

            return new PaymentDTO
            {
                Id = payment.Id,
                InvoiceId = payment.InvoiceId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod,
                ReferenceCode = payment.ReferenceCode
            };
        }
    }
}