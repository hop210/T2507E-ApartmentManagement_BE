using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Invoice;
using ApartmentManagement.Entities;
using ApartmentManagement.Enums;
using ApartmentManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Services.Impl
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly ApplicationDbContext _context; // Dùng để truy vấn chéo các bảng

        public InvoiceService(IInvoiceRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<InvoiceDTO>> GetAllInvoicesAsync()
        {
            var invoices = await _repository.GetAllAsync();
            return invoices.Select(MapToDTO);
        }

        public async Task<InvoiceDTO?> GetInvoiceByIdAsync(int id)
        {
            var invoice = await _repository.GetByIdAsync(id);
            return invoice == null ? null : MapToDTO(invoice);
        }

        public async Task<InvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO dto)
        {
            // 1. Kiểm tra xem tháng này đã xuất hóa đơn chưa
            var existingInvoice = await _context.Invoices
                .FirstOrDefaultAsync(i => i.ApartmentId == dto.ApartmentId && i.Month == dto.Month && i.Year == dto.Year);

            if (existingInvoice != null)
            {
                throw new InvalidOperationException($"Căn hộ {dto.ApartmentId} đã có hóa đơn trong tháng {dto.Month}/{dto.Year}");
            }

            // 2. Tìm Hợp đồng đang hiệu lực để lấy tiền nhà
            var activeContract = await _context.Contracts
                .FirstOrDefaultAsync(c => c.ApartmentId == dto.ApartmentId && c.Status == ContractStatus.Active);

            if (activeContract == null)
            {
                throw new InvalidOperationException("Không thể tạo hóa đơn vì căn hộ này chưa có hợp đồng thuê nhà đang hiệu lực.");
            }

            // 3. Lấy toàn bộ chỉ số điện/nước đã chốt trong tháng
            var usages = await _context.UtilityUsages
                .Include(u => u.Utility)
                .Where(u => u.ApartmentId == dto.ApartmentId && u.Month == dto.Month && u.Year == dto.Year)
                .ToListAsync();

            // 4. Bắt đầu tính toán
            decimal totalAmount = 0;
            var invoiceDetails = new List<InvoiceDetail>();

            // 4.1. Thêm tiền thuê nhà vào chi tiết
            invoiceDetails.Add(new InvoiceDetail
            {
                Description = "Tiền thuê nhà",
                Amount = activeContract.RentAmount
            });
            totalAmount += activeContract.RentAmount;

            // 4.2. Duyệt qua từng dịch vụ (Điện, nước) để tính tiền
            foreach (var usage in usages)
            {
                if (usage.Utility != null)
                {
                    decimal amount = (decimal)usage.UsageAmount * usage.Utility.UnitPrice;

                    invoiceDetails.Add(new InvoiceDetail
                    {
                        Description = $"Tiền {usage.Utility.Name} ({usage.UsageAmount} {usage.Utility.Unit})",
                        Amount = amount
                    });

                    totalAmount += amount;
                }
            }

            // 5. Khởi tạo đối tượng Hóa đơn tổng
            var invoice = new Invoice
            {
                ApartmentId = dto.ApartmentId,
                Month = dto.Month,
                Year = dto.Year,
                TotalAmount = totalAmount,
                Status = InvoiceStatus.Unpaid,
                DueDate = dto.DueDate,
                InvoiceDetails = invoiceDetails // EF Core sẽ tự động lưu cả cha lẫn con
            };

            var created = await _repository.AddAsync(invoice);

            return MapToDTO(created);
        }

        // Hàm Mapping dùng chung cho gọn code
        private InvoiceDTO MapToDTO(Invoice invoice)
        {
            return new InvoiceDTO
            {
                Id = invoice.Id,
                ApartmentId = invoice.ApartmentId,
                ApartmentNumber = invoice.Apartment?.ApartmentNumber ?? "",
                Month = invoice.Month,
                Year = invoice.Year,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status.ToString(),
                CreatedAt = invoice.CreatedAt,
                DueDate = invoice.DueDate,
                Details = invoice.InvoiceDetails?.Select(d => new InvoiceDetailDTO
                {
                    Id = d.Id,
                    Description = d.Description,
                    Amount = d.Amount
                }).ToList() ?? new List<InvoiceDetailDTO>()
            };
        }
    }
}