using ApartmentManagement.Data;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.Include(p => p.Invoice).ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.Include(p => p.Invoice).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }
    }
}