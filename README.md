# 🏢 Apartment Management API

Hệ thống Backend quản lý khu chung cư, cung cấp các API chuẩn RESTful để quản lý tòa nhà, căn hộ, cư dân và tích hợp hệ thống xác thực bảo mật.

## 🚀 Công nghệ sử dụng
* **Framework:** .NET 10 (ASP.NET Core Web API)
* **Database:** SQL Server & Entity Framework Core
* **Authentication/Authorization:** JSON Web Token (JWT) & Role-based Auth
* **Documentation:** Swagger (OpenAPI v2.x chuẩn .NET 10)
* **Security:** Isopoh.Cryptography.Argon2 (Mã hóa mật khẩu)

## ✨ Tính năng nổi bật
* **Quản lý cốt lõi:** Thêm, sửa, xóa, thống kê (Tòa nhà, Căn hộ, Cư dân).
* **Bảo mật JWT:** Hệ thống cấp phát Token an toàn cho các phiên đăng nhập.
* **Phân quyền vai trò (Role-Based Access Control):** 
  * `ADMIN`: Toàn quyền quản trị hệ thống.
  * `MANAGER`: Quản lý cấp trung (Thêm, Sửa dữ liệu).
  * `RESIDENT`: Cư dân (Chỉ có quyền xem dữ liệu).
* **Auto Data Seeding:** Tự động khởi tạo tài khoản quản trị viên (`admin`) khi hệ thống khởi chạy lần đầu.
* **CORS Policy:** Đã cấu hình mở cổng kết nối an toàn cho các ứng dụng Frontend.

## 🛠 Hướng dẫn chạy dự án

1. **Clone repository:**
   ```bash
   git clone <link-repo-cua-ban>
   cd ApartmentManagement
   ```

2. **Cấu hình Database:**
   Mở file `appsettings.json` và cập nhật chuỗi kết nối `DefaultConnection` cho phù hợp với SQL Server của bạn.

3. **Cập nhật Database (Migration):**
   ```bash
   dotnet ef database update
   ```

4. **Chạy ứng dụng:**
   ```bash
   dotnet run
   ```
   *Tài khoản Admin mặc định để test API:*
   * **Username:** `admin`
   * **Password:** `admin123`

5. **Test API:**
   Truy cập `https://localhost:<port>/swagger` để sử dụng giao diện Swagger UI. Nhập Token lấy được từ API Login vào nút **Authorize** để mở khóa các API yêu cầu quyền quản trị.