using ApartmentManagement.Data;
using ApartmentManagement.DTOs.Auth;
using ApartmentManagement.Entities;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApartmentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Bơm DbContext để gọi Database, bơm IConfiguration để lấy Secret Key trong appsettings.json
        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            // 1. Kiểm tra xem Username đã có ai xài chưa
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
            {
                throw new ApartmentManagement.Exceptions.AppException("Tên đăng nhập này đã tồn tại!", StatusCodes.Status409Conflict);
            }

            // 2. Mã hóa mật khẩu bằng Argon2 xịn sò
            string hashedPassword = Argon2.Hash(dto.Password);

            // 3. Tạo User mới
            var newUser = new User
            {
                Username = dto.Username,
                PasswordHash = hashedPassword,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = "RESIDENT" // Mặc định đăng ký là Cư dân
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok("Đăng ký tài khoản thành công!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            // 1. Tìm user trong Database
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                throw new ApartmentManagement.Exceptions.AppException("Tài khoản hoặc mật khẩu không chính xác.", StatusCodes.Status401Unauthorized);
            }

            // 2. Kiểm tra mật khẩu bằng Argon2
            if (!Argon2.Verify(user.PasswordHash, dto.Password))
            {
                throw new ApartmentManagement.Exceptions.AppException("Tài khoản hoặc mật khẩu không chính xác.", StatusCodes.Status401Unauthorized);
            }

            // 3. Nếu đúng, bắt đầu quy trình chế tạo Thẻ thông hành (JWT)
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, Message = "Đăng nhập thành công!" });
        }

        // Hàm bí mật dùng để đúc Token
        private string GenerateJwtToken(User user)
        {
            // Lấy thông tin từ file appsettings.json
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            // Nhét thông tin cá nhân (Claims) vào trong thẻ
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // Dòng này cực kỳ quan trọng để sau này phân quyền
            };

            // Thiết lập hạn sử dụng của thẻ (Ví dụ: 1 ngày)
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Trả về chuỗi Token hoàn chỉnh
            return tokenHandler.WriteToken(token);
        }
    }
}