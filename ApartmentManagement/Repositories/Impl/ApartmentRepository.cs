using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Apartment.Parameters;
using ApartmentManagement.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApartmentManagement.Repositories.Impl
{
    public class ApartmentRepository : IApartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public ApartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Apartment>> GetAllAsync(ApartmentParameters parameters)
        {
            // Bắt đầu tạo câu truy vấn (chưa gọi xuống DB ngay)
            var query = _context.Apartments.AsQueryable();

            // 1. Lọc theo Trạng thái (Nếu có gửi lên)
            if (parameters.Status.HasValue)
            {
                query = query.Where(a => a.Status == parameters.Status.Value);
            }

            // 2. Lọc theo Giá tối thiểu
            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(a => a.RentPrice >= parameters.MinPrice.Value);
            }

            // 3. Lọc theo Giá tối đa
            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(a => a.RentPrice <= parameters.MaxPrice.Value);
            }

            // 4. Phân trang (Skip và Take) & Thực thi truy vấn
            return await query
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
        }

        public async Task<Apartment> AddAsync(Apartment apartment)
        {
            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();
            return apartment;
        }
        public async Task<Apartment?> GetByIdAsync(int id)
        {
            return await _context.Apartments.FindAsync(id);
        }

        public async Task UpdateAsync(Apartment apartment)
        {
            _context.Apartments.Update(apartment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Apartment apartment)
        {
            _context.Apartments.Remove(apartment);
            await _context.SaveChangesAsync();
        }
    }
}