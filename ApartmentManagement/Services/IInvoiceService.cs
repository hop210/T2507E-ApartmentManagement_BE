using ApartmentManagement.DTOs.Invoice;

namespace ApartmentManagement.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<InvoiceDTO>> GetAllInvoicesAsync();
        Task<InvoiceDTO?> GetInvoiceByIdAsync(int id);
        Task<InvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO dto);
    }
}