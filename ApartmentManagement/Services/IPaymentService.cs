using ApartmentManagement.DTOs.Payment;

namespace ApartmentManagement.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetAllPaymentsAsync();
        Task<PaymentDTO> CreatePaymentAsync(CreatePaymentDTO dto);
    }
}